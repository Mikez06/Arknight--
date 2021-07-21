using System.Collections;
using UnityEngine;

namespace BattleUI
{
    partial class UI_BattleUnit
    {
        public Unit Unit;
        partial void Init()
        {
            touchable = false;
        }
        public void SetUnit(Unit unit)
        {
            this.Unit = unit;
            unit.uiUnit = this;
            if (unit is Units.干员)
            {
                m_unitType.selectedIndex = 0;
            }
            else
            {
                m_unitType.selectedIndex = 1;
            }
            Flush();
        }

        protected override void OnUpdate()
        {
            if (Unit != null)
            {
                Flush();
            }
            base.OnUpdate();
        }

        public void Flush()
        {
            Vector2 unitPos = Camera.main.WorldToScreenPoint(Unit.UnitModel.SkeletonAnimation.transform.position); //Stage.inst.touchPosition.ScreenToUI();
            unitPos.y = Screen.height - unitPos.y;
            xy = unitPos.ScreenToUI();
            if (Unit is Units.干员 u)
            {
                m_hp.max = Unit.MaxHp;
                m_hp.value = Unit.Hp;
                if (Unit.MainSkill != null)
                {
                    if (Unit.MainSkill.Opening.Finished())
                    {
                        m_sk.value = Unit.MainSkill.Power - Unit.MainSkill.MaxPower * Mathf.FloorToInt(Unit.MainSkill.Power / Unit.MainSkill.MaxPower);
                        m_sk.max = Unit.MainSkill.MaxPower;
                    }
                    else
                    {
                        m_sk.value = Unit.MainSkill.Opening.value;
                        m_sk.max = Unit.MainSkill.Config.OpenTime;
                    }
                    if (Unit.MainSkill.Power == Unit.MainSkill.MaxPower * Unit.MainSkill.PowerCount)
                    {
                        m_sk.value = m_sk.max;
                    }

                    if (!Unit.MainSkill.Opening.Finished())
                    {
                        m_sk.m_useControl.selectedIndex = 1;
                    }
                    else
                        m_sk.m_useControl.selectedIndex = 0;

                    if (Unit.MainSkill.Power >= Unit.MainSkill.MaxPower)
                    {
                        if (Unit.MainSkill.Config.UseType == SkillUseTypeEnum.手动)
                        {
                            m_readyControl.selectedIndex = 1;
                        }
                        else
                        {
                            m_skillCount.selectedIndex = 1;
                            m_skillCount_2.text = Mathf.FloorToInt(Unit.MainSkill.Power / Unit.MainSkill.MaxPower).ToString();
                        }
                    }
                    else
                    {
                        m_readyControl.selectedIndex = 0;
                        m_skillCount.selectedIndex = 0;
                    }
                }
            }
            else
            {
                m_eHp.max = Unit.MaxHp;
                m_eHp.value = Unit.Hp;
                m_eHp.visible = m_eHp.value != m_eHp.max;
            }
        }
    }
}