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
        public Unit Unit;
        public void SetUnit(Unit unit)
        {
            this.Unit = unit;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Unit == null) return;
            var s = Unit.MainSkill;
            if (s != null)
            {
                m_mainSkillInfo.visible = true;
                m_mainSkillInfo.m_using.selectedIndex = s.Opening.Finished() ? 0 : 1;
                if (!s.Opening.Finished())
                {
                    m_mainSkillInfo.max = s.SkillData.OpenTime;
                    m_mainSkillInfo.value = s.Opening.value;
                }
                else
                {
                    m_mainSkillInfo.max = Unit.MainSkill.MaxPower;
                    m_mainSkillInfo.value = Unit.MainSkill.Power - Unit.MainSkill.MaxPower * Mathf.FloorToInt(Unit.MainSkill.Power / Unit.MainSkill.MaxPower);
                }
                if (Unit.MainSkill.Power == Unit.MainSkill.MaxPower * Unit.MainSkill.PowerCount)
                {
                    m_mainSkillInfo.value = m_mainSkillInfo.max;
                }
                m_mainSkillInfo.m_text.text = $"{(int)m_mainSkillInfo.value}/{ (int)m_mainSkillInfo.max}";
            }
            else
            {
                m_mainSkillInfo.visible = false;
            }
        }
    }
}
