using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonUI
{
    partial class UI_Reward
    {
        public DungeonReward RewardData;
        public void SetReward(DungeonReward reward)
        {
            this.RewardData = reward;
            m_type.selectedIndex = reward.Type;
            if (reward.Type==2|| reward.Type==3)
            {
                m_count.SetVar("l", reward.Data.ToString()).FlushVars();
            }
            else
            {
                RelicData relicData = Database.Instance.Get<RelicData>(reward.Data);
                m_Name.text = relicData.Name;
                m_icon.icon = relicData.Icon.ToRelicIcon();
            }
        }
    }
}
