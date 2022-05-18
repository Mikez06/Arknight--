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
            m_Map.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<MapBuilderUI.UI_MapBuilder>(MapBuilderUI.UI_MapBuilder.URL);
            });
            m_member.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_MemberPage>(UI_MemberPage.URL);
            });
            m_team.onClick.Add(async () =>
            {
                var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
                uiTeam.IfGoBattle(false);
                await uiTeam.ChooseTeam();
                UIManager.Instance.ChangeView<GComponent>(URL);
            });
            m_battle.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Battle>(UI_Battle.URL);
            });
            m_rogue.onClick.Add(() =>
            {
            //    DungeonManager.Instance.PrepareDungeon();
            //    UIManager.Instance.ChangeView<DungeonUI.UI_DungeonStart>(DungeonUI.UI_DungeonStart.URL);
            });
            onRightClick.Add(async () =>
            {
                var ui = UIManager.Instance.ChangeView<DungeonUI.UI_Dialogue>(DungeonUI.UI_Dialogue.URL);
                await ui.StartDialogue("初始事件");
                UIManager.Instance.ChangeView<GComponent>(URL);
            });
            m_Name.onFocusOut.Add(() =>
            {
                if (GameData.Instance.Name != m_Name.text)
                {
                    GameData.Instance.Name = m_Name.text;
                    SaveHelper.SaveData();
                }
            });
            m_Setting.onClick.Add(() =>
            {
                m_settingC.selectedIndex = 1;
            });
            m_close.onClick.Add(() =>
            {
                m_settingC.selectedIndex = 0;
                SaveHelper.SaveData();
            });
            m_bgm.onChanged.Add(() =>
            {
                GameData.Instance.Bgm = (float)m_bgm.value / 100f;
            });
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void Enter()
        {
            Flush();
        }

        public void Flush()
        {
            m_bgm.value = GameData.Instance.Bgm * 100;
            m_Name.text = GameData.Instance.Name;
            m_Version.text = Application.version;
            if (gameData.Teams[0].Cards.Count > 0)
            {
                string picName = Database.Instance.Get<UnitData>(gameData.Teams[0].Cards[0].UnitId).StandPic;
                m_standPic.texture = new NTexture(ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + picName));
            }
        }
    }
}
