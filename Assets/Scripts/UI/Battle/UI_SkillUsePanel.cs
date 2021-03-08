using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleUI
{
    partial class UI_SkillUsePanel
    {
        public Units.干员 Unit;
        public void SetUnit(Units.干员 unit)
        {
            this.Unit = unit;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Unit == null) return;
            var s = Unit.MainSkill as Skills.主动;
            m_mainSkillInfo.visible = true;
            m_mainSkillInfo.m_using.selectedIndex = s.Opening.Finished() ? 0 : 1;
            if (!s.Opening.Finished())
            {
                m_mainSkillInfo.max = s.Config.OpenTime;
                m_mainSkillInfo.value = s.Opening.value;
            }
            else
            {
                m_mainSkillInfo.max = Unit.MaxPower;
                m_mainSkillInfo.value = Unit.Power - Unit.MaxPower * Mathf.FloorToInt(Unit.Power / Unit.MaxPower);
            }
            if (Unit.Power == Unit.MaxPower * Unit.PowerCount)
            {
                m_mainSkillInfo.value = m_mainSkillInfo.max;
            }
            m_mainSkillInfo.m_text.text = $"{(int)m_mainSkillInfo.value}/{ (int)m_mainSkillInfo.max}";
        }
    }
}
