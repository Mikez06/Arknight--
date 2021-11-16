using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace MainUI
{
    partial class UI_Battle
    {
        partial void Init()
        {
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Main>(UI_Main.URL);
            });
            foreach (var child in m_world.GetChildren())
            {
                child.onClick.Add(goTeamPage);
            }
        }

        async void goTeamPage(EventContext evt)
        {
            var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
            var battleLevel = (evt.sender as GObject).data as string;
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
