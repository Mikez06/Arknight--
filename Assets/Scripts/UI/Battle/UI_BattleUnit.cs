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
            m_unitType.selectedIndex = unit.UnitData.HpBarType;
            m_readyControl.selectedIndex = 0;
            m_skillCount.selectedIndex = 0;
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
            if (Unit is Units.敌人 u)
            {
                if (!u.Visiable)
                {
                    m_unitType.selectedIndex = 3;
                    return;
                }
                else
                    m_unitType.selectedIndex = Unit.UnitData.HpBarType;
            }
            if (!Unit.ElementProtect.Finished())
            {
                m_elementBar.m_Recover.selectedIndex = 2;
                m_elementBar.max = 10;
                m_elementBar.value = 10 - Unit.ElementProtect.value;
            }
            else
            {
                float elementValue = Unit.NervePoint;
                m_elementBar.max = 1000;
                m_elementBar.value = 1000 - elementValue;
                m_elementBar.m_Recover.selectedIndex = elementValue == 0 ? 0 : 1;
            }

            xy = Unit.UnitModel.GetModelPositon().WorldToUI();
            if (m_unitType.selectedIndex == 0 || m_unitType.selectedIndex == 2)
            {
                if (Unit.LifeTime != null)
                {
                    m_hp.max = Unit.UnitData.LifeTime;
                    m_hp.value = Unit.LifeTime.value;
                }
                else
                {
                    m_hp.max = Unit.MaxHp;
                    m_hp.value = Unit.Hp;
                }
                if (Unit.MainSkill != null)
                {
                    if (Unit.MainSkill.MaxPower > 0)
                    {
                        if (Unit.MainSkill.Opening.Finished())
                        {
                            m_sk.value = Unit.MainSkill.Power - Unit.MainSkill.MaxPower * Mathf.FloorToInt(Unit.MainSkill.Power / Unit.MainSkill.MaxPower);
                            m_sk.max = Unit.MainSkill.MaxPower;
                        }
                        else
                        {
                            m_sk.value = Unit.MainSkill.Opening.value;
                            m_sk.max = Unit.MainSkill.SkillData.OpenTime;
                        }
                        if (Unit.MainSkill.Power == Unit.MainSkill.MaxPower * Unit.MainSkill.PowerCount && Unit.MainSkill.Power != 0)
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
                            if (Unit.MainSkill.PowerCount == 1)
                            {
                                m_readyControl.selectedIndex = 1;
                                m_skillCount.selectedIndex = 0;
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
                    else
                    {
                        m_sk.value = 0;
                        m_readyControl.selectedIndex = 0;
                        m_skillCount.selectedIndex = 0;
                    }
                }
            }
            else
            {
                if (Unit.LifeTime != null)
                {
                    m_eHp.max = Unit.UnitData.LifeTime;
                    m_eHp.value = Unit.LifeTime.value;
                }
                else
                {
                    m_eHp.max = Unit.MaxHp;
                    m_eHp.value = Unit.Hp;
                }
                m_eHp.visible = Unit.LifeTime == null ? m_eHp.value != m_eHp.max : true;
            }
        }
    }
}