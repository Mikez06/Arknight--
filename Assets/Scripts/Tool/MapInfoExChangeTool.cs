using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapInfoExChangeTool : MonoBehaviour
{
    //async void Start()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        var a = await change(i);
    //        SaveHelper.Save(JsonHelper.ToJson(a), "/Map/" + Database.Instance.Get<MapData>(i).Id + ".map");
    //        //Debug.Log(JsonHelper.ToJson(a));
    //    }
    //}

    //async Task<MapInfo> change(int id)
    //{
    //    MapInfo result = new MapInfo();
    //    var mapdata = Database.Instance.Get<MapData>(id);
    //    result.MapName = mapdata.MapName;
    //    result.Description = mapdata.Description;
    //    result.Scene = mapdata.Scene;
    //    result.InitHp = mapdata.InitHp;
    //    result.InitCost = mapdata.InitCost;
    //    result.MaxBuildCount = mapdata.MaxBuildCount;
    //    result.MaxCost = mapdata.MaxCost;
    //    //result.MapModel = mapdata.MapModel;
    //    if (mapdata.Contracts != null)
    //        result.Contracts = mapdata.Contracts.Select(x => Database.Instance.Get<ContractData>(x).Id).ToList();

    //    SceneManager.LoadScene(mapdata.Scene);
    //    await Task.Delay(1000);

    //    var grids = GameObject.FindObjectsOfType<MapGrid>();
    //    result.GridInfos = new GridInfo[grids.Max(x => x.X) + 1, grids.Max(x => x.Y) + 1];
    //    foreach (var g in grids)
    //    {
    //        var gInfo = new GridInfo()
    //        {
    //            X = g.X,
    //            Y = g.Y,
    //            CanBuildUnit = g.CanBuildUnit,
    //            FarAttack = g.FarAttackGrid,
    //            CanMove = g.CanMove,
    //        };
    //        result.GridInfos[g.X, g.Y] = gInfo;
    //    }

    //    result.UnitInfos = new List<UnitInfo>();
    //    foreach (var g in grids)
    //    {
    //        if (!string.IsNullOrEmpty(g.MapUnitId))
    //        {
    //            var uInfo = new UnitInfo()
    //            {
    //                X = g.X,
    //                Y = g.Y,
    //                ActiveTime = g.ActiveTime,
    //                Tag = g.tag,
    //                UnitId = g.MapUnitId,
    //            };
    //            result.UnitInfos.Add(uInfo);
    //        }
    //        var exUnits = g.GetComponents<MapExUnit>();
    //        if (exUnits != null)
    //            foreach (var u in exUnits)
    //            {
    //                var uInfo = new UnitInfo()
    //                {
    //                    ActiveTime = u.ActiveTime,
    //                    UnitId = u.UnitId,
    //                    Tag = u.Tag,
    //                    X = g.X,
    //                    Y = g.Y,
    //                    Direction = g.transform.forward.ToV2(),
    //                };
    //                result.UnitInfos.Add(uInfo);
    //            }
    //    }

    //    result.PathInfos = new List<PathInfo>();

    //    var paths = GameObject.FindObjectsOfType<OnePath>();
    //    foreach (var p in paths)
    //    {
    //        var pInfo = new PathInfo()
    //        {
    //            Name = p.name,
    //            Path = p.Path,
    //        };
    //        result.PathInfos.Add(pInfo);
    //    }

    //    result.WaveInfos = new List<WaveInfo>();
    //    foreach (var waveInfo in Database.Instance.GetAll<WaveData>())
    //    {
    //        if (waveInfo.Map == mapdata.Id)
    //            result.WaveInfos.Add(new WaveInfo()
    //            {
    //                UnitId = waveInfo.UnitId,
    //                Delay = waveInfo.Delay,
    //                GapTime = waveInfo.GapTime,
    //                Count = waveInfo.Count,
    //                Path = waveInfo.Path,
    //                OffsetX = waveInfo.OffsetX,
    //                OffetsetY = waveInfo.OffetsetY,
    //            });
    //    }
    //    //await SceneManager.UnloadSceneAsync(mapdata.Scene);
    //    return result;
    //}
}
