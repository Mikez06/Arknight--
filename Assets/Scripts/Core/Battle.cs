using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Battle
{
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

    public HashSet<Unit>[,] UnitMap;//敌人快速检索缓存
    public Units.干员[,] PlayerUnitsMap;//干员快速检索缓存

    public void Init(BattleInput battleConfig)
    {
        if (battleConfig.UnitInputs != null)
            foreach (var unitInput in battleConfig.UnitInputs)
            {
                CreatePlayerUnit(unitInput.Id);
            }

        //读取场景地图信息
        Map.Init();
        
        UnitMap = new HashSet<Unit>[Map.Grids.GetLength(0), Map.Grids.GetLength(1)];
        PlayerUnitsMap = new Units.干员[Map.Grids.GetLength(0), Map.Grids.GetLength(1)];
        for (int i = 0; i < UnitMap.GetLength(0); i++)
            for (int j = 0; j < UnitMap.GetLength(1); j++)
            {
                UnitMap[i, j] = new HashSet<Unit>();
            }

        foreach (var wave in Database.Instance.GetAll<WaveConfig>())
        {
            if (wave.Map == battleConfig.MapName) Waves.Add(wave);
        }
        EnemyCount = Waves.Count;
        PlayerUnits[0].ChangePos(5, 3, DirectionEnum.Down);
        PlayerUnits[0].JoinMap();
    }

    public void Update()
    {
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
        foreach (var unit in Enemys.Union(PlayerUnits))
        {
            unit.UpdateBuffs();
        }
        foreach (var unit in Enemys.Union(PlayerUnits))
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

    public HashSet<Unit> FindEnemys(Vector2Int pos,Vector2Int[] points)
    {
        var result = new HashSet<Unit>();
        foreach (var point in points)
        {
            var targetPoint = pos + point;
            if (targetPoint.x>=0 && targetPoint.x<UnitMap.GetLength(0) && targetPoint.y>=0 && targetPoint.y < UnitMap.GetLength(1))
            {
                foreach (var unit in UnitMap[targetPoint.x, targetPoint.y])
                {
                    result.Add(unit);
                }
            }
        }
        return result;
    }

    public HashSet<Unit> FindPlayerUnits(Vector2Int pos, Vector2Int[] points)
    {
        var result = new HashSet<Unit>();
        foreach (var point in points)
        {
            var targetPoint = pos + point;
            if (PlayerUnitsMap[targetPoint.x, targetPoint.y] != null)
            {
                result.Add(PlayerUnitsMap[targetPoint.x, targetPoint.y]);
            }
        }
        return result;
    }

    public Units.干员 FindPlayerUnits(Vector2Int pos)
    {
        return PlayerUnitsMap[pos.x, pos.y];
    }

    void updateUnitMap()
    {
        foreach (var tile in UnitMap)
        {
            tile.Clear();
        }
        foreach (var unit in Enemys)
        {
            for (int i = Mathf.FloorToInt(unit.Position2.x - unit.Config.Radius); i < Mathf.CeilToInt(unit.Position2.x + unit.Config.Radius); i++)
            {
                for (int j = Mathf.FloorToInt(unit.Position2.y - unit.Config.Radius); j < Mathf.CeilToInt(unit.Position2.y + unit.Config.Radius); j++)
                {
                    if (i > 0 && i < UnitMap.GetLength(0) && j > 0 && j < UnitMap.GetLength(1))
                        UnitMap[i, j].Add(unit);
                }

            }
        }
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

