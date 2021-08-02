using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapGrid : MonoBehaviour, IPointerClickHandler
{
    public int X, Y;
    /// <summary>
    /// 能否造单位
    /// </summary>
    public bool CanBuildUnit;
    /// <summary>
    /// 远程格子
    /// </summary>
    public bool FarAttackGrid;
    /// <summary>
    /// 能否移动
    /// </summary>
    public bool CanMove;

    public TileTypeEnum TileType;

    public int ConfigId;

    Renderer Renderer;
    [HideInInspector]
    public Tile Tile;

    private void Awake()
    {
        Renderer = GetComponentInChildren<Renderer>();
    }

    public void AutoBuild()
    {
        X = 0;
        Y = 0;
        CanBuildUnit = true;
        FarAttackGrid = false;
        CanMove = true;
    }

    public void ChangeHighLight(bool bo)
    {
        Renderer.material.color = bo ? new Color(0.458f, 1, 0.42f) : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Tile.Unit != null && !FairyGUI.Stage.isTouchOnUI)
            BattleUI.UI_Battle.Instance.ChooseUnit(Tile.Unit);
    }
}
