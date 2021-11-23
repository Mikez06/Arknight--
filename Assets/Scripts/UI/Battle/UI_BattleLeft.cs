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
        partial void Init()
        {
            FairyGUI.UIConfig.allowSoftnessOnTopOrLeftSide = false;
            alt = ResHelper.GetAsset<Texture>(PathHelper.OtherPath + "a遮罩");
            //m_standPic.m_standPic.tex
        }

        public void SetUnit(Units.干员 unit)
        {
            m_name.text = unit.UnitData.Name;
            m_atk.text = unit.Attack.ToString();
            m_def.text = unit.Defence.ToString();
            m_magDef.text = unit.MagicDefence.ToString();
            m_block.text = unit.StopCount.ToString();
            m_Hp.max = unit.MaxHp;
            m_Hp.value = unit.Hp;
            (m_Pro as MainUI.UI_Pro).m_p.selectedIndex = (int)unit.UnitData.Profession;
            m_standPic.m_standPic.texture = new FairyGUI.NTexture(ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + unit.UnitData.StandPic), alt, 1, 1);
            var mainSkill = unit.MainSkill;
            m_SkillName.text = mainSkill.SkillData.Name;

            m_skillIcon.icon = mainSkill.SkillData.Icon.ToSkillIcon();
            (m_Recover as MainUI.UI_Recover).m_recover.selectedIndex = (int)mainSkill.SkillData.PowerType;
            (m_UseType as MainUI.UI_UseType).m_useType.selectedIndex = (int)mainSkill.SkillData.UseType;
            m_lastTime.text = mainSkill.SkillData.OpenTime > 1000 ? "∞" : mainSkill.SkillData.OpenTime.ToString();
            m_time.selectedIndex = mainSkill.SkillData.OpenTime >= 1f ? 0 : 1;
            m_SkillDesc.text = mainSkill.SkillData.Desc;
        }
    }
}
