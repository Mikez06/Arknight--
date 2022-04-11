using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_Battle:IGameUIView
    {
        TaskCompletionSource<bool> goTcs;
        List<int> contracts = new List<int>();
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

            m_levelInfo.m_Train.onClick.Add(() => m_contractChoose.selectedIndex = 1);
            m_contractBack.onClick.Add(() => m_contractChoose.selectedIndex = 0);

            m_contracts.RemoveChildrenToPool();
            ContractData[] array = Database.Instance.GetAll<ContractData>();
            for (int i = 0; i < array.Length; i++)
            {
                int k = i;
                ContractData cData = array[i];
                var uiContract = m_contracts.AddItemFromPool() as DungeonUI.UI_BattleContract;
                uiContract.m_icon.icon = cData.Icon.ToContractIcon();
                uiContract.m_TagName.text = cData.Name;
                uiContract.onClick.Add(() => { if (contracts.Contains(k)) contracts.Remove(k); else contracts.Add(k); freshContract(); });
            }
            freshContract();
        }

        void freshContract()
        {
            for (int i = 0; i < m_contracts.numItems; i++)
            {
                var uiContract = m_contracts.GetChildAt(i) as DungeonUI.UI_BattleContract;
                uiContract.m_button.selectedIndex = contracts.Contains(i) ? 0 : 1;
            }
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

            var mapInfo = Database.Instance.GetMap(battleLevel);
            m_levelInfo.SetInfo(battleLevel);
            m_showLevelInfo.selectedIndex = 1;

            var teamIndex = -1;
            while (teamIndex < 0)
            {
                goTcs = new TaskCompletionSource<bool>();
                var ifGo = await goTcs.Task;
                if (ifGo)
                {
                    if (mapInfo.Contracts != null && mapInfo.Contracts.Count > 0)
                    {
                        var uiContract = UIManager.Instance.ChangeView<UI_Contract>(UI_Contract.URL);
                        uiContract.SetMap(battleLevel);
                        await uiContract.WaitFinish();
                        m_showLevelInfo.selectedIndex = 0;
                        UIManager.Instance.ChangeView<GComponent>(URL);
                        return;
                    }
                    else
                    {
                        var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
                        uiTeam.IfGoBattle(true);

                        teamIndex = await uiTeam.ChooseTeam();
                        if (teamIndex < 0)
                        {
                            UIManager.Instance.ChangeView<GComponent>(URL);
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            await BattleManager.Instance.StartBattle(new BattleInput()
            {
                MapName = battleLevel,
                Seed = 0,
                Team = GameData.Instance.Teams[teamIndex],
                Contracts = new List<int>(contracts),
            }); 
            m_showLevelInfo.selectedIndex = 0;
            UIManager.Instance.ChangeView<GComponent>(URL);
        }

        void freshMaps()
        {
            int fileIndex = 0;
            var maps = m_world.GetChildren().Select(x => x.data).ToArray();
            foreach (var file in Database.Instance.GetMaps())
            {
                if (maps.Contains(file)) continue;
                var battleInfo = UIPackage.CreateObject("MainUI", "BattleInfo").asCom;
                battleInfo.text = file;
                battleInfo.data = file;
                m_world.AddChild(battleInfo);
                battleInfo.onClick.Add(doBattle);
                battleInfo.position = new Vector3((int)(fileIndex * 100) / 1000 * 200 + 100, 150+(fileIndex * 100) % 1000f);
                fileIndex++;
            }
        }

        public void Enter()
        {
            freshMaps();
        }
    }
}
