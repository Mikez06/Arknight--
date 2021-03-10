using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUI
{
    partial class UI_HalfUnit
    {
        public void SetCard(Card card)
        {
            m_typeControl.selectedIndex = (int)card.Config.Profession;
            m_halfPic.icon = "ui://UnitPic/" + card.Config.StandPic;
            m_stars.RemoveChildrenToPool();
            for (int i = 0; i < card.Config.Rare; i++)
            {
                m_stars.AddItemFromPool();
            }
            m_lv.text = card.Level.ToString();
            m_name.text = card.Config.Name.ToString();
        }
    }
}
