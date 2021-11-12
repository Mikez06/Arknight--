using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dungeon
{
    public DungeonCard StartCard;
    public DungeonTile[,] Tiles;
    public List<DungeonCard> Cards = new List<DungeonCard>();

    public DungeonTile NowTile;
    public int Sight = 2;
    public Random Seed;

    public void RebuildMap(int sizeX, int sizeY)
    {
        Seed = new Random();
        Tiles = new DungeonTile[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Tiles[i, j] = new DungeonTile()
                {
                    X = i,
                    Y = j,
                };
            }
        }
        NowTile = Tiles[0, 0];
        buildType();
        updateSight();
    }

    public void MoveTo(DungeonTile dungeonTile)
    {
        NowTile = dungeonTile;
        updateSight();
    }

    void updateSight()
    {
        foreach (var tile in Tiles)
        {
            if (tile.Type == DungeonTileTypeEnum.Tower)
            {
                tile.InSight = true;
                continue;
            }
            if (tile.Distance(NowTile) <= Sight)
            {
                tile.InSight = true;
            }
            else
            {
                tile.InSight = false;
            }
        }
    }

    void buildType()
    {
        Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 1].Type = DungeonTileTypeEnum.Battle;
        Tiles[Tiles.GetLength(0) - 2, Tiles.GetLength(1) - 1].Type = DungeonTileTypeEnum.Heal;
        Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 2].Type = DungeonTileTypeEnum.Heal;
        Tiles[0, 0].Type = DungeonTileTypeEnum.Event;
        Tiles[0, 1].Type = DungeonTileTypeEnum.Battle;
        Tiles[1, 0].Type = DungeonTileTypeEnum.Battle;
        var count = Tiles.Length - 6;
        buildTower(2);
    }

    void buildTower(int count)
    {
        int towerMinDist = 4;
        bool[,] bo = new bool[Tiles.GetLength(0), Tiles.GetLength(1)];
        List<DungeonTile> l = new List<DungeonTile>();
        for (int k = 0; k < count; k++)
        {
            l.Clear();
            foreach (var tile in Tiles)
            {
                if (tile.Type != DungeonTileTypeEnum.None) continue;
                if (bo[tile.X, tile.Y]) continue;
                l.Add(tile);
            }
            if (l.Count == 0) break;
            var buildTile = l[Seed.Next(0, l.Count)];
            buildTile.Type = DungeonTileTypeEnum.Tower;
            for (int i=-towerMinDist; i <= towerMinDist; i++)
            {
                int jMax = towerMinDist - Math.Abs(i);
                for (int j = -jMax; j <= jMax; j++)
                {
                    if (buildTile.X + i >= 0 && buildTile.X + i < bo.GetLength(0) && buildTile.Y + j >= 0 && buildTile.Y + j < bo.GetLength(1))
                        bo[buildTile.X + i, buildTile.Y + j] = true;
                }
            }
        }
    }
}