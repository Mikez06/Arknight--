using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace DungeonUI
{
    partial class UI_Battle
    {
        Dungeon Dungeon => DungeonManager.Instance.Dungeon;
        UI_TeamUnit[] uiUnits = new UI_TeamUnit[12];
        TaskCompletionSource<bool> tcs;
        MapInfo MapData;

        HashSet<int> contracts = new HashSet<int>();
        partial void Init()
        {
            for (int i = 0; i < uiUnits.Length; i++)
            {
                int k = i;
                uiUnits[i] = GetChild("u" + i) as UI_TeamUnit;
                uiUnits[i].onClick.Add(async () =>
                {
                    var teamSelect = UIManager.Instance.ChangeView<UI_TeamSelect>(UI_TeamSelect.URL);
                    var input = k >= Dungeon.Cards.Count ? null : Dungeon.Cards[k];
                    var result = await teamSelect.Select(input);
                    if (input != null && result == null)
                    {
                        Dungeon.Cards.Remove(input);
                    }
                    if (input != null && result != null)
                    {
                        Dungeon.Cards[k] = result;
                    }
                    if (input == null && result != null)
                    {
                        Dungeon.Cards.Add(result);
                    }
                    UIManager.Instance.ChangeView<GComponent>(URL);
                    Fresh();
                });
            }
            m_Contracts.onClickItem.Add(clickContract);
            m_DoBattle.onClick.Add(() =>
            {
                tcs?.TrySetResult(true);
            });
        }

        public async Task<HashSet<int>> BuildTeam(string mapId)
        {
            tcs = new TaskCompletionSource<bool>();

            MapData = Database.Instance.GetMap(mapId);
            m_Contracts.RemoveChildrenToPool();
            if (MapData.Contracts != null)
                foreach (var id in MapData.Contracts)
                {
                    var contractData = Database.Instance.Get<ContractData>(id);
                    var uiContract = m_Contracts.AddItemFromPool() as UI_BattleContract;
                    uiContract.m_icon.icon = contractData.Icon.ToContractIcon();
                    uiContract.m_TagName.text = contractData.Name;
                }
            contracts.Clear();
            Fresh();
            await tcs.Task;
            return contracts;
        }

        public void Fresh()
        {
            freshContracts();
            m_MapName.text = MapData.MapName;
            for (int i = 0; i < uiUnits.Length; i++)
            {
                if (i>= Dungeon.MaxCardCount)
                {
                    uiUnits[i].m_State.selectedIndex = 2;
                }
                else if (i>=Dungeon.Cards.Count)
                {
                    uiUnits[i].SetCard(null);
                }
                else
                {
                    uiUnits[i].SetCard(Dungeon.Cards[i]);
                }
            }
        }

        void freshContracts()
        {
            for (int i = 0; i < m_Contracts.numItems; i++)
            {
                var uiContract = m_Contracts.GetChildAt(i) as UI_BattleContract;
                uiContract.m_button.selectedIndex = contracts.Contains(i) ? 0 : 1;
            }
        }

        void clickContract(EventContext evt)
        {
            int index = m_Contracts.GetChildIndex(evt.data as GObject);
            if (!contracts.Contains(index))
                contracts.Add(index);
            else
                contracts.Remove(index);
            freshContracts();
        }
    }
}
