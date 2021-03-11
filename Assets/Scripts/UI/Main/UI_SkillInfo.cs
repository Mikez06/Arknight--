using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_SkillInfo
    {
        public void SetSkill(int skillId)
        {
            SkillConfig skillConfig = Database.Instance.Get<SkillConfig>(skillId);
            m_name.text = skillConfig.Name;
            m_desc.text = skillConfig.Desc;
            m_icon.icon = "ui://SkillIcon/" + skillConfig.Icon;
        }
    }
}
