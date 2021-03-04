using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Battle
{
    public int Hp;
    public int Hurt;
    public int Tick = -1;

    public Map Map = new Map();

    public List<WaveConfig> Waves = new List<WaveConfig>();

    public List<Units.干员> PlayerUnits = new List<Units.干员>();

    public HashSet<Unit> Units = new HashSet<Unit>();

    public bool Finish;

    protected HashSet<Unit>[,] UnitMap;//快速检索缓存

    public void Init(BattleConfig battleConfig)
    {
        //读取场景地图信息
        Map.Init();
        
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
    }

    public void Update()
    {
        Tick++;
        while (Tick * SystemConfig.DeltaTime > Waves[0].Delay)
        {
            var wave = Waves[0];
            Waves.RemoveAt(0);
            CreateEnemy(wave);
        }
        foreach (var unit in Units.ToArray())
        {
            unit.UpdateBuffs();
        }
        foreach (var unit in Units.ToArray())
        {
            unit.UpdateAction();
        }
        foreach (var unit in Units.ToArray())
        {
            if (unit.State==UnitStateEnum.Die&& unit.Dying.Finished())
            {
                Units.Remove(unit);
                unit.Destroy();
            }
        }
    }

    public Units.干员 CreatePlayerUnit(int id, int x, int y,DirectionEnum direction)
    {
        var config = Database.Instance.Get<UnitConfig>(id);
        var unit = typeof(Battle).Assembly.CreateInstance(nameof(Units) + "." + config.Type) as Units.干员;
        unit.Direction = direction;
        unit.Battle = this;
        unit.Init();
        var grid = Map.Grids[x, y];
        unit.Position = grid.transform.position + new Vector3(0, config.Height, 0);
        Units.Add(unit);
        return unit;
    }

    public Units.敌人 CreateEnemy(WaveConfig waveConfig)
    {
        return null;
    }

    public Unit FindNearTarget(Vector2 pos,Func<Unit,bool> selectable)
    {
        Unit result = null;
        float distance = float.MaxValue;
        foreach (var unit in Units) //TODO 遍历效率优化
        {
            if (!selectable(unit)) continue;
            var d = (unit.Position2 - pos).magnitude;
            if (d < distance)
            {
                result = unit;
                distance = d;
            }
        }
        return result;
    }

    public HashSet<Unit> FindTargets(Vector2Int pos,Vector2Int[] points)
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

    void updateUnitMap()
    {
        foreach (var tile in UnitMap)
        {
            tile.Clear();
        }
        foreach (var unit in Units)
        {
            for (int i = Mathf.FloorToInt(unit.Position2.x - unit.Config.Radius); i < Mathf.CeilToInt(unit.Position2.x + unit.Config.Radius); i++)
            {
                for (int j = Mathf.FloorToInt(unit.Position2.y - unit.Config.Radius); j < Mathf.CeilToInt(unit.Position2.y + unit.Config.Radius); j++)
                {
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

