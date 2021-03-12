using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Battle
{
    public static Battle Instance;
    public int Hp = 10;
    public int Hurt;
    public int Tick = -1;
    public float Cost;

    public Map Map = new Map();

    public List<WaveConfig> Waves = new List<WaveConfig>();

    public int EnemyCount;

    public List<Units.干员> PlayerUnits = new List<Units.干员>();

    public List<Unit> Enemys = new List<Unit>();

    public CountDown CostCounting = new CountDown(1);

    public bool Finish;

    public bool Win;

    public int NowUnitIndex = 0;

    public HashSet<Unit>[,] UnitMap;//敌人快速检索缓存

    public HashSet<Bullet> Bullets = new HashSet<Bullet>();

    public System.Random Random;

    public void Init(BattleInput battleConfig)
    {
        Instance = this;

        Random = new System.Random(battleConfig.Seed);

        Cost = battleConfig.StartCost;

        //读取场景地图信息
        Map.Init();

        for (int i = 0; i < battleConfig.Team.Cards.Count; i++)
        {
            Card unitInput = battleConfig.Team.Cards[i];
            CreatePlayerUnit(unitInput, battleConfig.Team.UnitSkill[i]);
        }
     
        UnitMap = new HashSet<Unit>[Map.Grids.GetLength(0), Map.Grids.GetLength(1)];
        for (int i = 0; i < UnitMap.GetLength(0); i++)
            for (int j = 0; j < UnitMap.GetLength(1); j++)
            {
                UnitMap[i, j] = new HashSet<Unit>();
            }

        foreach (var wave in Database.Instance.GetAll<WaveConfig>())
        {
            if (wave.Map == battleConfig.MapName) Waves.Add(wave);
        }
        Waves.Sort((x, y) => Math.Sign(x.Delay - y.Delay));
        EnemyCount = Waves.Count;
        //PlayerUnits[0].ChangePos(5, 3, DirectionEnum.Right);
        //PlayerUnits[0].JoinMap();
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
            CreateEnemy(wave);
        }

        foreach (var bullet in Bullets.ToArray())
        {
            bullet.Update();
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
        var config = Database.Instance.Get<UnitConfig>(card.UnitId);
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
        var config = Database.Instance.Get<UnitConfig>(id);
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

    public Units.敌人 CreateEnemy(WaveConfig waveConfig)
    {
        var config = Database.Instance.Get<UnitConfig>(waveConfig.UnitId);
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + config.Type) as Units.敌人;
        unit.Id = waveConfig.UnitId;
        unit.WaveId = Database.Instance.GetIndex(waveConfig);
        unit.Battle = this;
        unit.Init();
        //var grid = Map.Grids[waveConfig.Path, y];
        //unit.Position = grid.transform.position + new Vector3(0, config.Height, 0);
        Enemys.Add(unit);
        return unit;
    }

    public Bullet CreateBullet(int id, Vector3 startPos, Vector3 targetPos, Unit target, Skill skill)
    {
        var config = Database.Instance.Get<BulletConfig>(id);
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

    public HashSet<Unit> FindUnits(List<Vector2Int> points, int team, Func<Unit, bool> match)
    {
        var result = new HashSet<Unit>();
        foreach (var point in points)
        {
            var targetPoint = point;
            if (team == 0)
            {
                if (Map.Grids[targetPoint.x, targetPoint.y].Unit != null)
                {
                    if (match(Map.Grids[targetPoint.x, targetPoint.y].Unit))
                        result.Add(Map.Grids[targetPoint.x, targetPoint.y].Unit);
                }
            }
            else
            {
                foreach (var unit in UnitMap[targetPoint.x, targetPoint.y])
                {
                    if (match(unit))
                        result.Add(unit);
                }
            }
        }
        return result;
    }

    public Unit FindFirst(List<Vector2Int> points, int team, Func<Unit, bool> match,Func<Unit,float> sort)
    {

        HashSet<Unit> units = new HashSet<Unit>();
        foreach (var targetPoint in points)
        {
            if (team == 0)
            {
                if (Map.Grids[targetPoint.x, targetPoint.y].Unit != null)
                {
                    if (match(Map.Grids[targetPoint.x, targetPoint.y].Unit))
                        units.Add(Map.Grids[targetPoint.x, targetPoint.y].Unit);
                }
            }
            else
            {
                foreach (var unit in UnitMap[targetPoint.x, targetPoint.y])
                {
                    if (match(unit))
                        units.Add(unit);
                }
            }
        }
        Unit result = null;
        float v = float.NegativeInfinity;
        foreach (var unit in units)
        {
            if (result == null || sort(unit) > v)
            {
                result = unit;
                v = sort(unit);
            }
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="radius"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public Unit FindFirst(Vector2 pos, float radius,int team, Func<Unit, bool> match, Func<Unit, float> sort)
    {
        if (team == 0)
        {
            var units = PlayerUnits.Where(x => x.MapIndex >= 0).ToList();
            units.Sort((x, y) => Math.Sign(sort(x) - sort(y)));
            foreach (var unit in units)
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.Config.Radius
                    && match(unit)) return unit;
            }
        }
        else
        {
            var units = new List<Unit>(Enemys);//Sort((x, y) => Math.Sign(sort(x) - sort(y)));
            if (sort != null)
            {
                units.Sort((x, y) => Math.Sign(sort(x) - sort(y)));
            }
            foreach (var unit in units)
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.Config.Radius
                    && match(unit)) return unit;
            }
        }
        return null;
    }

    public HashSet<Unit> FindAll(Vector2 pos, float radius, int team, Func<Unit, bool> match)
    {
        HashSet<Unit> result = new HashSet<Unit>();
        if (team == 0)
        {
            var units = PlayerUnits.Where(x => x.MapIndex >= 0).ToList();
            foreach (var unit in units)
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.Config.Radius
                    && match(unit)) result.Add(unit);
            }
        }
        else
        {
            foreach (var unit in Enemys)
            {
                if ((unit.Position2 - pos).magnitude < radius + unit.Config.Radius
                    && match(unit)) result.Add(unit);
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
            for (int i = Mathf.RoundToInt(unit.Position2.x - unit.Config.Radius); i <= Mathf.RoundToInt(unit.Position2.x + unit.Config.Radius); i++)
            {
                for (int j = Mathf.RoundToInt(unit.Position2.y - unit.Config.Radius); j <= Mathf.RoundToInt(unit.Position2.y + unit.Config.Radius); j++)
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
}

