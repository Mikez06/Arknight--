using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace DungeonUI
{
    partial class UI_TeamUnit
    {
        public DungeonCard DungeonCard;

        partial void Init()
        {
            m_Skills.onClickItem.Add(clickSkill);
        }

        public void SetCard(DungeonCard dungeonCard)
        {
            this.DungeonCard = dungeonCard;
            Fresh();
        }

        public void Fresh()
        {
            if (DungeonCard == null)
            {
                m_State.selectedIndex = 1;
            }
            else
            {
                m_State.selectedIndex = 0;
                m_Head.icon = DungeonCard.UnitData.HeadIcon.ToHeadIcon();
                m_Skills.RemoveChildrenToPool();
                for (int i = 0; i < DungeonCard.UnitData.MainSkill.Length; i++)
                {
                    var skilldata = Database.Instance.Get<SkillData>(DungeonCard.UnitData.MainSkill[i]);
                    var teamSkill = m_Skills.AddItemFromPool() as UI_TeamSkill;
                    teamSkill.m_icon.icon = skilldata.Icon.ToSkillIcon();
                    teamSkill.m_select.selectedIndex = DungeonCard.UsingSkill == i ? 0 : 1;
                }
                m_Name.text = DungeonCard.UnitData.Name;
                m_exp.SetVar("n", DungeonCard.Exp.ToString()).SetVar("m", DungeonCard.GetUpgradeExp().ToString()).FlushVars();
                m_level.SetVar("l", DungeonCard.Level.ToString()).FlushVars();
            }
        } 

        void clickSkill(EventContext evt)
        {
            int index = m_Skills.GetChildIndex(evt.data as GObject);
            DungeonCard.UsingSkill = index;
            Fresh();
        }
    }
}
