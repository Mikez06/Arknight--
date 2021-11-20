using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace DungeonUI
{
    partial class UI_Recruit
    {
        Dungeon Dungeon => DungeonManager.Instance.Dungeon;
        TaskCompletionSource<int> tcs;

        int nowSelect;
        UnitData nowSelectUnitData => Database.Instance.Get<UnitData>(nowSelect);

        partial void Init()
        {
            m_units.onClickItem.Add(clickUnits);
            m_cancel.onClick.Add(() => { tcs.SetResult(-1); });
            m_ok.onClick.Add(() => { tcs.SetResult(nowSelect); });
        }

        public async Task<int> Choose(List<UnitData> unitDatas)
        {
            tcs = new TaskCompletionSource<int>();
            m_units.RemoveChildrenToPool();
            foreach (var unitData in unitDatas)
            {
                int id = Database.Instance.GetIndex(unitData);
                var item = m_units.AddItemFromPool() as UI_RecruitUnit;
                item.SetData(id);
            }
            var result = await tcs.Task;
            Log.Debug($"选择获得了{nowSelectUnitData.Name}");
            return result;
        }

        public void UpdateSelect()
        {
            foreach (UI_RecruitUnit item in m_units.GetChildren())
            {
                if (item.Id == nowSelect)
                {
                    item.m_select.selectedIndex = 1;
                }
                else
                {
                    item.m_select.selectedIndex = 0;
                }

            }
        }

        void doSelect(int id)
        {
            nowSelect = id;
            m_leftUnit.SetData(nowSelect == -1 ? null : nowSelectUnitData);
            UpdateSelect();
        }

        void clickUnits(EventContext evt)
        {
            UI_RecruitUnit teamUnit = evt.data as UI_RecruitUnit;
            doSelect(teamUnit.Id);
        }
    }
}
