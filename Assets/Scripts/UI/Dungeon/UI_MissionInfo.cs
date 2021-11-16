using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonUI
{
    partial class UI_MissionInfo
    {
        public void SetInfo(DungeonTile dungeonTile)
        {
            switch (dungeonTile.TileType)
            {
                case DungeonTileTypeEnum.Battle:
                    break;
            }
        }
    }
}
