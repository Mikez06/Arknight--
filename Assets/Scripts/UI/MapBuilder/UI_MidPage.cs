using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapBuilderUI
{
    partial class UI_MidPage
    {
        public MapInfo MapInfo => (parent as UI_MapBuilder).MapInfo;
        public UnitInfo NowSelect;
        TaskCompletionSource<string> tcs;

        partial void Init()
        {
            m_selectBack.onClick.Add(() => { tcs.TrySetCanceled(); m_selectEnemy.selectedIndex = 0; });
            m_filterName.onFocusOut.Add(filterList);
            m_AddWave.onClick.Add(addUnit);
            m_DeleteWave.onClick.Add(() =>
            {
                if (NowSelect == null) return;
                MapInfo.UnitInfos.Remove(NowSelect);
                Fresh();
            });
            m_unitList.onClickItem.Add((x) =>
            {
                var unitUI = x.data as UI_MidInfo;
                NowSelect = unitUI.UnitInfo;
            });
            m_filterList.onClickItem.Add((x) =>
            {
                var enemyUI = x.data as UI_MidUnitInfo;
                tcs.TrySetResult(enemyUI.UnitData.Id);
            });
            m_Hide_2.onClick.Add(() =>
            {
                m_Hide.selectedIndex = m_Hide.selectedIndex == 0 ? 1 : 0;
            });
        }

        public void Fresh()
        {
            m_unitList.RemoveChildrenToPool();
            foreach (var unitInfo in MapInfo.UnitInfos)
            {
                var unitInfoUI = m_unitList.AddItemFromPool() as UI_MidInfo;
                unitInfoUI.SetInfo(unitInfo);
                unitInfoUI.selected = NowSelect == unitInfo;
            }
        }

        public async Task<string> Choose()
        {
            m_selectEnemy.selectedIndex = 1;
            filterList();
            tcs = new TaskCompletionSource<string>();
            var result = await tcs.Task;
            m_selectEnemy.selectedIndex = 0;
            return result;
        }
        void filterList()
        {
            var list = Database.Instance.GetAll<UnitData>().Where(x => x.Type == "中立单位");
            list = list.Where(x => x.Name == null || x.Name.Contains(m_filterName.text));
            m_filterList.RemoveChildrenToPool();
            foreach (var unitData in list)
            {
                var enemyInfoUI = m_filterList.AddItemFromPool() as UI_MidUnitInfo;
                enemyInfoUI.SetInfo(unitData);
            }
        }

        async void addUnit()
        {
            m_Hide.selectedIndex = 2;
            var grid = await MapManager.Instance.SelectGrid();
            MapInfo.UnitInfos.Add(new UnitInfo()
            {
                X=grid.X,
                Y=grid.Y,                
            });
            Fresh();
            m_Hide.selectedIndex = 0;
        }

        List<Transform> Points = new List<Transform>();
        Pool<Transform> p = new Pool<Transform>();

        public void Despawn()
        {
            foreach (var t in Points)
            {
                this.p.Despawn(t);
            }
            Points.Clear();
        }
        public void UpdatePoints()
        {
            //if (!string.IsNullOrEmpty((parent as UI_MapBuilder).scene)) return;
            Despawn();
            foreach (var unitInfo in MapInfo.UnitInfos)
            {
                var g = p.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.UnitPath + Database.Instance.Get<UnitData>(unitInfo.UnitId).Model).transform, MapManager.Instance.Grids[unitInfo.X, unitInfo.Y].transform.position);
                var m = g.GetComponent<UnitModel>();
                GameObject.Destroy(m);//防止捣乱
                Points.Add(g);
                g.transform.eulerAngles = new Vector3(0, Vector2.SignedAngle(unitInfo.Direction, Vector2.right), 0);
                //if (unitInfo.UnitId == "红门")
                //{
                //    ResHelper.GetAsset<GameObject>(PathHelper.UnitPath + Database.Instance.Get<UnitData>(unitInfo.UnitId).Model);
                //    var g = p.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.UnitPath + Database.Instance.Get<UnitData>(unitInfo.UnitId).Model).transform, new Vector3(unitInfo.X, 0, unitInfo.Y));
                //    Points.Add(g);
                //}
                //if (unitInfo.UnitId == "蓝门")
                //{
                //    var g = p.Spawn(ResHelper.GetAsset<GameObject>("Assets/Bundles/Units/end").transform, new Vector3(unitInfo.X, 0, unitInfo.Y));
                //    Points.Add(g);
                //}
            }
        }
    }
}
