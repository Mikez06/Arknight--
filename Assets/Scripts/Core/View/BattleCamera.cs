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
                    _focusUnit == null ? startPosition : _focusUnit.UnitModel.GetModelPositon() + new Vector3(0, 5, -3),
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
                var tween= DOTween.To(() => _rotate ? 0f : -5f, (x) => transform.rotation = Quaternion.AngleAxis(x, startUp) * startRotation, _rotate ? -5f : 0f, 0.1f);
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
        if (BuildMode && BuildUnit != null)
        {
            bool canBuild = false;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var g = BuildUnit.UnitModel.gameObject;
                var grid = hit.collider.GetComponentInParent<MapGrid>();
                if (grid != null && grid.Tile.CanSet(BuildUnit))
                {
                    //预览干员位置和攻击范围
                    canBuild = true;
                    g.transform.position = grid.GetPos();
                    BuildUnit.ChangePos(grid.X, grid.Y, DirectionEnum.Left);

                    if (grid != lastGrid)
                    {
                        lastGrid = grid;
                        ShowUnitAttackArea();
                    }
                }
                else
                {
                    lastGrid = null;
                    HideUnitAttackArea();
                    //BuildUnit.UnitModel.gameObject.SetActive(false);
                    g.transform.position = hit.point - new Vector3(0, 0.5f, 0);
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                BuildMode = false;
                if (canBuild)
                {
                    BattleUI.UI_Battle.Instance.m_state.selectedIndex = 3;
                    //FocusUnit = BuildUnit
                }
                else
                {
                    CancelBuild();
                    BattleUI.UI_Battle.Instance.m_state.selectedIndex = 1;
                }
            }
        }
    }

    public void ShowUnitAttackArea()
    {
        //Debug.LogWarning("ShowAttackArea");
        foreach (var go in Tiles)
        {
            TilePool.Despawn(go);
        }
        Tiles.Clear();
        var targetUnit = FocusUnit == null ? BuildUnit : FocusUnit;
        bool ifHeal = targetUnit.FirstSkill.SkillData.IfHeal;
        foreach (var tile in targetUnit.GetNowAttackSkill().AttackPoints)
        {
            var grid = BattleManager.Instance.Battle.Map.Tiles[tile.x, tile.y];
            if (grid == null) continue;
            var tileAsset = ResHelper.GetAsset<GameObject>(PathHelper.OtherPath + "HighLight").GetComponent<MapTile>();
            var go = TilePool.Spawn(tileAsset, grid.MapGrid.GetPos(), null);
            go.IfHeal(ifHeal);
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

    public void CancelBuild()
    {
        if (BuildUnit == null || BuildUnit.InputTime > 0) return;
        HideUnitAttackArea();
        BuildUnit.UnitModel.gameObject.SetActive(false);
        BuildUnit = null;
    }

    public void ShowHighLight()
    {
        foreach (var grid in BattleManager.Instance.Battle.Map.Tiles)
        {
            if (grid == null) continue;
            if (grid.CanSet(BuildUnit))
            {
                grid.MapGrid.ChangeHighLight(true);
            }
            else
            {
                grid.MapGrid.ChangeHighLight(false);
            }
        }
    }

    public void HideHighLight()
    {
        foreach (var grid in BattleManager.Instance.Battle.Map.Tiles)
        {
            grid?.MapGrid.ChangeHighLight(false);
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
