using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_UnitInfo
    {
        public Card Card;
        UnitData UnitData => Card.UnitData;
        GObjectPool pool;

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        partial void Init()
        {
            pool = new GObjectPool(container.cachedTransform);
            m_back.onClick.Add(() => { tcs?.TrySetResult(true); });
            m_Skills.onClickItem.Add(clickSkill);
        }

        public async Task ShowUnit(Card card)
        {
            this.Card = card;
            Fresh();
            tcs = new TaskCompletionSource<bool>();
            await tcs.Task;
        }

        public void Fresh()
        {
            m_LV.text = UnitData.Level.ToString();
            m_hp.text = (UnitData.Hp + UnitData.HpEx).ToString();
            m_atk.text = (UnitData.Attack + UnitData.AttackEx).ToString();
            m_def.text = (UnitData.Defence + UnitData.DefenceEx).ToString();
            m_magDef.text = (UnitData.MagicDefence + UnitData.MagicDefenceEx).ToString();
            m_rebuild.text = (UnitData.ResetTime + UnitData.ResetTimeEx).ToString();
            m_cost.text = (UnitData.Cost + UnitData.CostEx).ToString();
            m_stop.text = UnitData.StopCount.ToString();
            m_agi.text = UnitData.AttackGap.ToString();
            m_engName.text = UnitData.engName;
            m_Name.text = UnitData.Name;
            m_Pro.m_p.selectedIndex = (int)UnitData.Profession;
            m_upgrade.m_upgrade.selectedIndex = UnitData.Upgrade;
            m_star.selectedIndex = UnitData.Rare;

            var mainSkill = Database.Instance.Get<SkillData>(UnitData.Skills[0]);
            foreach (var item in m_attackArea.GetChildren())
            {
                pool.ReturnObject(item);
            }
            m_attackArea.RemoveChildren();

            float midX = (mainSkill.AttackPoints.Max(x => x.x) + mainSkill.AttackPoints.Min(x => x.x)) / 2f;
            float midY = (mainSkill.AttackPoints.Max(x => x.y) + mainSkill.AttackPoints.Min(x => x.y)) / 2f;
            foreach (var point in mainSkill.AttackPoints)
            {
                var a = pool.GetObject(UI_AttackArea.URL) as UI_AttackArea;
                m_attackArea.AddChild(a);
                a.xy = new Vector2((point.x - midX) * 33-12f, (point.y - midY) * 33 - 12f);
                a.m_type.selectedIndex = (point.x == 0 && point.y == 0) ? 1 : 0;
            }
            m_place.text = UnitData.SetPos;
            var ts = "";
            if (UnitData.Tags!=null)
            foreach (var s in UnitData.Tags)
                {
                    ts += s + " ";
                }
            m_tags.text = ts;

            m_abs.RemoveChildrenToPool();
            if (UnitData.Ablititys != null)
            {
                foreach (var kv in UnitData.Ablititys)
                {
                    var uiAb = m_abs.AddItemFromPool().asCom;
                    uiAb.text = kv.Key;
                }
            }
            m_standPic.texture = new NTexture(ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + UnitData.StandPic));
            m_standPic.position = new Vector2(UnitData.StandPicPos[0], UnitData.StandPicPos[1]);
            m_standPic.size = new Vector2(UnitData.StandPicPos[2], UnitData.StandPicPos[3]);

            updateSkill();
        }

        void updateSkill()
        {
            m_Skills.RemoveChildrenToPool();
            if (UnitData.MainSkill != null)
                for (int i = 0; i < UnitData.MainSkill.Length; i++)
                {
                    var uiSkill = m_Skills.AddItemFromPool() as UI_UnitSkillIcon;
                    uiSkill.SetSkill(UnitData.MainSkill[i]);
                    uiSkill.m_default.selectedIndex = Card.DefaultUsingSkill == i ? 1 : 0;
                }
        }

        void clickSkill(EventContext evt)
        {
            var index = m_Skills.GetChildIndex(evt.data as GComponent);
            Card.DefaultUsingSkill = index;
            updateSkill();
        }
    }
}
