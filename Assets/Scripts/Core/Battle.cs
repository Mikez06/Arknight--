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
    public float Cost;

    public Map Map = new Map();

    public List<WaveData> Waves = new List<WaveData>();

    public int EnemyCount;

    public List<Units.干员> PlayerUnits = new List<Units.干员>();

    public List<Unit> Enemys = new List<Unit>();

    public CountDown CostCounting = new CountDown(1);

    public bool Finish;

    public bool Win;

    public HashSet<Unit>[,] UnitMap;//敌人快速检索缓存

    public HashSet<Bullet> Bullets = new HashSet<Bullet>();

    public System.Random Random;

    public void Init(BattleInput battleConfig)
    {
        Random = new System.Random(battleConfig.Seed);

        Cost = battleConfig.StartCost;

        //读取场景地图信息
        Map.Init(this);

        for (int i = 0; i < battleConfig.Team.Cards.Count; i++)
        {
            Card unitInput = battleConfig.Team.Cards[i];
            CreatePlayerUnit(unitInput, battleConfig.Team.UnitSkill[i]);
        }
     
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

        foreach (var wave in Database.Instance.GetAll<WaveData>())
        {
            if (wave.Map == battleConfig.MapName) Waves.Add(wave);
        }
        Waves.Sort((x, y) => Math.Sign(x.Delay - y.Delay));
        EnemyCount = Waves.Count;
    }

    public void Update()
    {
        updateFinish();
        if (Finish) return;
        Tick++;
        updateUnitMap();
        if (CostCounting.Update(SystemConfig.DeltaTime))
        {
            Cost++;
            CostCounting.Set(1);
        }
        while (Waves.Count > 0 && Tick * SystemConfig.DeltaTime > Waves[0].Delay)
        {
            var wave = Waves[0];
            Waves.RemoveAt(0);
            var enemy = CreateEnemy(wave);
            TriggerDatas.Push(new TriggerData()
            {
                Target = enemy,
            });
            Trigger(TriggerEnum.入场);
            TriggerDatas.Pop();
        }

        foreach (var tile in Map.Tiles)
        {
            tile.Update();
        }

        foreach (var unit in Enemys)
        {
            unit.UpdateCollision();
        }

        foreach (var bullet in Bullets.ToArray())
        {
            bullet.Update();
        }

        foreach (var unit in Enemys.Union(PlayerUnits).ToArray())
        {
            unit.UpdatePush();
        }
        foreach (var unit in Enemys.Union(PlayerUnits).ToArray())
        {
            unit.UpdateBuffs();
        }
        foreach (var unit in Enemys.Union(PlayerUnits).ToArray())
        {
            unit.UpdateAction();
        }
        foreach (var unit in Enemys.ToArray())
        {
            if (unit.State==StateEnum.Die&& unit.Dying.Finished())
            {
                Enemys.Remove(unit);
                unit.Finish();
            }
        }
    }

    public Units.干员 CreatePlayerUnit(Card card,int skill)
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
        return unit;
    }

    public Units.敌人 CreateEnemy(WaveData waveConfig)
    {
        var config = Database.Instance.Get<UnitData>(waveConfig.UnitId.Value);
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + config.Type) as Units.敌人;
        unit.Id = waveConfig.UnitId.Value;
        unit.WaveId = Database.Instance.GetIndex(waveConfig);
        unit.Battle = this;
        unit.Init();
        //var grid = Map.Grids[waveConfig.Path, y];
        //unit.Position = grid.transform.position + new Vector3(0, config.Height, 0);
        Enemys.Add(unit);
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
        result.Postion = startPos;
        result.TargetPos = targetPos;
        result.Target = target;
        result.Skill = skill;
        Bullets.Add(result);
        result.Init();
        return result;
    }

    public HashSet<Unit> FindAll(Vector2Int point,int team,bool aliveOnly=true)
    {
        var result = new HashSet<Unit>();
        if (team % 2 == 1)
        {
            var target = Map.Tiles[point.x, point.y].Unit;
            if (target != null)
            {
                if (!aliveOnly || target.Alive())
                    result.Add(target);
            }
        }
        else if ((team >> 1) % 2 == 1)
        {
            foreach (var unit in UnitMap[point.x, point.y])
            {
                if (!aliveOnly || unit.Alive())
                    result.Add(unit);
            }
        }
        return result;
    }

    public HashSet<Unit> FindAll(List<Vector2Int> points, int team)
    {
        var result = new HashSet<Unit>();
        foreach (var point in points)
        {
            var targetPoint = point;
            if (team  % 2 == 1)
            {
                var target = Map.Tiles[targetPoint.x, targetPoint.y].Unit;
                if (target != null)
                {
                    if (target.Alive())
                        result.Add(target);
                }
            }
            else if ((team >> 1) % 2 == 1)
            {
                foreach (var unit in UnitMap[targetPoint.x, targetPoint.y])
                {
                    if (unit.Alive())
                        result.Add(unit);
                }
            }
        }
        return result;
    }

    public HashSet<Unit> FindAll(Vector2 pos, float radius, int team)
    {
        HashSet<Unit> result = new HashSet<Unit>();
        if (team%2 == 1)
        {
            var units = PlayerUnits.Where(x => x.InputTime >= 0 && x.Alive()).ToList();
            foreach (var unit in units) //需要优化！
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.UnitData.Radius
                    ) result.Add(unit);
            }
        }
        else if ((team >> 1) % 2 == 1)
        {
            foreach (var unit in Enemys) //需要优化！
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.UnitData.Radius && unit.Alive()) result.Add(unit);
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
                    if (i > 0 && i < UnitMap.GetLength(0) && j > 0 && j < UnitMap.GetLength(1))
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

    public void DoDamage(int count)
    {
        Hp -= count;
        Hurt += count;
        if (Hp <= 0)
        {
            Finish = true;
        }
    }

    public void Trigger(TriggerEnum triggerEnum)
    {
        foreach (var unit in PlayerUnits)
        {
            unit.Trigger(triggerEnum);
        }
        foreach (var enemy in Enemys)
        {
            enemy.Trigger(triggerEnum);
        }
    }
}

