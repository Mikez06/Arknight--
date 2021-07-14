using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleCamera : MonoBehaviour
{
    public static BattleCamera Instance;
    public bool BuildMode;
    public Units.干员 BuildUnit;
    Pool<MapTile> TilePool = new Pool<MapTile>();
    HashSet<MapTile> Tiles = new HashSet<MapTile>();

    Vector3 startPosition;
    Vector3 startUp;
    Quaternion startRotation;

    public bool Blur
    {
        get => blur;
        set
        {
            blur = value;
            GetComponent<ImageEffect_GaussianBlur>().enabled = blur;
        }
    }
    private bool blur;

    public Unit FocusUnit
    {
        get => _focusUnit;
        set
        {
            if (_focusUnit != value)
            {
                _focusUnit = value;
                var tween= DOTween.To(() => transform.position,
                    (x) => transform.position = x,
                    _focusUnit == null ? startPosition : _focusUnit.UnitModel.SkeletonAnimation.transform.position + new Vector3(0, 5, -3),
                    0.1f);
                tween.SetUpdate(true);
                if (_focusUnit != null) 
                    ShowUnitAttackArea();
                else HideUnitAttackArea();
            }
        }
    }

    private Unit _focusUnit;

    public bool Rotate
    {
        get => _rotate;
        set
        {
            if (_rotate != value)
            {
                _rotate = value;
                var tween= DOTween.To(() => _rotate ? 0f : 5f, (x) => transform.rotation = Quaternion.AngleAxis(x, startUp) * startRotation, _rotate ? 5f : 0f, 0.1f);
                tween.SetUpdate(true);
            }
        }
    }
    bool _rotate;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResourcesManager.Instance.LoadBundle("Bundles/Other/HighLight");
        startPosition = transform.position;
        startUp = transform.up;
        startRotation = transform.rotation;
        //ResourcesManager.Instance.LoadBundle("Bundles/Other/CanBuild");
    }

    MapGrid lastGrid;
    // Update is called once per frame
    void Update()
    {
        if (BuildMode)
        {
            bool canBuild = false;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var g = BuildUnit.UnitModel.gameObject;
                var grid = hit.collider.GetComponentInParent<MapGrid>();
                if (grid != null && grid.CanSet(BuildUnit))
                {
                    //预览干员位置和攻击范围
                    canBuild = true;
                    g.transform.position = grid.transform.position;
                    BuildUnit.ChangePos(grid.X, grid.Y, DirectionEnum.Left);

                    if (grid != lastGrid)
                    {
                        lastGrid = grid;
                        ShowUnitAttackArea();
                    }
                }
                else
                {
                    grid = null;
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
                    BuildUnit.UnitModel.gameObject.SetActive(false);
                    BattleUI.UI_Battle.Instance.m_state.selectedIndex = 1;
                    BuildUnit = null;
                }
            }
        }
    }

    public void ShowUnitAttackArea()
    {
        Debug.LogWarning("ShowAttackArea");
        foreach (var go in Tiles)
        {
            TilePool.Despawn(go);
        }
        Tiles.Clear();
        var targetUnit = FocusUnit == null ? BuildUnit : FocusUnit;
        foreach (var tile in targetUnit.Skills[0].AttackPoints)
        {
            var tileAsset = ResHelper.GetAsset<GameObject>(PathHelper.OtherPath + "HighLight").GetComponent<MapTile>();
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

    public void StartBuild()
    {
        BuildMode = true;
        BuildUnit.UnitModel.gameObject.SetActive(true);
        ShowHighLight();
    }

    public void ShowHighLight()
    {
        foreach (var grid in Battle.Instance.Map.Grids)
        {
            if (grid.CanSet(BuildUnit))
            {
                grid.ChangeHighLight(true);
            }
        }
    }

    public void HideHighLight()
    {
        foreach (var grid in Battle.Instance.Map.Grids)
        {
            grid.ChangeHighLight(false);
        }
    }
    public void ShowUnitInfo(Unit unit)
    {
        FocusUnit = unit;
        if (unit != null)
        {
            HideHighLight();
        }
    }
}
