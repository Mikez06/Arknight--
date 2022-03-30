using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace MainUI
{
    partial class UI_Contract
    {
        MapData MapData => Database.Instance.Get<MapData>(mapId);
        string mapId;
        List<int> chooseList = new List<int>();
        const int lineHeight = 3;
        int maxLevel;

        partial void Init()
        {
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Main>(UI_Main.URL);
            });
            m_supportList.onClickItem.Add(onClickContract);
            m_conList.onClickItem.Add(onClickContract);
            m_start.onClick.Add(doBattle);
        }

        public void SetMap(string mapId)
        {
            this.mapId = mapId;
            m_mapName.text = MapData.MapName;
            chooseList.Clear();
            fresh();
        }

        void fresh()
        {
            int lastGroup = -1;
            int lineIndex = 0;
            m_supportList.RemoveChildrenToPool();
            m_conList.RemoveChildrenToPool();

            foreach (var contractId in MapData.Contracts)
            {
                var contractData = Database.Instance.Get<ContractData>(contractId);
                if (contractData.Level <= 0)
                {
                    var icon = m_supportList.AddItemFromPool() as UI_ContractIcon;
                    icon.ContractId = contractId;
                    icon.icon = "ui://Res/" + contractData.Icon;
                    icon.m_state.selectedIndex = chooseList.Contains(contractId) ? 4 : 0;
                }
                else
                {
                    if (lastGroup != contractData.Group && lastGroup!=-1)
                    {
                        for (int i = 0; i < 2 - (lineIndex-1) % lineHeight; i++)
                        {
                            var emptyIcon = m_conList.AddItemFromPool() as UI_ContractIcon;
                            emptyIcon.ContractId = -1;
                            emptyIcon.m_state.selectedIndex = 3;
                        }
                        lineIndex = 0;
                    }
                    lastGroup = contractData.Group;
                    var icon= m_conList.AddItemFromPool() as UI_ContractIcon;
                    icon.ContractId = contractId;
                    icon.icon = "ui://Res/" + contractData.Icon;
                    lineIndex++;
                    if (chooseList.Contains(contractId))
                    {
                        icon.m_state.selectedIndex = 1;
                    }
                    else if (chooseList.Any(x => Database.Instance.Get<ContractData>(x).Group == contractData.Group))
                    {
                        icon.m_state.selectedIndex = 2;
                    }
                    else
                    {
                        icon.m_state.selectedIndex = 0;
                    }
                }
            }

            m_choose.RemoveChildrenToPool();

            foreach (var contractId in chooseList)
            {
                var info = m_choose.AddItemFromPool() as UI_ContractInfo;
                var contractData = Database.Instance.Get<ContractData>(contractId);
                info.m_info.text = contractData.Description;
                if (contractData.Level <= 0) info.m_level.selectedIndex = 0;
                else info.m_level.selectedIndex = contractData.Level;
            }
            m_nowLevel.text = nowLevel().ToString();
        }

        int nowLevel()
        {
            if (chooseList.Any(x => Database.Instance.Get<ContractData>(x).Level <= 0))
            {
                return 0;
            }
            else return chooseList.Sum(x => Database.Instance.Get<ContractData>(x).Level);
        }

        void onClickContract(EventContext evt)
        {
            var icon = evt.data as UI_ContractIcon;
            if (icon.ContractId >= 0)
            {
                if (chooseList.Contains(icon.ContractId))
                {
                    chooseList.Remove(icon.ContractId);
                }
                else
                {
                    int group = Database.Instance.Get<ContractData>(icon.ContractId).Group;
                    if (group != 0) chooseList.RemoveAll(x => Database.Instance.Get<ContractData>(x).Group == group);
                    chooseList.Add(icon.ContractId);
                    chooseList.Sort();
                }
                fresh();
            }
        }

        async void doBattle()
        {
            var uiTeam = UIManager.Instance.ChangeView<UI_Team>(UI_Team.URL);
            uiTeam.IfGoBattle(true);

            int teamIndex = await uiTeam.ChooseTeam();
            if (teamIndex < 0)
            {
                UIManager.Instance.ChangeView<GComponent>(URL);
                return;
            }

            await BattleManager.Instance.StartBattle(new BattleInput()
            {
                MapName = mapId,
                Seed = 0,
                Team = GameData.Instance.Teams[teamIndex],
                Contracts = new List<int>(chooseList),
            });
            var battle = BattleManager.Instance.Battle;
            if (battle.Win)
            {
                if (nowLevel() > maxLevel)
                {
                    maxLevel = nowLevel();
                    m_nowLevel.text = maxLevel.ToString();
                }
            }
        }
    }
}
