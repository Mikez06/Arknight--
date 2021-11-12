using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonTile
{
    public DungeonTileController TileController;

    public DungeonTileTypeEnum Type;

    public MapTileData TileData => Database.Instance.Get<MapTileData>((int)Type);

    public int X, Y;

    public bool InSight;

    public Vector3 WorldPos => new Vector3(X, 0, Y);

    public int Distance(DungeonTile target)
    {
        return Math.Abs(X - target.X) + Math.Abs(Y - target.Y);
    }
}
