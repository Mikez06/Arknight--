using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapBuilderUI
{
    partial class UI_MapBuilder : IGameUIView
    {
        public static UI_MapBuilder Instance;
        public MapInfo MapInfo;
        public string scene;

        partial void Init()
        {
            Instance = this;
            m_exit.onClick.Add(() => { UIManager.Instance.ChangeView<MainUI.UI_Main>(MainUI.UI_Main.URL); MapManager.Instance?.ShowPath(null); if (m_state.selectedIndex != 0) if (!string.IsNullOrEmpty(scene)) SceneManager.UnloadSceneAsync(scene); else SceneManager.UnloadSceneAsync("MapBuilder");m_MidPage.Despawn(); });
            m_back.onClick.Add(goMain);
            m_DoPath.onClick.Add(() => { m_state.selectedIndex = 3; AstarPath.active.Scan(); m_PathPage.Fresh(); m_PathPage.FreshPoints(); });
            m_DoUnit.onClick.Add(() => { m_state.selectedIndex = 4; AstarPath.active.Scan(); m_WavePage.Fresh(); });
            m_DoMidUnit.onClick.Add(() => { m_state.selectedIndex = 5; m_MidPage.Fresh(); });

            m_upgrid.onClick.Add(() => { m_state.selectedIndex = 6; MapManager.Instance.Brush(upper); });
            m_downgrid.onClick.Add(() => { m_state.selectedIndex = 6; MapManager.Instance.Brush(lower); });
            m_moveable.onClick.Add(() => { m_state.selectedIndex = 6; MapManager.Instance.Brush(canBuild); });
            m_unmoveable.onClick.Add(() => { m_state.selectedIndex = 6; MapManager.Instance.Brush(cantBuild); });

            m_X.onChanged.Add(setCamera);
            m_Y.onChanged.Add(setCamera);
            m_Z.onChanged.Add(setCamera);

            m_StartPage.m_load.onClick.Add(() =>
            {
                var info = Database.Instance.GetMap(m_StartPage.m_FileName.text);
                if (info != null)
                {
                    m_MidPage.Despawn();
                    MapInfo = info;
                    if (info.PathInfos == null) info.PathInfos = new List<PathInfo>();
                    if (info.UnitInfos == null) info.UnitInfos = new List<UnitInfo>();
                    if (info.WaveInfos == null) info.WaveInfos = new List<WaveInfo>();
                    else
                    {
                        foreach (var waveInfo in info.WaveInfos)
                        {
                            if (waveInfo.UnitId == null || waveInfo.sUnitId != null) continue;
                            waveInfo.sUnitId = Database.Instance.Get<UnitData>(waveInfo.UnitId.Value).Id;
                        }
                    }
                    if (info.UnitOvDatas == null) info.UnitOvDatas = new List<OverwriteUnitInfo>();
                    if (info.Contracts == null) info.Contracts = new List<string>();

                    m_StartPage.m_SceneName.text = info.Scene;
                    m_StartPage.m_width.text = info.GridInfos.GetLength(0).ToString();
                    m_StartPage.m_height.text = info.GridInfos.GetLength(1).ToString();
                    m_StartPage.Fresh();
                }
            });
            m_StartPage.m_next.onClick.Add(async () =>
            {
                scene = m_StartPage.m_SceneName.text;
                if (string.IsNullOrEmpty(scene))
                {
                    if (MapInfo.GridInfos == null)
                    {
                        MapInfo.GridInfos = new GridInfo[int.Parse(m_StartPage.m_width.text), int.Parse(m_StartPage.m_height.text)];
                        MapInfo.CameraPos = new Vector3((MapInfo.GridInfos.GetLength(0) - 1) / 2f, 0.7f * MapInfo.GridInfos.GetLength(0), -4.5f + (MapInfo.GridInfos.GetLength(1) - 1) / 3f);

                        for (int i = 0; i < MapInfo.GridInfos.GetLength(0); i++)
                        {
                            for (int j = 0; j < MapInfo.GridInfos.GetLength(1); j++)
                            {
                                MapInfo.GridInfos[i, j] = new GridInfo() { X = i, Y = j, CanBuildUnit = true, CanMove = true, FarAttack = false };
                            }
                        }
                    }
                    await SceneManager.LoadSceneAsync("MapBuilder", LoadSceneMode.Additive);
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapBuilder"));
                    changeCamera();
                    MapManager.Instance.Build(MapInfo.GridInfos);
                    goMain();
                }
                else
                {
                    await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                    goMain();
                }
                m_MidPage.UpdatePoints();
            });
            m_yes.onClick.Add(() => ovTcs.SetResult(true));
            m_no.onClick.Add(() => ovTcs.SetResult(false));

            m_save.onClick.Add(async () =>
            {
                check();
                if (SaveHelper.ExistFile(m_StartPage.m_FileName.text))
                {
                    var result = await ShowCancelWindow();
                    if (!result) return;
                }
                SaveHelper.Save(JsonHelper.ToJson(MapInfo), "/Map/" + m_StartPage.m_FileName.text + ".map");
                Database.Instance.Maps.Remove(m_StartPage.m_FileName.text);
                Database.Instance.Maps.Add(m_StartPage.m_FileName.text, MapInfo);
                m_saveSuccess.Play();
            });
            m_StartPage.m_quickLoad.onChanged.Add(() =>
            {
                m_StartPage.m_FileName.text = m_StartPage.m_quickLoad.text;
                m_StartPage.m_load.onClick.Call();
            });
        }

        TaskCompletionSource<bool> ovTcs;
        async Task<bool> ShowCancelWindow()
        {
            m_sure2.selectedIndex = 1;
            m_mapName2.text = m_StartPage.m_FileName.text;
            ovTcs = new TaskCompletionSource<bool>();
            var result = await ovTcs.Task;
            m_sure2.selectedIndex = 0;
            return result;
        }

        void changeCamera()
        {
            Camera.main.transform.position = MapInfo.CameraPos;
            m_X.text = MapInfo.CameraPos.x.ToString();
            m_Y.text = MapInfo.CameraPos.y.ToString();
            m_Z.text = MapInfo.CameraPos.z.ToString();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (m_state.selectedIndex == 2)
            {
                Vector3 v3 = new Vector3();
                v3.y = -Input.GetAxis("Mouse ScrollWheel") * 50;
                if (Input.GetKey(KeyCode.Mouse2))
                {
                    v3.x = -Input.GetAxis("Mouse X") * 5;
                    v3.z = -Input.GetAxis("Mouse Y") * 5;
                }
                MapInfo.CameraPos += v3 * Time.deltaTime;
                changeCamera();
            }
        }

        public void Enter()
        {
            scene = "";
            MapInfo = new MapInfo()
            {
                Contracts = new List<string>(),
                Description = "挑战xxxx",
                MapName = "地图名",
                InitCost = 10,
                InitHp = 3,
                MaxBuildCount = 9,
                MaxCost = 99,
                PathInfos = new List<PathInfo>(),
                UnitInfos = new List<UnitInfo>(),
                WaveInfos = new List<WaveInfo>(),
                UnitOvDatas = new List<OverwriteUnitInfo>(),
            };
            m_state.selectedIndex = 0;
            m_StartPage.Fresh();

            m_StartPage.m_quickLoad.items = m_StartPage.m_quickLoad.values = Database.Instance.GetMaps().ToArray();
        }

        void check()
        {
            foreach (var waveInfo in MapInfo.WaveInfos)
            {
                if (!MapInfo.PathInfos.Any(x=>x.Name== waveInfo.Path))
                {
                    waveInfo.Path = MapInfo.PathInfos[0].Name;
                }
            }
        }

        void goMain()
        {
            if (m_state.selectedIndex == 6) MapManager.Instance.EndBrush();
            m_PathPage.NowPoints = null;
            m_PathPage.NowSelect = null;
            if (string.IsNullOrEmpty(scene))
            {
                m_state.selectedIndex = 2;
            }
            else
            {
                m_state.selectedIndex = 1;
            }
        }

        void upper(MapGrid mapGrid)
        {
            if (!mapGrid.FarAttackGrid)
            {
                mapGrid.FarAttackGrid = true;
                MapInfo.GridInfos[mapGrid.X, mapGrid.Y].FarAttack = true;
                mapGrid.AutoBuild();
            }
        }
        void lower(MapGrid mapGrid)
        {
            if (mapGrid.FarAttackGrid)
            {
                mapGrid.FarAttackGrid = false;
                MapInfo.GridInfos[mapGrid.X, mapGrid.Y].FarAttack = false;
                mapGrid.AutoBuild();
            }
        }
        void cantBuild(MapGrid mapGrid)
        {
            if (mapGrid.CanBuildUnit)
            {
                mapGrid.CanBuildUnit = false;
                MapInfo.GridInfos[mapGrid.X, mapGrid.Y].CanBuildUnit = false;
                mapGrid.AutoBuild();
            }
        }
        void canBuild(MapGrid mapGrid)
        {
            if (!mapGrid.CanBuildUnit)
            {
                mapGrid.CanBuildUnit = true;
                MapInfo.GridInfos[mapGrid.X, mapGrid.Y].CanBuildUnit = true;
                mapGrid.AutoBuild();
            }
        }

        void setCamera()
        {           
            Camera.main.transform.position = new Vector3(float.Parse(m_X.text), float.Parse(m_Y.text), float.Parse(m_Z.text));
            MapInfo.CameraPos = Camera.main.transform.position;
        }
    }
}
