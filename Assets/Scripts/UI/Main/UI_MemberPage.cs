using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainUI
{
    partial class UI_MemberPage : IGameUIView
    {
        public int Sort;
        public bool Up;
        partial void Init()
        {
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Main>(UI_Main.URL);
            });
            m_level.onClick.Add(() => updateSort(0));
            m_rare.onClick.Add(() => updateSort(1));
            m_name.onClick.Add(() => updateSort(2));
        }

        public void Enter()
        {
            Sort = 0;
            Up = true;
            Flush();
        }

        public void Flush()
        {
            m_Cards.RemoveChildrenToPool();

            m_level.m_up.selectedIndex = Sort != 0 ? 0 : Up ? 1 : 2;
            m_rare.m_up.selectedIndex = Sort != 1 ? 0 : Up ? 1 : 2;
            m_name.m_up.selectedIndex = Sort != 2 ? 0 : Up ? 1 : 2;

            List<Card> cards;
            if (Up)
            {
                if (Sort == 0) cards = GameData.Instance.Cards.OrderBy(x => x.Level).ThenBy(x => x.UnitId).ToList();
                else if (Sort == 1) cards = GameData.Instance.Cards.OrderBy(x => x.UnitData.Rare).ThenBy(x => x.Level).ThenBy(x => x.UnitId).ToList();
                else cards = GameData.Instance.Cards.OrderBy(x => NPinyin.Pinyin.GetPinyin(x.UnitData.Name)).ToList();
            }
            else
            {
                if (Sort == 0) cards = GameData.Instance.Cards.OrderByDescending(x => x.Level).ThenBy(x => x.UnitId).ToList();
                else if (Sort == 1) cards = GameData.Instance.Cards.OrderByDescending(x => x.UnitData.Rare).ThenBy(x => x.Level).ThenBy(x => x.UnitId).ToList();
                else cards = GameData.Instance.Cards.OrderByDescending(x => NPinyin.Pinyin.GetPinyin(x.UnitData.Name)).ToList();
            }
            
            foreach (var card in cards)
            {
                var uiCard = m_Cards.AddItemFromPool() as UI_HalfUnit;
                uiCard.SetCard(card, card.DefaultUsingSkill);
            }
        }

        void updateSort(int index)
        {
            if (Sort == index) Up = !Up;
            else Sort = index;
            Flush();
        }
    }
}

