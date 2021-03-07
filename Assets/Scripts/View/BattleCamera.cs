using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public static BattleCamera Instance;
    public bool BuildMode;
    public Units.干员 BuildUnit;
    Pool<MapTile> TilePool = new Pool<MapTile>();
    HashSet<MapTile> Tiles = new HashSet<MapTile>();

    bool rotate;
    float rotateAmount;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResourcesManager.Instance.LoadBundle("Bundles/Other/HighLight");
        //ResourcesManager.Instance.LoadBundle("Bundles/Other/CanBuild");
    }

    // Update is called once per frame
    void Update()
    {
        float rotateSpeed = 200;
        if (rotate && rotateAmount<1)
        {
            float a = Mathf.Clamp(Time.deltaTime * rotateSpeed, 0, 1 - rotateAmount);
            rotateAmount += a;
            transform.Translate(Vector3.left * a);
        }
        if (!rotate && rotateAmount > 0)
        {
            float a = Mathf.Clamp(Time.deltaTime * rotateSpeed, 0, rotateAmount);
            rotateAmount -= a;
            transform.Translate(Vector3.right * a);
        }

        if (BuildMode)
        {
            bool canBuild = false;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var g = BuildUnit.UnitModel.gameObject;
                var grid = hit.collider.GetComponent<MapGrid>();
                if (grid != null && grid.CanSet(BuildUnit))
                {
                    //预览干员位置和攻击范围
                    canBuild = true;
                    g.transform.position = grid.transform.position;
                    BuildUnit.ChangePos(grid.X, grid.Y, DirectionEnum.Left);

                    ShowUnitAttackArea();
                }
                else
                {
                    HideUnitAttackArea();
                    g.transform.position = hit.point - new Vector3(0, 0.5f, 0);
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                BuildMode = false;
                if (canBuild)
                {
                    BattleUI.UI_Battle.Instance.m_state.selectedIndex = 3;
                }
                else
                {
                    BattleUI.UI_Battle.Instance.m_state.selectedIndex = 1;
                }
            }
        }
    }

    public void ShowUnitAttackArea()
    {
        foreach (var go in Tiles)
        {
            TilePool.Despawn(go);
        }
        Tiles.Clear();
        foreach (var tile in BuildUnit.Skills[0].AttackPoints)
        {
            var tileAsset = ResourcesManager.Instance.GetAsset<GameObject>("Bundles/Other/HighLight", "HighLight").GetComponent<MapTile>();
            var go = TilePool.Spawn(tileAsset, Battle.Instance.Map.Grids[tile.x, tile.y].transform.position, null);
            Tiles.Add(go);
        }
    }

    public void HideUnitAttackArea()
    {
        foreach (var go in Tiles)
        {
            TilePool.Despawn(go);
        }
        Tiles.Clear();
    }

    public void StartBuild(Units.干员 unit)
    {
        BuildMode = true;
        BuildUnit = unit;//
        unit.UnitModel.gameObject.SetActive(true);
        rotate = true;

        foreach (var grid in Battle.Instance.Map.Grids)
        {
            if (grid.CanSet(unit))
            {
                grid.ChangeHighLight(true);
            }
        }
    }
    public void EndBuild()
    {
        rotate = false;
        foreach (var grid in Battle.Instance.Map.Grids)
        {
            grid.ChangeHighLight(false);
        }
    }
}
