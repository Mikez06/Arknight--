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

    public Unit Unit;
    public Unit MidUnit;
    /// <summary>
    /// 广搜临时数据
    /// </summary>
    public Tile PreGrid;

    public string Tag;
    public float ActiveTime;

    public virtual void Update()
    {

    }

    public virtual void Init(Map map, MapGrid mapGrid)
    {
        this.MapGrid = mapGrid;
        mapGrid.Tile = this;
        this.Map = map;
        this.MapGrid = mapGrid;
        this.Pos = mapGrid.GetPos();
        this.X = mapGrid.X;
        this.Y = mapGrid.Y;
        this.CanBuildUnit = mapGrid.CanBuildUnit;
        this.FarAttackGrid = mapGrid.FarAttackGrid;
        this.Tag = mapGrid.Tag;
    }

    public bool CanSet(Unit unit)
    {
        if (this.Unit != null) return false;
        if (CanBuildUnit)
        {
            if (Battle.MapData.NoBuildLimit) return true;
            if (FarAttackGrid)
            {
                return unit.UnitData.CanSetHigh;
            }
            else
                return unit.UnitData.CanSetGround;
        }
        return false;
    }

    public void ChangeToDefault()
    {
        Pos = new Vector3(X, 0, Y);
    }
}