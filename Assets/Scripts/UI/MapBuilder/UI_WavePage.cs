using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace MapBuilderUI
{
    partial class UI_WavePage
    {
        public MapInfo MapInfo => (parent as UI_MapBuilder).MapInfo;
        public WaveInfo NowSelect;
        string[] DropInfo;
        TaskCompletionSource<int> tcs;
        partial void Init()
        {
            m_selectBack.onClick.Add(() => { tcs.TrySetCanceled(); m_selectEnemy.selectedIndex = 0; });
            m_filterName.onFocusOut.Add(filterList);
            m_AddWave.onClick.Add(() =>
            {
                var info = new WaveInfo()
                {
                    Path = DropInfo.Length > 0 ? DropInfo[0] : "",
                    Count = 1,
                };
                MapInfo.WaveInfos.Add(info);
                Fresh();
            });
            m_CopyWave.onClick.Add(() =>
            {
                int index = MapInfo.WaveInfos.IndexOf(NowSelect);
                WaveInfo info;
                if (NowSelect == null)
                {
                    info = new WaveInfo()
                    {
                        Path = DropInfo.Length > 0 ? DropInfo[0] : "",
                        Count = 1,
                    };
                }
                else
                {
                    info = JsonHelper.Clone<WaveInfo>(NowSelect);
                }
                if (index == -1)
                {
                    MapInfo.WaveInfos.Add(info);
                }
                else
                {
                    MapInfo.WaveInfos.Insert(index + 1, info);
                }
                Fresh();
            });
            m_DeleteWave.onClick.Add(() =>
            {
                if (NowSelect == null) return;
                MapInfo.WaveInfos.Remove(NowSelect);
                Fresh();
                FreshPath();
            });
            m_wavwList.onClickItem.Add((x) =>
            {
                var pathUI = x.data as UI_WaveInfo;
                NowSelect = pathUI.WaveInfo;
                FreshPath();
            });
            m_filterList.onClickItem.Add((x) =>
            {
                var enemyUI = x.data as UI_EnemyInfo;
                tcs.TrySetResult(enemyUI.UnitData == null ? -1 : Database.Instance.GetIndex<UnitData>(enemyUI.UnitData));
            });
            m_Hide_2.onClick.Add(() =>
            {
                m_Hide.selectedIndex = m_Hide.selectedIndex == 0 ? 1 : 0;
            });
            m_filterList.SetVirtual();
            m_filterList.itemRenderer = filterRender;
        }

        public void Fresh()
        {
            DropInfo = MapInfo.PathInfos.Select(x => x.Name).ToArray();
            m_wavwList.RemoveChildrenToPool();
            foreach (var waveInfo in MapInfo.WaveInfos)
            {
                var waveInfoUI = m_wavwList.AddItemFromPool() as UI_WaveInfo;
                waveInfoUI.SetInfo(waveInfo, DropInfo);
                waveInfoUI.selected = NowSelect == waveInfo;
            }
            FreshPath();
        }

        void FreshPath()
        {
            if (NowSelect != null && !string.IsNullOrEmpty(NowSelect.Path))
            {
                var p = MapInfo.PathInfos.Find(x => x.Name == NowSelect.Path);
                if (p != null)
                    MapManager.Instance.ShowPath(p.Path);
                else
                    MapManager.Instance.ShowPath(null);
            }
            else
                MapManager.Instance.ShowPath(null);
        }

        public async Task<int> Choose()
        {
            m_selectEnemy.selectedIndex = 1;
            filterList();
            tcs = new TaskCompletionSource<int>();
            var result= await tcs.Task;
            m_selectEnemy.selectedIndex = 0;
            return result;
        }

        List<UnitData> filters = new List<UnitData>();
        void filterList()
        {
            filters = Database.Instance.GetAll<UnitData>().Where(x => x.Type == "敌人").Where(x => x.Name == null || x.Name.Contains(m_filterName.text)).ToList();
            filters.Insert(0, null);
            m_filterList.numItems = filters.Count;
            //m_filterList.RemoveChildrenToPool();
            //(m_filterList.AddItemFromPool() as UI_EnemyInfo).SetInfo(null);
            //foreach (var unitData in list)
            //{
            //    var enemyInfoUI = m_filterList.AddItemFromPool() as UI_EnemyInfo;
            //    enemyInfoUI.SetInfo(unitData);
            //}
        }

        void filterRender(int index,GObject item)
        {
            var enemyInfoUI = item as UI_EnemyInfo;
            enemyInfoUI.SetInfo(filters[index]);
        }
    }
}
