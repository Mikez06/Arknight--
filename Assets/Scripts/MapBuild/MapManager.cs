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
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if (t == this) continue;
            var grid = t.GetComponent<MapGrid>();
            if (grid == null) grid = t.gameObject.AddComponent<MapGrid>();
            grid.Reset();
            grid.X = (int)t.transform.position.x;
            grid.Y = (int)t.transform.position.z;
            var texName = t.GetComponent<Renderer>().sharedMaterial.mainTexture.name;
            //根据贴图名自动匹配
            switch (texName)
            {
                case "caution_000":
                    grid.CanBuildUnit = false;
                    break;
                case "stone_000":
                    //grid.
                    break;
                case "stone_002":
                    grid.FarAttackGrid = true;
                    break;
                default:
                    Debug.Log($"未自动设置的贴图名:{texName}");
                    break;
            }
        }
        Debug.Log("自动设置地图信息完成");
    }
}
