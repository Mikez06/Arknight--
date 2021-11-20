using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonUI
{
    partial class UI_RecruitUnit
    {
        public UnitData UnitData=>Database.Instance.Get<UnitData>(Id);
        public int Id;
        public void SetData(int Id)
        {
            this.Id = Id;
            Refresh();
        }

        public void Refresh()
        {
            m_Name.text = UnitData.Name;
            m_Head.icon = UnitData.HeadIcon.ToHeadIcon();
            m_level.text = UnitData.Level.ToString();
            m_upgrade.selectedIndex = UnitData.Upgrade;
            m_Cost.text = HopeHelper.GetCost(DungeonManager.Instance.Dungeon, Id, out bool ifUpgrade).ToString();
            m_ifUpgrade.selectedIndex = ifUpgrade ? 1 : 0;
        }
    }
}
