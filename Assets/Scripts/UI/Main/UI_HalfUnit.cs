using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUI
{
    partial class UI_HalfUnit
    {
        public Card Card;
        public void SetCard(Card card,int skillIndex)
        {
            this.Card = card;
            m_typeControl.selectedIndex = (int)card.UnitData.Profession;
            m_halfPic.icon = "ui://Res/" + card.UnitData.HalfIcon;
            m_skillIcon.icon = Database.Instance.Get<SkillData>(card.UnitData.MainSkill[skillIndex]).Icon.ToSkillIcon();
            m_stars.RemoveChildrenToPool();
            for (int i = 0; i < card.UnitData.Rare; i++)
            {
                m_stars.AddItemFromPool();
            }
            m_lv.text = card.Level.ToString();
            m_name.text = card.UnitData.Name.ToString();
        }
    }
}
