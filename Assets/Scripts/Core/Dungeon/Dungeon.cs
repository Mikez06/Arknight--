using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Dungeon
    {
        public HashSet<DungeonTile> Tiles = new HashSet<DungeonTile>();
        public DungeonTile NowTile;

        public void CreateDungeon(int height, int width,int path)
        {
            Tiles.Clear();
            for (int i = 0; i < path; i++)
            {
                DungeonTile LastTile = null;
                for (int j = 0; j < width; j++)
                {
                    DungeonTile next;
                    if (j == 0)
                    {
                        //第零层固定位置生成起点
                        int y = height / 2;
                        next = Get(j, y);
                        if (next == null)
                            next = new DungeonTile() { X = j, Y = y };
                        NowTile = next;
                    }
                    else if (j == width - 1)
                    {
                        //最后一层固定生成boss点
                        int y = height / 2;
                        next = Get(j, y);
                        if (next == null)
                            next = new DungeonTile() { X = j, Y = y };
                    }
                    else
                    {
                        List<int> waychoose = new List<int>() { -1, 0, 1 };
                        if (LastTile.Y == 0) waychoose.Remove(-1);
                        else if (LastTile.Y == height - 1) waychoose.Remove(1);
                        DungeonTile midNext = Get(j, LastTile.Y);
                        if (midNext != null)
                        {
                            foreach (var t in midNext.Pres)
                            {
                                if (t.Y == midNext.Y + 1)
                                    waychoose.Remove(1);
                                else if (t.Y == midNext.Y - 1)
                                    waychoose.Remove(-1);
                            }
                        }
                        int NextY = LastTile.Y + waychoose[UnityEngine.Random.Range(0, waychoose.Count)];
                        next = new DungeonTile() { X = j, Y = NextY };
                    }
                    if (j != 0) LastTile.Nexts.Add(next);
                    next.Pres.Add(LastTile);
                    Tiles.Add(next);
                    LastTile = next;
                }
            }
        }
        public DungeonTile Get(int x, int y)
        {
            DungeonTile tile = Tiles.FirstOrDefault((t) =>
            {
                return t.X == x && t.Y == y;
            });
            return tile;
        }
    }
}
