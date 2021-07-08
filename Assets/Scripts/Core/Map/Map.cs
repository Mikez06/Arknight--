using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
public class Map
{
    //public List<MapGrid> StartPoints = new List<MapGrid>();//起始点有什么卵用吗
    //public List<MapGrid> EndPoints = new List<MapGrid>();//终点好像也没什么卵用
    public MapGrid[,] Grids;

    public MapData Config => Database.Instance.Get<MapData>(Id);

    public int Id;

    public void Init()
    {
        var grids = MapManager.Instance.GetComponentsInChildren<MapGrid>();
        Grids = new MapGrid[grids.Max(x => x.X) + 1, grids.Max(x => x.Y) + 1];
        foreach (var grid in grids)
        {
            Grids[grid.X, grid.Y] = grid;
        }

    }

    StartEndModifier startEndModifier = new StartEndModifier()
    {
        exactStartPoint = StartEndModifier.Exactness.ClosestOnNode,
        exactEndPoint = StartEndModifier.Exactness.ClosestOnNode,
    };
    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        var p = ABPath.Construct(start, end);
        AstarPath.StartPath(p);
        p.BlockUntilCalculated();
        startEndModifier.Apply(p);
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
    public List<MapGrid> FindPath(MapGrid start, MapGrid end)
    {
        Queue<MapGrid> searchGrids = new Queue<MapGrid>();
        HashSet<MapGrid> updateGrids = new HashSet<MapGrid>();
        searchGrids.Enqueue(start);
        updateGrids.Add(start);
        void tryAdd(MapGrid pre, int x,int y)
        {
            if (x >= 0 && x < Grids.GetLength(0) && y >= 0 && y < Grids.GetLength(1))
            {
                if (Grids[x, y].CanMove && !updateGrids.Contains(Grids[x,y]))
                {
                    Grids[x, y].PreGrid = pre;
                    searchGrids.Enqueue(Grids[x, y]);
                    updateGrids.Add(Grids[x, y]);
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
                List<MapGrid> result = new List<MapGrid>();
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
