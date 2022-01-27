using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_TeamSelect : IGameUIView
    {
        GameData gameData => GameData.Instance;

        public int Sort;
        public bool Up;

        /// <summary>
        /// 0:快速编队 1:单选编队
        /// </summary>
        public int mode;

        public Team baseTeam;
        public Card baseCard;
        public int baseSkill;

        public List<Card> NowTeam = new List<Card>();
        public List<int> NowSkill = new List<int>();

        partial void Init()
        {
            m_level.onClick.Add(() => updateSort(0));
            m_rare.onClick.Add(() => updateSort(1));
            m_name.onClick.Add(() => updateSort(2));
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
            });
            m_cancel.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
            });
            m_ok.onClick.Add(() =>
            {
                if (m_quick.selectedIndex == 1)
                {
                    //快速编队
                    baseTeam.Cards.Clear();
                    baseTeam.Cards.AddRange(NowTeam);
                    baseTeam.UnitSkill.Clear();
                    baseTeam.UnitSkill.AddRange(NowSkill);
                }
                else
                {
                    int index = baseTeam.Cards.IndexOf(baseCard);
                    if (NowTeam.Count == 0)
                    {
                        if (index != -1)
                        {
                            //移除
                            baseTeam.Cards.RemoveAt(index);
                            baseTeam.UnitSkill.RemoveAt(index);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (index != -1)
                        {
                            baseTeam.Cards[index] = NowTeam[0];
                            baseTeam.UnitSkill[index] = NowSkill[0];
                        }
                        else
                        {
                            baseTeam.Cards.Add( NowTeam[0]);
                            baseTeam.UnitSkill.Add(NowSkill[0]);
                        }
                    }
                }
                UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
            });
            m_Cards.onClickItem.Add(clickCard);
            m_leftUnit.m_Skills.onClickItem.Add(x =>
            {
                int index = m_leftUnit.m_Skills.GetChildIndex((x.data as GObject));
                int cardIndex = NowTeam.IndexOf(m_leftUnit.Card);
                if (cardIndex < 0)
                {
                    return;
                }
                NowSkill[cardIndex] = index;
                m_leftUnit.SetCard(m_leftUnit.Card, index);
                Flush(false);
            });
        }

        public void Enter()
        {

        }

        public void QuickSelect(Team team)
        {
            m_quick.selectedIndex = 1;
            baseTeam = team;
            NowTeam.Clear();
            NowTeam.AddRange(baseTeam.Cards);
            NowSkill.Clear();
            NowSkill.AddRange(baseTeam.UnitSkill);
            Flush(true);
            if (team.Cards.Count > 0)
            {
                m_leftUnit.SetCard(team.Cards[0], team.UnitSkill[0]);
            }
            else
            {
                m_leftUnit.SetCard(null, 0);
            }
        }

        public void ChangeUnit(Team team,Card card)
        {
            m_quick.selectedIndex = 0;
            baseTeam = team;
            baseCard = card;
            NowTeam.Clear();
            NowSkill.Clear();
            int index = team.Cards.IndexOf(card);
            if (index != -1)//-1表示新增卡
            {
                NowTeam.Add(card);
                NowSkill.Add(baseTeam.UnitSkill[index]);
            }
            Flush(true);
            if (card == null)
            {
                m_leftUnit.SetCard(null, 0);
            }
            else
            {
                m_leftUnit.SetCard(card, team.UnitSkill[team.Cards.IndexOf(card)]);
            }
        }

        public void Flush(bool resort)
        {
            if (resort)
            {
                m_Cards.RemoveChildrenToPool();

                m_level.m_up.selectedIndex = Sort != 0 ? 0 : Up ? 1 : 2;
                m_rare.m_up.selectedIndex = Sort != 1 ? 0 : Up ? 1 : 2;
                m_name.m_up.selectedIndex = Sort != 2 ? 0 : Up ? 1 : 2;

                List<Card> cards;
                if (Up)
                {
                    if (Sort == 0) cards = GameData.Instance.Cards.OrderBy(x => NowTeam.IndexOf(x) < 0 ? 1 : 0).ThenBy(x => x.Level).ThenBy(x => x.UnitId).ToList();
                    else if (Sort == 1) cards = GameData.Instance.Cards.OrderBy(x => NowTeam.IndexOf(x) < 0 ? 1 : 0).ThenBy(x => x.UnitData.Rare).ThenBy(x => x.Level).ThenBy(x => x.UnitId).ToList();
                    else cards = GameData.Instance.Cards.OrderBy(x => NowTeam.IndexOf(x) < 0 ? 1 : 0).ThenBy(x => NPinyin.Pinyin.GetPinyin(x.UnitData.Name)).ToList();
                }
                else
                {
                    if (Sort == 0) cards = GameData.Instance.Cards.OrderBy(x => NowTeam.IndexOf(x) < 0 ? 1 : 0).ThenByDescending(x => x.Level).ThenBy(x => x.UnitId).ToList();
                    else if (Sort == 1) cards = GameData.Instance.Cards.OrderBy(x => NowTeam.IndexOf(x) < 0 ? 1 : 0).ThenByDescending(x => x.UnitData.Rare).ThenBy(x => x.Level).ThenBy(x => x.UnitId).ToList();
                    else cards = GameData.Instance.Cards.OrderBy(x => NowTeam.IndexOf(x) < 0 ? 1 : 0).ThenByDescending(x => NPinyin.Pinyin.GetPinyin(x.UnitData.Name)).ToList();
                }

                if (m_quick.selectedIndex == 0)//单选模式下 剔除已在队里的卡
                {
                    foreach (var card in baseTeam.Cards)
                    {
                        if (card != baseCard) cards.Remove(card);
                    }
                }

                foreach (var card in cards)
                {
                    var uiCard = m_Cards.AddItemFromPool() as UI_HalfUnit;
                    int index = NowTeam.IndexOf(card);
                    uiCard.SetCard(card, index >= 0 ? NowSkill[index] : card.DefaultUsingSkill);
                    if (m_quick.selectedIndex == 0)//单选模式下，选中卡片高亮
                    {
                        uiCard.m_seletd.selectedIndex = NowTeam.Contains(card) ? 1 : 0;
                        uiCard.m_index.text = "";
                    }
                    else
                    {
                        uiCard.m_seletd.selectedIndex = NowTeam.Contains(card) ? 1 : 0;
                        uiCard.m_index.text = (NowTeam.IndexOf(card) + 1).ToString();
                    }
                }
            }
            else
            {
                foreach (UI_HalfUnit uiCard in m_Cards.GetChildren())
                {
                    var card = uiCard.Card;
                    int index = NowTeam.IndexOf(card);
                    uiCard.SetCard(card, index >= 0 ? NowSkill[index] : card.DefaultUsingSkill);
                    if (m_quick.selectedIndex == 0)//单选模式下，选中卡片高亮
                    {
                        uiCard.m_seletd.selectedIndex = NowTeam.Contains(card) ? 1 : 0;
                        uiCard.m_index.text = "";
                    }
                    else
                    {
                        uiCard.m_seletd.selectedIndex = NowTeam.Contains(card) ? 1 : 0;
                        uiCard.m_index.text = (NowTeam.IndexOf(card) + 1).ToString();
                    }
                }
            }
        }

        void clickCard(EventContext evt)
        {
            var card = (evt.data as UI_HalfUnit).Card;
            m_leftUnit.SetCard(card, card.DefaultUsingSkill);
            if (m_quick.selectedIndex == 0)
            {
                if (NowTeam.Contains(card))
                {
                    NowTeam.Clear();
                    NowSkill.Clear();
                }
                else
                {
                    NowTeam.Clear();
                    NowSkill.Clear();
                    NowTeam.Add(card);
                    NowSkill.Add(0);
                }
            }
            else
            {
                if (NowTeam.Contains(card))
                {
                    int index = NowTeam.IndexOf(card);
                    NowTeam.RemoveAt(index);
                    NowSkill.RemoveAt(index);
                }
                else if (NowTeam.Count < 12)
                {
                    NowTeam.Add(card);
                    NowSkill.Add(0);
                }
            }
            Flush(false);
        }

        void updateSort(int index)
        {
            if (Sort == index) Up = !Up;
            else Sort = index;
            Flush(true);
        }
    }
}
