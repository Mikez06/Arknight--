using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUI
{
    partial class UI_UnitSkillIcon
    {
        public void SetSkill(int id)
        {
            SkillData skillData = Database.Instance.Get<SkillData>(id);
            m_level.selectedIndex = skillData.Upgrade;
            m_icon.icon = skillData.Icon.ToSkillIcon();
            if (skillData.StartPower == 0)
            {
                m_start.selectedIndex = 0;
            }
            else
            {
                m_start.selectedIndex = 1;
                m_startTime.text = skillData.StartPower.ToString();
            }
            m_cost.text = skillData.MaxPower.ToString();
            m_skillName.text = skillData.Name;
        }
    }
}
