using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleUI
{
    partial class UI_BattleLeft
    {
        Texture alt;
        Unit Unit;
        partial void Init()
        {
            FairyGUI.UIConfig.allowSoftnessOnTopOrLeftSide = false;
            alt = ResHelper.GetAsset<Texture>(PathHelper.OtherPath + "a遮罩");
            //m_standPic.m_standPic.tex
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Unit != null)
            {
                m_name.text = Unit.UnitData.Name;
                m_atk.text = Unit.Attack.ToString();
                m_def.text = Unit.Defence.ToString();
                m_agi.text = Unit.Agi.ToString();
                m_magDef.text = Unit.MagicDefence.ToString();
                m_block.text = Unit.UnitData.StopCount.ToString();
                m_Hp.max = Unit.MaxHp;
                m_Hp.value = Unit.Hp;
                m_standPic.m_standPic.texture = new FairyGUI.NTexture(ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + Unit.UnitData.StandPic), alt, 1, 1);
                if (Unit is Units.干员)
                {
                    m_palyerUnit.selectedIndex = 0;
                    (m_Pro as MainUI.UI_Pro).m_p.selectedIndex = (int)Unit.UnitData.Profession;
                    var mainSkill = Unit.MainSkill;
                    if (mainSkill != null)
                    {
                        m_SkillName.text = mainSkill.SkillData.Name;

                        m_skillIcon.icon = mainSkill.SkillData.Icon.ToSkillIcon();
                        (m_Recover as MainUI.UI_Recover).m_recover.selectedIndex = (int)mainSkill.SkillData.PowerType;
                        (m_UseType as MainUI.UI_UseType).m_useType.selectedIndex = (int)mainSkill.SkillData.UseType;
                        m_lastTime.text = mainSkill.SkillData.OpenTime > 1000 ? "∞" : mainSkill.SkillData.OpenTime.ToString();
                        m_time.selectedIndex = mainSkill.SkillData.OpenTime >= 1f ? 0 : 1;
                        m_SkillDesc.text = mainSkill.SkillData.Desc;
                    }
                }
                else
                {
                    m_palyerUnit.selectedIndex = 1;
                }
            }
        }

        public void SetUnit(Unit unit)
        {
            this.Unit = unit;
        }
    }
}
