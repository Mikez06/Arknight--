using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Dungeon
{
    public DungeonCard StartCard;
    public DungeonTile[,] Tiles;
    public List<DungeonCard> Cards = new List<DungeonCard>();
    public List<DungeonRelic> Relics = new List<DungeonRelic>();
    public List<DungeonCard> AllCards = new List<DungeonCard>();

    public int MaxCardCount = 5;

    public DungeonTile NowTile;
    public int Sight = 2;
    public System.Random Seed;


    public void RebuildMap(int sizeX, int sizeY)
    {
        Seed = new System.Random();
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

    public DungeonCard AddUnit(int cardId)
    {
        var card = new DungeonCard()
        {
            CardId=cardId,
        };
        AllCards.Add(card);
        if (Cards.Count< MaxCardCount)
        {
            Cards.Add(card);
        }
        return card;
    }

    public bool CanMove(DungeonTile dungeonTile)
    {
        return (((dungeonTile.X == NowTile.X && dungeonTile.Y == NowTile.Y + 1) || (dungeonTile.X == NowTile.X + 1 && dungeonTile.Y == NowTile.Y)) && dungeonTile.TileType != DungeonTileTypeEnum.Tower);
    }

    public void MoveTo(DungeonTile dungeonTile)
    {
        if (((dungeonTile.X == NowTile.X && dungeonTile.Y == NowTile.Y + 1) || (dungeonTile.X == NowTile.X + 1 && dungeonTile.Y == NowTile.Y)) && dungeonTile.TileType != DungeonTileTypeEnum.Tower)
        {
            NowTile = dungeonTile;
            updateSight();
        }
    }

    public async Task TriggerNowTile()
    {
        if (NowTile.TileType == DungeonTileTypeEnum.Battle)
        {
            await BattleManager.Instance.StartBattle(new BattleInput()
            {
                Dungeon = this,
                MapName = NowTile.MapId,
            });
        }
    }

    void updateSight()
    {
        foreach (var tile in Tiles)
        {
            if (tile.TileType == DungeonTileTypeEnum.Tower)
            {
                tile.InSight = true;
                continue;
            }
            if (tile.Distance(NowTile) <= Sight)
            {
                tile.InSight = true;
                if (tile.TileType == DungeonTileTypeEnum.Battle && string.IsNullOrEmpty(tile.MapId))
                {
                    tile.MapId = getTileBattleMap();
                    usingMap.Add(tile.MapId);
                    facedMap.Remove(tile.MapId);
                }
            }
            else
            {
                tile.InSight = false;
                if (tile.TileType == DungeonTileTypeEnum.Battle && !string.IsNullOrEmpty(tile.MapId))
                {
                    usingMap.Remove(tile.MapId);
                    facedMap.Add(tile.MapId);
                }
            }
        }
    }

    void buildType()
    {
        Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 1].TileType = DungeonTileTypeEnum.Battle;
        Tiles[Tiles.GetLength(0) - 2, Tiles.GetLength(1) - 1].TileType = DungeonTileTypeEnum.Heal;
        Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 2].TileType = DungeonTileTypeEnum.Heal;
        Tiles[0, 0].TileType = DungeonTileTypeEnum.Event;
        Tiles[0, 1].TileType = DungeonTileTypeEnum.Battle;
        Tiles[1, 0].TileType = DungeonTileTypeEnum.Battle;
        buildTower(2);
        buildHeal();
        foreach (var tile in Tiles)
        {
            if (tile.TileType == DungeonTileTypeEnum.None)
            {
                if (Seed.Next(0, 3) == 0) tile.TileType = DungeonTileTypeEnum.Event;
                else tile.TileType = DungeonTileTypeEnum.Battle;
            }
        }
    }

    void buildHeal()
    {
        List<int> used = new List<int>();
        int healCount = 1;

        for (int n = 0; n < healCount; n++)
        {
            List<DungeonTile> l = new List<DungeonTile>();
            //希望至少能取到4个点,并且取到的点不能和之前的在同一行
            int line = Seed.Next(4, Tiles.GetLength(0) + Tiles.GetLength(1) - 4 - used.Count);
            foreach (var a in used) if (line > a) line++;
            used.Add(line);

            for (int i = 0; i < Tiles.GetLength(1); i++)
            {
                var t = Tiles.Get(line - i, i);
                if (t != null && t.TileType == DungeonTileTypeEnum.None) l.Add(t);
            }
            //七成多一点的格子为目标格，其余均算为随机格
            int lineShopCount = randomToInt(l.Count * 0.7f);
            for (int i = 0; i < lineShopCount; i++)
            {
                var t = l[Seed.Next(0, l.Count)];
                t.TileType = DungeonTileTypeEnum.Heal;
                l.Remove(t);
            }
        }

        int rewardCount = 1;
        for (int n = 0; n < rewardCount; n++)
        {
            List<DungeonTile> l = new List<DungeonTile>();
            //希望至少能取到4个点,并且取到的点不能和之前的在同一行
            int line = Seed.Next(4, Tiles.GetLength(0) + Tiles.GetLength(1) - 4 - used.Count);
            foreach (var a in used) if (line > a) line++;
            used.Add(line);

            for (int i = 0; i < Tiles.GetLength(1); i++)
            {
                var t = Tiles.Get(line - i, i);
                if (t != null && t.TileType == DungeonTileTypeEnum.None) l.Add(t);
            }
            //七成多一点的格子为目标格，其余均算为随机格
            int lineShopCount = randomToInt(l.Count * 0.7f);
            for (int i = 0; i < lineShopCount; i++)
            {
                var t = l[Seed.Next(0, l.Count)];
                t.TileType = DungeonTileTypeEnum.Reward;
                l.Remove(t);
            }
        }
        int shopCount = 1;
        for (int n = 0; n < shopCount; n++)
        {
            List<DungeonTile> l = new List<DungeonTile>();
            //希望至少能取到4个点,并且取到的点不能和之前的在同一行
            int line = Seed.Next(4, Tiles.GetLength(0) + Tiles.GetLength(1) - 4 - used.Count);
            foreach (var a in used) if (line > a) line++;
            used.Add(line);

            for (int i = 0; i < Tiles.GetLength(1); i++)
            {
                var t = Tiles.Get(line - i, i);
                if (t != null && t.TileType == DungeonTileTypeEnum.None) l.Add(t);
            }
            //七成多一点的格子为目标格，其余均算为随机格
            int lineShopCount = randomToInt(l.Count * 0.7f);
            for (int i = 0; i < lineShopCount; i++)
            {
                var t = l[Seed.Next(0, l.Count)];
                t.TileType = DungeonTileTypeEnum.Shop;
                l.Remove(t);
            }
        }
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
                if (tile.TileType != DungeonTileTypeEnum.None) continue;
                if (bo[tile.X, tile.Y]) continue;
                l.Add(tile);
            }
            if (l.Count == 0) break;
            var buildTile = l[Seed.Next(0, l.Count)];
            buildTile.TileType = DungeonTileTypeEnum.Tower;
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


    HashSet<string> usingMap = new HashSet<string>();//当前玩家视野里的地图
    HashSet<string> battleMap = new HashSet<string>();//玩家已经挑战了的地图
    List<string> facedMap = new List<string>();//玩家已经见过的地图
    string getTileBattleMap()
    {
        List<MapData> mapDatas = Database.Instance.GetAll<MapData>().ToList();
        List<string> maps = mapDatas.Select(x => x.Id).ToList();
        maps.RemoveAll(x => usingMap.Contains(x) || battleMap.Contains(x));//尽量不使用视野中和已经挑战过的图
        List<string> unfaced = maps.Where(x => !facedMap.Contains(x)).ToList();
        //见过的地图和没见过的地图按照1:3权重加成进行随机
        int all = unfaced.Count * 3 + facedMap.Count;
        if (all == 0)//地图不够用了
        {
            if (battleMap.Count > 0)
            {
                //把已挑战地图全部踢到见过的地图里
                facedMap.AddRange(battleMap);
                battleMap.Clear();
                all = facedMap.Count;
            }
            else
            {
                //全部地图都在视野里了？建议去揍策划
                return usingMap.ToList()[Seed.Next(0, usingMap.Count)];
            }
        }
        int s = Seed.Next(0, all);
        if (facedMap.Count > s)
        {
            return facedMap[s];
        }
        else
        {
            return unfaced[(s - facedMap.Count) / 3];
        }
    }

    int randomToInt(float f)
    {
        int i = Mathf.FloorToInt(f);
        i = i + (Seed.NextDouble() < (f - i) ? 1 : 0);
        return i;
    }
}