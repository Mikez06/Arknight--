using FairyGUI;
using MainUI;
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
        GObjectPool pool;
        partial void Init()
        {
            FairyGUI.UIConfig.allowSoftnessOnTopOrLeftSide = false;
            alt = ResHelper.GetAsset<Texture>(PathHelper.OtherPath + "a遮罩");
            pool = new GObjectPool(container.cachedTransform);
            //m_standPic.m_standPic.tex
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Unit != null)
            {
                m_subSkill.text = Unit.UnitData.AblitityInfo;
                m_name.text = Unit.UnitData.Name;
                m_atk.text = Unit.Attack.ToString();
                m_def.text = Unit.Defence.ToString();
                m_agi.text = Unit.Agi.ToString();
                m_magDef.text = Unit.MagicDefence.ToString();
                m_block.text = Unit.UnitData.StopCount.ToString();
                m_Hp.max = Unit.MaxHp;
                m_Hp.value = Unit.Hp;
                var t = ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + Unit.UnitData.StandPic);
                if (t != null)
                    m_standPic.m_standPic.texture = new FairyGUI.NTexture(t, alt, 1, 1);
                else
                    m_standPic.m_standPic.icon = "";
                if (Unit is Units.干员)
                {
                    m_Lv.text = Unit.UnitData.Level.ToString();
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
                    else
                    {
                        m_palyerUnit.selectedIndex = 1;
                        m_midUnitDesc.text = "";
                    }
                }
                else
                {
                    m_palyerUnit.selectedIndex = 1;
                    m_midUnitDesc.text = Unit.UnitData.AblitityInfo;
                }
            }
        }

        public void SetUnit(Unit unit)
        {
            this.Unit = unit;

            foreach (var item in m_attackArea.GetChildren())
            {
                pool.ReturnObject(item);
            }
            m_attackArea.RemoveChildren();

            if (unit != null && unit is Units.干员)
            {
                var mainSkill = Unit.FirstSkill;
                if (mainSkill.SkillData.AttackPoints == null) return;
                float midX = (mainSkill.SkillData.AttackPoints.Max(x => x.x) + mainSkill.AttackPoints.Min(x => x.x)) / 2f;
                float midY = (mainSkill.SkillData.AttackPoints.Max(x => x.y) + mainSkill.AttackPoints.Min(x => x.y)) / 2f;
                foreach (var point in mainSkill.SkillData.AttackPoints)
                {
                    var a = pool.GetObject(UI_AttackArea.URL) as UI_AttackArea;
                    m_attackArea.AddChild(a);
                    a.xy = new Vector2((point.x - midX) * 33 - 12f, (point.y - midY) * 33 - 12f);
                    a.m_type.selectedIndex = (point.x == 0 && point.y == 0) ? 1 : 0;
                }
            }
        }
    }
}
