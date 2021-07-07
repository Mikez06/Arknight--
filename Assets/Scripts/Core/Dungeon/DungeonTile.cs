using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DungeonTile
    {
        public virtual TileTypeEnum Type => TileTypeEnum.Other;
        public int X, Y;
        public TileStatusEnum tileStatus;
        public HashSet<DungeonTile> Pres = new HashSet<DungeonTile>(), Nexts = new HashSet<DungeonTile>();

        public virtual DungeonTile Init()
        {
            return this;
        }

        public void Enter()
        {
            tileStatus = TileStatusEnum.OnTile;
        }

        public enum TileStatusEnum
        {
            CanEnter,
            NotEnter,
            OnTile,
            Complete,
        }

        public enum TileTypeEnum
        {
            Treasure,
            Battle_Sp,
            Battle,
            Rest,
            Shop,
            Event,
            Boss,

            Other = 100,
        }
    }
}