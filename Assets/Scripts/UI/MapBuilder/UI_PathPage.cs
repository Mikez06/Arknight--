using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_PathPage
    {
        public MapInfo MapInfo => (parent as UI_MapBuilder).MapInfo;
        public PathInfo NowSelect;
        public PathPoint NowPoints;

        partial void Init()
        {
            m_AddPath.onClick.Add(() =>
            {
                var info = new PathInfo()
                {
                    Name = "p" + (MapInfo.PathInfos.Count + 1),
                    Path = new List<PathPoint>(),
                };
                MapInfo.PathInfos.Add(info);
                Fresh();
            });
            m_CopyPath.onClick.Add(() =>
            {
                int index = MapInfo.PathInfos.IndexOf(NowSelect);
                PathInfo info;
                if (NowSelect == null) 
                {
                    info = new PathInfo()
                    {
                        Name = "p" + (MapInfo.PathInfos.Count + 1),
                        Path = new List<PathPoint>(),
                    };
                }
                else
                {
                    info = JsonHelper.Clone<PathInfo>(NowSelect);
                    info.Name += "1";
                }
                if (index == -1)
                {
                    MapInfo.PathInfos.Add(info);
                }
                else
                {
                    MapInfo.PathInfos.Insert(index + 1, info);
                }
                Fresh();
            });
            m_DeletePath.onClick.Add(() =>
            {
                if (NowSelect == null) return;
                MapInfo.PathInfos.Remove(NowSelect);
                Fresh();
            });
            m_Paths.onClickItem.Add((x) =>
            {
                var pathUI = x.data as UI_PathInfo;
                NowSelect = pathUI.PathInfo;
                FreshPoints();
            });
            m_PathPoints.onClickItem.Add((x) =>
            {
                var pathUI = x.data as UI_PathPoint;
                NowPoints = pathUI.PathPoint;
                MapManager.Instance.ShowPath(NowSelect.Path, NowSelect.FlyPath);
            });
            m_AddPoint.onClick.Add(AddPoint);
            m_InsertPoint.onClick.Add(InsertPoint);
            m_DeletePoint.onClick.Add(() =>
            {
                if (NowPoints == null) return;
                NowSelect.Path.Remove(NowPoints);
                FreshPoints();
            });
        }

        public void Fresh()
        {
            m_Paths.RemoveChildrenToPool();
            foreach (var pathInfo in MapInfo.PathInfos)
            {
                var pathInfoUI = m_Paths.AddItemFromPool() as UI_PathInfo;
                pathInfoUI.SetInfo(pathInfo);
                pathInfoUI.selected = NowSelect == pathInfo;
            }
        }

        public void FreshPoints()
        {
            m_PathPoints.RemoveChildrenToPool();
            if (NowSelect == null)
            {
                MapManager.Instance.ShowPath(null);
                return;
            }
            foreach (var pointInfo in NowSelect.Path)
            {
                var pathPointUI = m_PathPoints.AddItemFromPool() as UI_PathPoint;
                pathPointUI.SetInfo(pointInfo);
                pathPointUI.selected = pointInfo == NowPoints;
            }
            MapManager.Instance.ShowPath(NowSelect.Path, NowSelect.FlyPath);
        }

        async void AddPoint()
        {
            if (NowSelect == null) return;
            m_select.selectedIndex = 1;
            var grid = await MapManager.Instance.SelectGrid();
            NowSelect.Path.Add(new PathPoint()
            {
                Delay = 0,
                DirectMove = false,
                HideMove = false,
                Pos = grid.transform.position,
            });
            m_select.selectedIndex = 0;
            FreshPoints();
        }
        async void InsertPoint()
        {
            if (NowSelect == null) return;
            m_select.selectedIndex = 1;
            var grid = await MapManager.Instance.SelectGrid();
            int index = m_PathPoints.selectedIndex;
            if (index < 0)
                NowSelect.Path.Add(new PathPoint()
                {
                    Delay = 0,
                    DirectMove = false,
                    HideMove = false,
                    Pos = grid.transform.position,
                });
            else
            {
                NowSelect.Path.Insert(index + 1, new PathPoint()
                {
                    Delay = 0,
                    DirectMove = false,
                    HideMove = false,
                    Pos = grid.transform.position,
                });
            }
            m_select.selectedIndex = 0;
            FreshPoints();
        }
    }
}
