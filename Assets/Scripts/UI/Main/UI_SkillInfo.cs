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
            SkillData skillConfig = Database.Instance.Get<SkillData>(skillId);
            m_desc.text = skillConfig.Desc;
            m_r.m_recover.selectedIndex = (int)skillConfig.PowerType;
            m_UseType.m_useType.selectedIndex = (int)skillConfig.UseType;
            m_r.visible = skillConfig.UseType != SkillUseTypeEnum.被动;
            m_skillIcon.SetSkill(skillId);
            m_lastTime.text = skillConfig.OpenTime > 1000 ? "∞" : skillConfig.OpenTime.ToString();
            m_time.selectedIndex = skillConfig.OpenTime >= 1f ? 0 : 1;
            var targetHeight = m_desc.height + 65f;
            if (targetHeight < 184f) targetHeight = 184f;
            height = targetHeight;
        }
    }
}
