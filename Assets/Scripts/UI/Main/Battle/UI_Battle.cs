using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_Battle
    {
        TaskCompletionSource<bool> goTcs;
        partial void Init()
        {
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Main>(UI_Main.URL);
                m_showLevelInfo.selectedIndex = 0;
            });
            foreach (var child in m_world.GetChildren())
            {
                if (child is UI_BattleInfo)
                    child.onClick.Add(doBattle);
            }
            m_levelInfo.m_start.onClick.Add(() => { goTcs.TrySetResult(true); });
            m_world.m_levelBack.onClick.Add(cancelLevelInfo);
            m_world.draggable = true;
            m_world.onDragStart.Add((x) =>
            {
                x.PreventDefault();
                cancelLevelInfo();
            });
        }

        void cancelLevelInfo()
        {
            if (m_showLevelInfo.selectedIndex == 1)
            {
                goTcs.TrySetResult(false);
                m_showLevelInfo.selectedIndex = 0;
            }
        }

        async void doBattle(EventContext evt)
        {
            var sender = evt.sender as GObject;
            var battleLevel = sender.data as string;

            var pos = sender.LocalToGlobal(Vector2.zero);
            if (pos.x < 100)
            {
                m_world.scrollPane.SetPosX((100f - sender.x), true);
            }
            if (pos.x > 920)
            {
                m_world.scrollPane.SetPosX(sender.x - 920f, true);
                //m_world.scrollPane.ScrollToView(sender);
            }

            m_levelInfo.SetInfo(battleLevel);
            m_showLevelInfo.selectedIndex = 1;
            goTcs = new TaskCompletionSource<bool>();
            var ifGo= await goTcs.Task;
            if (ifGo)
            {
                var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
                uiTeam.IfGoBattle(true);
                var teamIndex = await uiTeam.ChooseTeam();
                if (teamIndex < 0)
                {
                    UIManager.Instance.ChangeView<GComponent>(URL);
                }
                else
                {
                    await BattleManager.Instance.StartBattle(new BattleInput()
                    {
                        MapName = battleLevel,
                        Seed = 0,
                        Team = GameData.Instance.Teams[teamIndex],
                    });
                    UIManager.Instance.ChangeView<GComponent>(URL);
                }
            }
        }
    }
}
