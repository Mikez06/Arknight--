using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace MapBuilderUI
{
    partial class UI_MapInfo
    {
        public MapInfo MapInfo => (parent as UI_MapBuilder).MapInfo;

        partial void Init()
        {
            
            m_Contract.RemoveChildrenToPool();
            foreach (var contract in Database.Instance.GetAll<ContractData>())
            {
                var contractUI = m_Contract.AddItemFromPool() as UI_ContractIcon;
                contractUI.SetContract(contract);
            }
            m_Contract.onClickItem.Add(chooseContract);
            m_MapName.onChanged.Add(() => { MapInfo.MapName = m_MapName.text; });
            m_MapDesc.onChanged.Add(() => { MapInfo.Description = m_MapDesc.text; });
            m_InitHp.onChanged.Add(() => { MapInfo.InitHp = int.Parse(m_InitHp.text); });
            m_InitCost.onChanged.Add(() => { MapInfo.InitCost = int.Parse(m_InitCost.text); });
            m_BuildCount.onChanged.Add(() => { MapInfo.MaxBuildCount = int.Parse(m_BuildCount.text); });
            m_MaxCost.onChanged.Add(() => { MapInfo.MaxCost = int.Parse(m_MaxCost.text); });
            m_NoBuildLimit.onClick.Add(() => { MapInfo.NoBuildLimit = !MapInfo.NoBuildLimit; });
            m_BoxCount.onChanged.Add(() => { MapInfo.BoxCount = int.Parse(m_BoxCount.text); });
        }

        public void Fresh()
        {
            m_MapName.text = MapInfo.MapName;
            m_MapDesc.text = MapInfo.Description;
            m_InitHp.text = MapInfo.InitHp.ToString();
            m_InitCost.text = MapInfo.InitCost.ToString();
            m_BuildCount.text = MapInfo.MaxBuildCount.ToString();
            m_MaxCost.text = MapInfo.MaxCost.ToString();
            m_NoBuildLimit.selected = !MapInfo.NoBuildLimit;
            m_BoxCount.text = MapInfo.BoxCount.ToString();
            for (int i = 0; i < m_Contract.numItems; i++)
            {
                var contractUI = m_Contract.GetChildAt(i) as UI_ContractIcon;
                contractUI.m_state.selectedIndex = (MapInfo.Contracts != null && MapInfo.Contracts.Contains(contractUI.Contract.Id)) ? 1 : 0;
            }
        }

        void chooseContract(EventContext evt)
        {
            var contractUI = evt.data as UI_ContractIcon;
            if (MapInfo.Contracts.Contains(contractUI.Contract.Id))
            {
                MapInfo.Contracts.Remove(contractUI.Contract.Id);
                contractUI.m_state.selectedIndex = 0;
            }
            else
            {
                MapInfo.Contracts.Add(contractUI.Contract.Id);
                contractUI.m_state.selectedIndex = 1;
            }
        }
    }
}
