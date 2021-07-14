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

    public Units.干员 Unit;
    /// <summary>
    /// 广搜临时数据
    /// </summary>
    public MapGrid PreGrid;

    Renderer Renderer;

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

    public bool CanSet(Unit unit)
    {
        if (this.Unit != null) return false;
        if (CanBuildUnit)
            if (FarAttackGrid)
            {
                return unit.Config.CanSetHigh;
            }
            else
                return unit.Config.CanSetGround;
        return false;
    }

    public void ChangeHighLight(bool bo)
    {
        Renderer.material.color = bo ? new Color(0.458f, 1, 0.42f) : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Unit != null && !FairyGUI.Stage.isTouchOnUI)
            BattleUI.UI_Battle.Instance.ChooseUnit(Unit);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
