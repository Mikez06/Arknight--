using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_Team : IGameUIView
    {
        GameData gameData => GameData.Instance;
        protected UI_TeamUnit[] teamUnits = new UI_TeamUnit[12];
        protected GButton[] teamBtns = new GButton[4];
        public int TeamIndex;

        public string backPage;
        public string BattleLevel;


        partial void Init()
        {
            for (int i = 0; i < teamUnits.Length; i++)
            {
                int k = i;
                teamUnits[i] = GetChild("u" + i) as UI_TeamUnit;
                teamUnits[i].onClick.Add(() =>
                {
                    var selectTeam = UIManager.Instance.ChangeView<UI_TeamSelect>(UI_TeamSelect.URL);
                    selectTeam.ChangeUnit(gameData.Teams[TeamIndex], teamUnits[k].Card);
                });
            }

            for (int i = 0; i < teamBtns.Length; i++)
            {
                teamBtns[i] = GetChild("team" + i).asButton;
            }
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<GComponent>(backPage);
            });
            m_quickTeam.onClick.Add(() =>
            {
                var selectTeam = UIManager.Instance.ChangeView<UI_TeamSelect>(UI_TeamSelect.URL);
                selectTeam.QuickSelect(gameData.Teams[TeamIndex]);
            });
            m_right.onClick.Add(() =>
            {
                TeamIndex = (TeamIndex + 1) % gameData.Teams.Length;
                Flush();
            });
            m_left.onClick.Add(() =>
            {
                TeamIndex = (TeamIndex - 1);
                if (TeamIndex<0) TeamIndex+= gameData.Teams.Length;
                Flush();
            });
            m_delete.onClick.Add(() =>
            {
                gameData.Teams[TeamIndex].Cards.Clear();
                gameData.Teams[TeamIndex].UnitSkill.Clear();
                Flush();
            });
            m_support.onClick.Add(() =>
            {
                TipManager.Instance.ShowTip("不支持友招");
            });
            m_battle.onClick.Add(() =>
            {
                BattleManager.Instance.StartBattle(new BattleInput()
                {
                    MapName = BattleLevel,
                    Seed = 0,
                    StartCost = 50,
                    Team = gameData.Teams[TeamIndex],
                });
            });
        }

        public void IfGoBattle(bool bo)
        {
            if (bo)
            {
                m_goBattle.selectedIndex = 0;
                backPage = UI_Battle.URL;
            }
            else
            {
                m_goBattle.selectedIndex = 1;
                backPage = UI_Main.URL;
            }
        }

        public void Enter()
        {
            Flush();
        }

        public void Flush()
        {
            for (int i = 0; i < teamBtns.Length; i++)
            {
                teamBtns[i].selected = i != TeamIndex;
            }
            for (int i = 0; i < teamUnits.Length; i++)
            {
                if (i < gameData.Teams[TeamIndex].Cards.Count)
                {
                    teamUnits[i].SetCard(gameData.Teams[TeamIndex].Cards[i], gameData.Teams[TeamIndex].UnitSkill[i]);
                }
                else
                {
                    teamUnits[i].SetCard(null, 0);
                }
            }
        }
    }
}
