using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Battle
{
    public Stack<TriggerData> TriggerDatas = new Stack<TriggerData>();
    public int Hp = 10;
    public int Hurt;
    public int Tick = -1;
    public int WaveTick;
    public string WaveTag = "";
    public float Cost;

    public Map Map = new Map();

    //public MapData MapData;
    public MapInfo MapData;

    public List<OneWave> Waves = new List<OneWave>();
    public List<OneWave> CheckPointWaves = new List<OneWave>();

    public List<MapUnitInfo> SceneUnits = new List<MapUnitInfo>();

    public int EnemyCount;

    Unit RuleUnit;

    public List<Units.干员> PlayerUnits = new List<Units.干员>();
    public List<Unit> PlayerUnits2 = new List<Unit>();

    public List<Unit> Enemys = new List<Unit>();

    public List<Unit> AllUnits = new List<Unit>();

    public CountDown CostCounting = new CountDown(1);

    public float CostCountSpeed = 1;

    public bool Finish;

    public bool Win;

    public HashSet<Unit>[,] UnitMap;//敌人快速检索缓存

    public HashSet<Bullet> Bullets = new HashSet<Bullet>();

    public System.Random Random;

    public int BuildCount;

    public int TeamLimit = 99;
    public HashSet<UnitTypeEnum> ProfessionLimit = new HashSet<UnitTypeEnum>();
    public List<int> CheckPoints = new List<int>();

    public void Init(BattleInput battleConfig)
    {
        MapData = Database.Instance.GetMap(battleConfig.MapName);
        Hp = MapData.InitHp;
        Random = new System.Random(battleConfig.Seed);

        RuleUnit = new Unit();
        RuleUnit.Battle = this;
        RuleUnit.Init();
        foreach (var contracrId in battleConfig.Contracts)
        {
            var contract = Database.Instance.Get<ContractData>(contracrId);
            if (contract.MapHp != 0) Hp = contract.MapHp;
            if (contract.TeamLimit > 0 && TeamLimit > contract.TeamLimit) TeamLimit = contract.TeamLimit;
            if (contract.ProfessionLimit != null) foreach (var p in contract.ProfessionLimit) ProfessionLimit.Add(p);
            var skills = contract.Skills;
            if (skills != null)
                foreach (var skillId in skills)
                {
                    RuleUnit.LearnSkill(skillId, null);
                }
        }

        Cost = MapData.InitCost;

        //读取场景地图信息
        Map.Init(this);
        //读取中立单位信息
        foreach (var u in MapData.UnitInfos)
        {
            SceneUnits.Add(new MapUnitInfo()
            {
                Time = u.ActiveTime,
                Id = u.UnitId,
                Tag = u.Tag,
                Pos = Map.Tiles[u.X, u.Y].MapGrid.transform.position,
                Direction = u.Direction,
            });
        }

        if (battleConfig.Team != null)
        {
            for (int i = 0; i < battleConfig.Team.Cards.Count; i++)
            {
                if (i >= TeamLimit) continue;
                Card unitInput = battleConfig.Team.Cards[i];
                if (ProfessionLimit.Contains(unitInput.UnitData.Profession)) continue;
                CreatePlayerUnit(unitInput, battleConfig.Team.UnitSkill[i]);
            }
            BuildCount = MapData.MaxBuildCount;
        }
        else
        {
            foreach (var card in battleConfig.Dungeon.Cards)
            {
                CreatePlayerUnit(card, card.UsingSkill);
            }
            foreach (var relic in battleConfig.Dungeon.Relics)
            {
                var skills = relic.RelicData.Skills;
                if (skills != null)
                    foreach (var skillId in skills)
                    {
                        RuleUnit.LearnSkill(skillId, null);
                    }
            }
            BuildCount = battleConfig.Dungeon.MaxBuildCount;
        }

        Trigger(TriggerEnum.起始);
        foreach (var unit in PlayerUnits)
        {
            TriggerDatas.Push(new TriggerData()
            {
                Target = unit,
            });
            Trigger(TriggerEnum.出场);
            TriggerDatas.Pop();
        }

        UnitMap = new HashSet<Unit>[Map.Tiles.GetLength(0), Map.Tiles.GetLength(1)];
        for (int i = 0; i < UnitMap.GetLength(0); i++)
            for (int j = 0; j < UnitMap.GetLength(1); j++)
            {
                UnitMap[i, j] = new HashSet<Unit>();
            }

        //WaveData[] array = Database.Instance.GetAll<WaveData>();
        //for (int id = 0; id < array.Length; id++)
        foreach (var wave in MapData.WaveInfos)
        {
            //WaveData wave = array[id];
            for (int i = 0; i < wave.Count; i++)
            {
                var waveInfo = new OneWave() { WaveData = wave, Time = wave.Delay + wave.GapTime * i };
                if (wave.CheckPoint == 0)
                    Waves.Add(waveInfo);
                else
                    CheckPointWaves.Add(waveInfo);
            }
        }
        Waves.Sort((x, y) => Math.Sign(x.Time - y.Time));
        EnemyCount = Waves.Where(x => x.WaveData.sUnitId != null).Count() + CheckPointWaves.Where(x => x.WaveData.sUnitId != null).Count();

        checkSceneUnit();

        //这里刷新下单位状态，有些开场附加数据需要刷新
        foreach (var unit in PlayerUnits)
        {
            unit.Refresh();
        }
    }

    public void Update()
    {
        updateFinish();
        if (Finish) return;
        Tick++;
        WaveTick++;
        checkSceneUnit();
        updateUnitMap();
        if (CostCounting.Update(SystemConfig.DeltaTime * CostCountSpeed))
        {
            Cost++;
            CostCounting.Set(1);
        }
        if (Cost > MapData.MaxCost) Cost = MapData.MaxCost;
        while (Waves.Count > 0 && Tick * SystemConfig.DeltaTime > Waves[0].Time)
        {
            var wave = Waves[0];
            Waves.RemoveAt(0);
            createWave(wave);
        }
        for (int i = CheckPointWaves.Count-1; i >= 0; i--)
        {
            OneWave checkPointWave = CheckPointWaves[i];
            if (CheckPoints.Count >= checkPointWave.WaveData.CheckPoint && (Tick - CheckPoints[checkPointWave.WaveData.CheckPoint - 1]) * SystemConfig.DeltaTime > checkPointWave.Time)
            {
                CheckPointWaves.RemoveAt(i);
                createWave(checkPointWave);
            }
        }

        foreach (var tile in Map.Tiles)
        {
            tile?.Update();
        }

        foreach (var bullet in Bullets.ToArray())
        {
            bullet.Update();
        }

        foreach (var unit in AllUnits.ToArray())
        {
            unit.UpdatePush();
        }
        foreach (var unit in AllUnits.ToArray())
        {
            unit.UpdateBuffs();
        }
        foreach (var unit in AllUnits.ToArray())
        {
            unit.UpdateAction();
        }
        foreach (var unit in Enemys)
        {
            unit.UpdateCollision();
        }
        foreach (var unit in Enemys.ToArray())
        {
            if (unit.State==StateEnum.Die&& unit.Dying.Finished())
            {
                Enemys.Remove(unit);
                AllUnits.Remove(unit);
                unit.Finish();
            }
        }
    }

    void checkSceneUnit()
    {
        for (int i = SceneUnits.Count - 1; i >= 0; i--)
        {
            var unit = SceneUnits[i];
            if (unit.Time <= WaveTick * SystemConfig.DeltaTime && (unit.Tag == WaveTag || string.IsNullOrEmpty(SceneUnits[i].Tag)))
            {
                CreateSceneUnit(unit.Id, unit.Pos, unit.Direction);
                SceneUnits.RemoveAt(i);
            }
        }
    }

    void createWave(OneWave wave)
    {
        if (wave.WaveData.sUnitId == null)
        {
            var PathPoints = MapData.PathInfos.Find(x => x.Name == wave.WaveData.Path).Path;
            List<Vector3> p = new List<Vector3>();
            for (int i = 0; i < PathPoints.Count - 1; i++)
            {
                var p1 = Map.FindPath(PathPoints[i].Pos, PathPoints[i + 1].Pos, PathPoints[i].DirectMove);
                p.AddRange(p1);
            }
            TrailManager.Instance.ShowPath(p);
        }
        else
        {
            var enemy = CreateEnemy(wave.WaveData);
            TriggerDatas.Push(new TriggerData()
            {
                Target = enemy,
            });
            Trigger(TriggerEnum.入场);
            TriggerDatas.Pop();
        }
    }

    public void SortSceneUnit()
    {
        SceneUnits = SceneUnits.OrderBy(x => x.Tag == WaveTag ? 0 : 1).ThenBy(x => x.Time).ToList();
    }

    public void ChangeWaveTag(string tag)
    {
        WaveTag = tag;
        WaveTick = 0;
        SortSceneUnit();
    }

    public Unit CreateSceneUnit(string id,Vector3 pos,Vector2 direction)
    {
        var unitData = Database.Instance.Get<UnitData>(id);
        if (unitData ==null) return null;
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + unitData.Type) as Unit;
        unit.Id = Database.Instance.GetIndex(unitData);
        unit.Battle = this;
        unit.Position = pos;
        unit.Direction = direction;
        unit.Init();
        if (!unit.UnitData.NotUseTile)
            Map.Tiles[(int)pos.x, (int)pos.z].Unit = unit;
        else
            Map.Tiles[(int)pos.x, (int)pos.z].MidUnit = unit;
        AllUnits.Add(unit);
        if (unit.Team == 0) PlayerUnits2.Add(unit);
        return unit;
    }

    public Units.干员 CreatePlayerUnit(ICard card,int skill)
    {
        var config = Database.Instance.Get<UnitData>(card.UnitId);
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + config.Type) as Units.干员;
        unit.Id = card.UnitId;
        unit.Card = card;
        unit.MainSkillId = skill;
        //unit.SetDirection(direction);
        unit.Battle = this;
        unit.Init();
        //var grid = Map.Grids[x, y];
        //unit.Position = grid.transform.position + new Vector3(0, config.Height, 0);
        PlayerUnits.Add(unit);
        AllUnits.Add(unit);
        return unit;
    }

    public Units.干员 CreatePlayerUnit(int id)
    {
        var config = Database.Instance.Get<UnitData>(id);
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + config.Type) as Units.干员;
        unit.Id = id;
        //unit.SetDirection(direction);
        unit.Battle = this;
        unit.Init();
        //var grid = Map.Grids[x, y];
        //unit.Position = grid.transform.position + new Vector3(0, config.Height, 0);
        PlayerUnits.Add(unit);
        AllUnits.Add(unit);
        return unit;
    }

    public Units.敌人 CreateEnemy(WaveInfo waveConfig)
    {
        var config = Database.Instance.Get<UnitData>(waveConfig.sUnitId);
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + config.Type) as Units.敌人;
        unit.Id = Database.Instance.GetIndex<UnitData>(waveConfig.sUnitId);
        unit.WaveData = waveConfig;
        unit.Battle = this;
        unit.Init();
        //var grid = Map.Grids[waveConfig.Path, y];
        //unit.Position = grid.transform.position + new Vector3(0, config.Height, 0);
        Enemys.Add(unit);
        AllUnits.Add(unit);
        TriggerDatas.Push(new TriggerData()
        {
            Target = unit,
        });
        Trigger(TriggerEnum.出场);
        TriggerDatas.Pop();
        return unit;
    }

    public Bullet CreateBullet(int id, Vector3 startPos, Vector3 targetPos, Unit target, Skill skill)
    {
        var config = Database.Instance.Get<BulletData>(id);
        var result = typeof(Battle).Assembly.CreateInstance(nameof(Bullets) + "." + config.Type) as Bullet;
        result.Id = id;
        result.Position = startPos;
        result.TargetPos = targetPos;
        result.Target = target;
        result.Skill = skill;
        Bullets.Add(result);
        result.Init();
        return result;
    }
    public Bullet CreateBullet(int id, Vector3 startPos, Vector3 targetPos, Vector2 target, Skill skill)
    {
        var config = Database.Instance.Get<BulletData>(id);
        var result = typeof(Battle).Assembly.CreateInstance(nameof(Bullets) + "." + config.Type) as Bullet;
        result.Id = id;
        result.Position = startPos;
        result.TargetPos = targetPos;
        result.Target = null;
        result.Skill = skill;
        Bullets.Add(result);
        result.Init();
        return result;
    }

    public HashSet<Unit> FindAll(Vector2Int point,int team,bool aliveOnly=true)
    {
        var result = new HashSet<Unit>();
        if (Map.Tiles.GetLength(0) <= point.x || Map.Tiles.GetLength(1) <= point.y || point.x < 0 || point.y < 0) return result;
        if (team % 2 == 1)
        {
            var target = Map.Tiles[point.x, point.y].Unit;
            if (target != null)
            {
                if ((!aliveOnly || target.Alive()) && (team >> target.Team) % 2 == 1)
                    result.Add(target);
            }
        }
        if ((team >> 1) % 2 == 1)
        {
            foreach (var unit in UnitMap[point.x, point.y])
            {
                if ((!aliveOnly || unit.Alive()) && (team >> unit.Team) % 2 == 1)
                    result.Add(unit);
            }
        }
        //result.RemoveWhere(x => team >> x.Team != 1);
        return result;
    }

    public HashSet<Unit> FindAll(List<Vector2Int> points, int team, bool aliveOnly = true)
    {
        var result = new HashSet<Unit>();
        foreach (var point in points)
        {
            var targetPoint = point;
            if (targetPoint.x < 0 || targetPoint.y < 0 || targetPoint.x >= Map.Tiles.GetLength(0) || targetPoint.y >= Map.Tiles.GetLength(1)) continue;
            if (team  % 2 == 1)
            {
                var target = Map.Tiles[targetPoint.x, targetPoint.y].Unit;
                if (target != null)
                {
                    if ((!aliveOnly || target.Alive()) && (team >> target.Team) % 2 == 1)
                        result.Add(target);
                }
            }
            if ((team >> 1) % 2 == 1)
            {
                foreach (var unit in UnitMap[targetPoint.x, targetPoint.y])
                {
                    if ((!aliveOnly || unit.Alive()) && (team >> unit.Team) % 2 == 1)
                        result.Add(unit);
                }
            }
        }
        return result;
    }

    public HashSet<Unit> FindAll(Vector2 pos, float radius, int team, bool aliveOnly = true)
    {
        HashSet<Unit> result = new HashSet<Unit>();
        if (team%2 == 1)
        {
            var units = PlayerUnits.Where(x => !aliveOnly || x.InputTime >= 0).ToList();
            foreach (var unit in units) //需要优化！
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.UnitData.Radius
                    ) if ((!aliveOnly || unit.Alive() && (team >> unit.Team) % 2 == 1))
                        result.Add(unit);
            }
            foreach (var unit in PlayerUnits2)
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.UnitData.Radius
                   ) if ((!aliveOnly || unit.Alive() && (team >> unit.Team) % 2 == 1))
                        result.Add(unit);
            }
        }
        else if ((team >> 1) % 2 == 1)
        {
            foreach (var unit in Enemys) //需要优化！
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.UnitData.Radius) 
                    if ((!aliveOnly || unit.Alive()) && (team >> unit.Team) % 2 == 1)
                        result.Add(unit);
            }
        }
        else //中立单位
        {
            foreach (var unit in AllUnits) //需要优化！
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.UnitData.Radius)
                    if ((!aliveOnly || unit.Alive()) && (team >> unit.Team) % 2 == 1)
                        result.Add(unit);
            }
        }
        return result;
    }

    void updateUnitMap()
    {
        foreach (var tile in UnitMap)
        {
            tile.Clear();
        }
        foreach (var unit in Enemys)
        {
            for (int i = Mathf.RoundToInt(unit.Position2.x - unit.UnitData.Radius); i <= Mathf.RoundToInt(unit.Position2.x + unit.UnitData.Radius); i++)
            {
                for (int j = Mathf.RoundToInt(unit.Position2.y - unit.UnitData.Radius); j <= Mathf.RoundToInt(unit.Position2.y + unit.UnitData.Radius); j++)
                {
                    if (i >= 0 && i < UnitMap.GetLength(0) && j >= 0 && j < UnitMap.GetLength(1))
                        UnitMap[i, j].Add(unit);
                }
            }
        }
    }


    void updateFinish()
    {
        if (Finish) return;
        if (Hp <= 0)
        {
            Finish = true;
            Win = false;
        }
        else if (EnemyCount == 0)
        {
            Finish = true;
            Win = true;
        }
        if (Finish)
            BattleUI.UI_Battle.Instance.BattleEnd();
    }

    public void GiveUp()
    {
        Finish = true;
        Win = false;
        BattleUI.UI_Battle.Instance.BattleEnd();
    }

    public void DoDamage(int count)
    {
        if (Hp <= 0) return;
        Hp -= count;
        Hurt += count;
    }

    public void Trigger(TriggerEnum triggerEnum)
    {
        RuleUnit.Trigger(triggerEnum);
        //foreach (var unit in PlayerUnits.ToArray())
        //{
        //    unit.Trigger(triggerEnum);
        //}
        //foreach (var enemy in Enemys)
        //{
        //    enemy.Trigger(triggerEnum);
        //}
        foreach (var unit in AllUnits.ToArray())
        {
            unit.Trigger(triggerEnum);
        }
    }
}

