using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Pathfinding;
using System;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public MapGrid[,] Grids;

    bool choose;
    bool brush;
    Action<MapGrid> Action;
    TaskCompletionSource<MapGrid> tcs;

    private void Awake()
    {
        Instance = this;
        init();
    }
    void init()
    {
        MapGrid[] grids = GetComponentsInChildren<MapGrid>();
        if (grids != null && grids.Length > 0)
        {
            Grids = new MapGrid[grids.Max(x => x.X) + 1, grids.Max(x => x.Y) + 1];

            foreach (var g in grids)
            {
                Grids[g.X, g.Y] = g;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((choose && Input.GetKeyDown(KeyCode.Mouse0)) || (brush && Input.GetKey(KeyCode.Mouse0) && !FairyGUI.Stage.isTouchOnUI))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var grid = hit.collider.GetComponentInParent<MapGrid>();
                if (grid != null)
                {
                    if (choose)
                        tcs.SetResult(grid);
                    if (brush)
                        Action(grid);
                }
            }
        }
    }

    public async Task<MapGrid> SelectGrid()
    {
        choose = true;
        tcs = new TaskCompletionSource<MapGrid>();
        var result = await tcs.Task;
        choose = false;
        return result;
    }

    public void Brush(Action<MapGrid> action)
    {
        brush = true;
        this.Action = action;
    }

    public void EndBrush()
    {
        brush = false;
        Action = null;
    }


    StartEndModifier startEndModifier = new StartEndModifier()
    {
        exactStartPoint = StartEndModifier.Exactness.ClosestOnNode,
        exactEndPoint = StartEndModifier.Exactness.ClosestOnNode,
    };
    RaycastModifier raycastModifier = new RaycastModifier()
    {
        useGraphRaycasting = true,
        useRaycasting = false,
    };
    LineRenderer Line;
    Pool<Transform> Pool = new Pool<Transform>();
    List<Transform> Sphere = new List<Transform>();
    public void ShowPath(List<PathPoint> points)
    {
        if (Line == null)
        {
            Line = ResHelper.Instantiate("Assets/Bundles/Other/Line").GetComponent<LineRenderer>();
        }

        foreach (var t in Sphere)
        {
            Pool.Despawn(t);
        }
        Sphere.Clear();
        if (points != null)
        {
            foreach (var p in points)
            {
                var t = Pool.Spawn(ResHelper.GetAsset<GameObject>("Assets/Bundles/Other/Sphere").transform, p.Pos + new Vector3(0, 0.3f, 0));
                Sphere.Add(t);
            }
            List<Vector3> r = new List<Vector3>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                PathPoint point = points[i];
                var p = ABPath.Construct(points[i].Pos, points[i + 1].Pos);
                AstarPath.StartPath(p);
                p.BlockUntilCalculated();

                startEndModifier.Apply(p);

                if (p.vectorPath.Count > 0 && (points[i].DirectMove || points[i].HideMove)) raycastModifier.Apply(p);
                r.AddRange(p.vectorPath);
            }

            Line.positionCount = r.Count;
            for (int i1 = 0; i1 < r.Count; i1++)
            {
                r[i1] += new Vector3(0, 0.3f, 0);
            }

            Line.SetPositions(r.ToArray());
        }
        else
        {
            Line.positionCount = 0;
        }
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

    public void Build(GridInfo[,] infos)
    {
        //Camera.main.transform.position = new Vector3((infos.GetLength(0) - 1) / 2f, 0.6f * infos.GetLength(0), -3.5f + (infos.GetLength(1) - 1) / 3f);
        for (int i = 0; i < infos.GetLength(0); i++)
        {
            for (int j = 0; j < infos.GetLength(1); j++)
            {
                var g = infos[i, j];
                if (g == null) continue;
                var mapGrid = new GameObject(i + "," + j).AddComponent<MapGrid>();
                mapGrid.X = i;
                mapGrid.Y = j;
                mapGrid.CanBuildUnit = g.CanBuildUnit;
                mapGrid.CanMove = g.CanMove;
                mapGrid.FarAttackGrid = g.FarAttack;
                mapGrid.transform.parent = transform;
                mapGrid.AutoBuild();
            }
        }
        init();
    }
}
