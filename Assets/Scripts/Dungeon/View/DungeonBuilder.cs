using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DungeonBuilder : Singleton<DungeonBuilder>
{

    Pool<DungeonTileController> Pool = new Pool<DungeonTileController>();
    public void ReBuild()
    {
        var map = GameObject.Find("DungeonMap");
        var dungeon = DungeonManager.Instance.Dungeon;
        foreach (Transform child in map.transform)
        {
            if (child == map.transform) continue;
            Pool.Despawn(map.GetComponent<DungeonTileController>());
        }

        foreach (var tile in dungeon.Tiles)
        {
            var grid = Pool.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.DungeonGridPath + tile.TileData.Model).GetComponent<DungeonTileController>(), tile.WorldPos, null);
            grid.transform.parent = map.transform;
            tile.TileController = grid;
            grid.DungeonTile = tile;
        }
    }
}