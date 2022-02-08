using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutoBuild()
    {
        var mapRoot = GameObject.Find("S_playground");
        for (int i = 0; i < mapRoot.transform.childCount; i++)
        {
            var t = mapRoot.transform.GetChild(i);
            if (t == mapRoot) continue;
            var gr = t.GetComponent<MapGrid>();
            if (gr == null) gr = t.gameObject.AddComponent<MapGrid>();
            gr.X = (int)(-t.transform.localPosition.z / 100f);
            gr.Y = (int)(t.transform.localPosition.x / 100f);
            gr.FarAttackGrid = gr.transform.localPosition.y != 0;
            gr.CanMove = !gr.FarAttackGrid;
            //Transform t1 = null;
            //if (t.childCount == 0)
            //{
            //    var g = new GameObject("tile" + i);
            //    g.transform.parent = transform;
            //    t.transform.parent = g.transform;
            //    var gr = t.GetComponent<MapGrid>();
            //    if (gr != null) DestroyImmediate(gr);
            //    t1 = t;
            //    t = g.transform;
            //    t.transform.position = t1.transform.position+new Vector3(0,0.5f,0);
            //    t1.transform.localPosition = new Vector3(0, -0.5f, 0);
            //}
            //else
            //{
            //    t1 = t.GetChild(0).transform;
            //}
            //var grid = t.GetComponent<MapGrid>();
            //if (grid == null) grid = t.gameObject.AddComponent<MapGrid>();
            //grid.AutoBuild();
            //grid.X = Mathf.RoundToInt( t.transform.position.x);
            //grid.Y = Mathf.RoundToInt(t.transform.position.z);
            //t.name = "Grid:" + grid.X + "," + grid.Y + "," + grid.MapUnitId;
            //t.position = new Vector3(grid.X, t.transform.position.y, grid.Y);
            //var texName = t1.GetComponent<Renderer>().sharedMaterial.mainTexture.name;
            ////根据贴图名自动匹配
            //switch (texName)
            //{
            //    case "caution_000":
            //        grid.CanBuildUnit = false;
            //        break;
            //    case "stone_000":
            //        //grid.
            //        break;
            //    case "stone_002":
            //        grid.FarAttackGrid = true;
            //        break;
            //    default:
            //        Debug.Log($"未自动设置的贴图名:{texName}");
            //        break;
            //}
        }
        Debug.Log("自动设置地图信息完成");
    }
}
