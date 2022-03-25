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

    public string MapUnitId;

    public bool Alive;

    public string Tag;
    public float ActiveTime;
    //public TileTypeEnum TileType;

    //public int ConfigId;

    BoxCollider BoxCollider;

    Renderer Renderer;
    [HideInInspector]
    public Tile Tile;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
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
        if (Renderer != null)
            Renderer.material.color = bo ? new Color(0.458f, 1, 0.42f) : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!FairyGUI.Stage.isTouchOnUI)
        {
            if (Tile.Unit != null)
                BattleUI.UI_Battle.Instance.ChooseUnit(Tile.Unit);
            else if (Tile.MidUnit != null)
                BattleUI.UI_Battle.Instance.ChooseUnit(Tile.MidUnit);
        }
    }

    public Vector3 GetPos()
    {
        if (BoxCollider != null)
        {
            //var result = BoxCollider.center + transform.position;
            //result.y += BoxCollider.bounds.size.y / 2;
            return transform.TransformPoint(BoxCollider.center + new Vector3(0, BoxCollider.size.y / 2, 0));
        }
        return transform.position;
    }
}
