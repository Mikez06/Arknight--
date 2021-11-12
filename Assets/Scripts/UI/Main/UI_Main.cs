using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_Main : IGameUIView
    {
        GameData gameData => GameData.Instance;
        partial void Init()
        {
            m_member.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_MemberPage>(UI_MemberPage.URL);
            });
            m_team.onClick.Add(() =>
            {
                var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
                uiTeam.IfGoBattle(false);
            });
            m_battle.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Battle>(UI_Battle.URL);
            });
            m_rogue.onClick.Add(() =>
            {
                DungeonManager.Instance.PrepareDungeon();
                UIManager.Instance.ChangeView<DungeonUI.UI_DungeonStart>(DungeonUI.UI_DungeonStart.URL);
            });
        }

        public void Enter()
        {
            Flush();
        }

        public void Flush()
        {
            if (gameData.Teams[0].Cards.Count > 0)
            {
                string picName = Database.Instance.Get<UnitData>(gameData.Teams[0].Cards[0].UnitId).StandPic;
                m_standPic.texture = new NTexture(ResHelper.GetAsset<Texture>(PathHelper.SpritePath + picName));
            }
        }
    }
}
