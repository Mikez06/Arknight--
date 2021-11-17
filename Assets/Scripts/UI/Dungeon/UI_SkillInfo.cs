using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace DungeonUI
{
    partial class UI_SkillInfo
    {
        public void SetSkill(int skillId)
        {
            SkillData skillConfig = Database.Instance.Get<SkillData>(skillId);
            m_name.text = skillConfig.Name;
            m_desc.text = skillConfig.Desc;
            m_icon.icon = skillConfig.Icon.ToSkillIcon();
        }
    }
}
