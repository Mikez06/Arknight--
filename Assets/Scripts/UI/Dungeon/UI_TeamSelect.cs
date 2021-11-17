using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace DungeonUI
{
    partial class UI_TeamSelect
    {
        Dungeon Dungeon => DungeonManager.Instance.Dungeon;
        TaskCompletionSource<DungeonCard> tcs;

        DungeonCard NowSelect;
        DungeonCard Input;

        partial void Init()
        {
            m_units.onClickItem.Add(clickUnits);
            m_cancel.onClick.Add(() => { tcs.SetResult(Input); });
            m_ok.onClick.Add(() => { tcs.SetResult(NowSelect); });
            m_back.onClick.Add(() => { tcs.SetResult(null); });
        }

        public async Task<DungeonCard> Select(DungeonCard input)
        {
            this.Input = input;
            tcs = new TaskCompletionSource<DungeonCard>();
            UpdateUnits();
            doSelect(input);
            m_back.visible = input != null;
            var result= await tcs.Task;
            return result;
        }

        public void UpdateUnits()
        {
            List<DungeonCard> l = new List<DungeonCard>();
            l.AddRange(Dungeon.AllCards);
            foreach (var c in Dungeon.Cards) l.Remove(c);

            l = l.OrderByDescending(x => x.Upgrade).ThenByDescending(x => x.Level).ThenBy(x => x.UnitData.Rare).ThenBy(x => x.UnitId).ToList();

            if (Input != null) l.Insert(0, Input);

            m_units.RemoveChildrenToPool();

            foreach (var card in l)
            {
                var uiCard = m_units.AddItemFromPool() as UI_TeamUnit;
                uiCard.SetCard(card);
            }
        }

        public void UpdateSelect()
        {
            foreach (UI_TeamUnit item in m_units.GetChildren())
            {
                if (item.DungeonCard == NowSelect)
                {
                    item.m_select.selectedIndex = 1;
                }
                else
                {
                    item.m_select.selectedIndex = 0;
                }

            }
        }

        void doSelect(DungeonCard dungeonCard)
        {
            NowSelect = dungeonCard;
            m_leftUnit.SetCard(dungeonCard, -1);
            UpdateSelect();
        }

        void clickUnits(EventContext evt)
        {
            UI_TeamUnit teamUnit = evt.data as UI_TeamUnit;
            doSelect(teamUnit.DungeonCard);
        }
    }
}
