using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Tile
{
    public Battle Battle => Map.Battle;
    public Map Map;
    public MapGrid MapGrid;
    public Vector3 Pos;
    public int X, Y;
    /// <summary>
    /// 能否造单位
    /// </summary>
    public bool CanBuildUnit;
    /// <summary>
    /// 远程格子
    /// </summary>
    public bool FarAttackGrid;
    /// <summary>
    /// 能否移动
    /// </summary>
    public bool CanMove;

    public Units.干员 Unit;
    /// <summary>
    /// 广搜临时数据
    /// </summary>
    public Tile PreGrid;

    public TileTypeEnum TileType;

    public virtual void Update()
    {

    }

    public virtual void Init(Map map, MapGrid mapGrid)
    {
        this.MapGrid = mapGrid;
        mapGrid.Tile = this;
        this.Map = map;
        this.MapGrid = mapGrid;
        this.Pos = mapGrid.transform.position;
        this.X = mapGrid.X;
        this.Y = mapGrid.Y;
        this.CanBuildUnit = mapGrid.CanBuildUnit;
        this.FarAttackGrid = mapGrid.FarAttackGrid;
        this.CanMove = mapGrid.CanMove;
    }

    public bool CanSet(Unit unit)
    {
        if (this.Unit != null) return false;
        if (CanBuildUnit)
            if (FarAttackGrid)
            {
                return unit.UnitData.CanSetHigh;
            }
            else
                return unit.UnitData.CanSetGround;
        return false;
    }
}

public enum TileTypeEnum
{
    普通,
    陷坑,
    灼烧,
    火山,
}