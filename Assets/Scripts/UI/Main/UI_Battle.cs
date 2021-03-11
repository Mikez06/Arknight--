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

        void goTeamPage(EventContext evt)
        {
            var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
            uiTeam.IfGoBattle(true);
            uiTeam.BattleLevel = (evt.sender as GObject).data as string;
        }
    }
}
