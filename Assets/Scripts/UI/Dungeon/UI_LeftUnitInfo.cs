using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using MainUI;
using UnityEngine;

namespace DungeonUI
{
    partial class UI_LeftUnitInfo
    {
        GObjectPool pool;
        public DungeonCard Card;

        partial void Init()
        {
            pool = new GObjectPool(container.cachedTransform);
        }
        public void SetCard(DungeonCard card,int skillIndex)
        {
            this.Card = card;
            if (card == null)
            {
                m_empty.selectedIndex = 1;
            }
            else
            {
                m_empty.selectedIndex = 0;
                m_name.text = card.UnitData.Name;
                m_lv.text = card.Level.ToString();
                m_reset.text = card.UnitData.ResetTime.ToString();
                m_cost.text = card.UnitData.Cost.ToString();
                m_stop.text = card.UnitData.StopCount.ToString();
                var mainSkill = Database.Instance.Get<SkillData>(card.UnitData.Skills[0]);
                m_agi.text = mainSkill.Cooldown.ToString();
                m_hp.text = card.UnitData.Hp.ToString();
                m_def.text = card.UnitData.Defence.ToString();
                m_magdefence.text = card.UnitData.MagicDefence.ToString();

                foreach (var item in m_attackArea.GetChildren())
                {
                    pool.ReturnObject(item);
                }
                m_attackArea.RemoveChildren();

                foreach (var point in mainSkill.AttackPoints)
                {
                    var a = pool.GetObject(UI_AttackArea.URL) as UI_AttackArea;
                    m_attackArea.AddChild(a);
                    a.xy = new Vector2(point.x * 33 + 45, point.y * 33 + 67);
                    a.m_type.selectedIndex = (point.x == 0 && point.y == 0) ? 1 : 0;
                }

                m_Skills.RemoveChildrenToPool();

                for (int i = 0; i < card.UnitData.MainSkill.Length; i++)
                {
                    int skill = card.UnitData.MainSkill[i];
                    var uiSkill = m_Skills.AddItemFromPool() as UI_SkillInfo;
                    uiSkill.SetSkill(skill);
                    uiSkill.m_seleted.selectedIndex = i == skillIndex ? 1 : 0;
                }
            }
        }
        public void SetData(UnitData unitData)
        {
            if (unitData == null)
            {
                m_empty.selectedIndex = 1;
            }
            else
            {
                m_empty.selectedIndex = 0;
                m_name.text = unitData.Name;
                m_lv.text = unitData.Level.ToString();
                m_reset.text = unitData.ResetTime.ToString();
                m_cost.text = unitData.Cost.ToString();
                m_stop.text = unitData.StopCount.ToString();
                var mainSkill = Database.Instance.Get<SkillData>(unitData.Skills[0]);
                m_agi.text = mainSkill.Cooldown.ToString();
                m_hp.text = unitData.Hp.ToString();
                m_def.text = unitData.Defence.ToString();
                m_magdefence.text = unitData.MagicDefence.ToString();

                foreach (var item in m_attackArea.GetChildren())
                {
                    pool.ReturnObject(item);
                }
                m_attackArea.RemoveChildren();

                foreach (var point in mainSkill.AttackPoints)
                {
                    var a = pool.GetObject(UI_AttackArea.URL) as UI_AttackArea;
                    m_attackArea.AddChild(a);
                    a.xy = new Vector2(point.x * 33 + 45, point.y * 33 + 67);
                    a.m_type.selectedIndex = (point.x == 0 && point.y == 0) ? 1 : 0;
                }

                m_Skills.RemoveChildrenToPool();

                for (int i = 0; i < unitData.MainSkill.Length; i++)
                {
                    int skill = unitData.MainSkill[i];
                    var uiSkill = m_Skills.AddItemFromPool() as UI_SkillInfo;
                    uiSkill.SetSkill(skill);
                    uiSkill.m_seleted.selectedIndex = 0;
                }
            }
        }
    }
}
