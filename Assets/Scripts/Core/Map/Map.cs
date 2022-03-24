using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
public class Map
{
    public Battle Battle;
    //public List<MapGrid> StartPoints = new List<MapGrid>();//起始点有什么卵用吗
    //public List<MapGrid> EndPoints = new List<MapGrid>();//终点好像也没什么卵用
    public Tile[,] Tiles;

    public MapData Config => Database.Instance.Get<MapData>(Id);

    public int Id;

    public float minX = float.MaxValue, minZ = float.MaxValue, maxX = float.MinValue, maxZ = float.MinValue;

    public void Init(Battle battle)
    {
        this.Battle = battle;
        var grids = MapManager.Instance.GetComponentsInChildren<MapGrid>();
        Tiles = new Tile[grids.Max(x => x.X) + 1, grids.Max(x => x.Y) + 1];
        foreach (var grid in grids)
        {
            Tiles[grid.X, grid.Y] = CreateTile(grid);
        }
        foreach (var grid in grids)
        {
            if (!string.IsNullOrEmpty(grid.MapUnitId))
            {
                battle.SceneUnits.Add(new MapUnitInfo()
                {
                    Time = grid.ActiveTime,
                    Id = grid.MapUnitId,
                    Tag = grid.Tag,
                    Pos = grid.transform.position,
                    Direction = grid.transform.forward.ToV2(),
                });
            }
        }
        battle.SortSceneUnit();
    }

    Tile CreateTile(MapGrid mapGrid)
    {
        Tile tile = new Tile();
        tile.Init(this, mapGrid);
        if (tile.Pos.x > maxX) maxX = tile.Pos.x;
        if (tile.Pos.x < minX) minX = tile.Pos.x;
        if (tile.Pos.z > maxZ) maxZ = tile.Pos.z;
        if (tile.Pos.z < minZ) minZ = tile.Pos.z;
        return tile;
    }

    StartEndModifier startEndModifier = new StartEndModifier()
    {
        exactStartPoint = StartEndModifier.Exactness.ClosestOnNode,
        exactEndPoint = StartEndModifier.Exactness.ClosestOnNode,
    };
    RaycastModifier raycastModifier = new RaycastModifier()
    {
        useGraphRaycasting = true,
        useRaycasting = false,
    };
    public List<Vector3> FindPath(Vector3 start, Vector3 end,bool raycastModify)
    {
        var p = ABPath.Construct(start, end);
        AstarPath.StartPath(p);
        p.BlockUntilCalculated();

        startEndModifier.Apply(p);

        if (raycastModifier) raycastModifier.Apply(p);

        var result = new List<Vector3>(p.vectorPath);
        return result;
    }

    /// <summary>
    /// 已弃用，请使用A*插件版
    /// 找到一条从起点到终点的最短路径
    /// TODO 箱子 阻碍物
    /// 目前使用广搜 或许改成A*更佳
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public List<Tile> FindPath(Tile start, Tile end)
    {
        Queue<Tile> searchGrids = new Queue<Tile>();
        HashSet<Tile> updateGrids = new HashSet<Tile>();
        searchGrids.Enqueue(start);
        updateGrids.Add(start);
        void tryAdd(Tile pre, int x,int y)
        {
            if (x >= 0 && x < Tiles.GetLength(0) && y >= 0 && y < Tiles.GetLength(1))
            {
                if (Tiles[x, y].CanMove && !updateGrids.Contains(Tiles[x,y]))
                {
                    Tiles[x, y].PreGrid = pre;
                    searchGrids.Enqueue(Tiles[x, y]);
                    updateGrids.Add(Tiles[x, y]);
                }
            }
        }
        while (searchGrids.Count > 0)
        {
            var nextGrid = searchGrids.Dequeue();
            tryAdd(nextGrid, nextGrid.X + 1, nextGrid.Y);
            tryAdd(nextGrid, nextGrid.X - 1, nextGrid.Y);
            tryAdd(nextGrid, nextGrid.X, nextGrid.Y + 1);
            tryAdd(nextGrid, nextGrid.X, nextGrid.Y - 1);
            if (updateGrids.Contains(end))
            {
                List<Tile> result = new List<Tile>();
                var g = end;
                result.Add(g);
                while (g.PreGrid != null)
                {
                    result.Add(g.PreGrid);
                    g = g.PreGrid;
                }
                result.Reverse();
                foreach (var grid in updateGrids) grid.PreGrid = null;
                return result;
            }
        }
        return null;
    }
}
