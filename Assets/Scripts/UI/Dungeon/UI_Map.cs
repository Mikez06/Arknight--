using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonUI
{
    partial class UI_Map
    {
        public DungeonTile NowSelect;

        partial void Init()
        {
            m_unselect.onClick.Add(() =>
            {
                if (m_showInfo.selectedIndex == 1)
                {
                    NowSelect = null;
                    m_showInfo.selectedIndex = 0;
                }
            });
            m_MissionInfo.m_go.onClick.Add(async () =>
            {
                m_showInfo.selectedIndex = 2;
                await DungeonManager.Instance.Move(NowSelect);
                m_showInfo.selectedIndex = 0;
            });
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            m_hope.SetVar("c", DungeonManager.Instance.Dungeon.Hope.ToString()).FlushVars();
            m_gold.SetVar("c", DungeonManager.Instance.Dungeon.Gold.ToString()).FlushVars();
        }

        public void SelectTile(DungeonTile dungeonTile)
        {
            NowSelect = dungeonTile;
            m_showInfo.selectedIndex = 1;
            m_MissionInfo.SetInfo(dungeonTile);
        }
    }
}
