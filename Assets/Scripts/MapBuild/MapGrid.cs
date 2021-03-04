using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
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

    /// <summary>
    /// 广搜临时数据
    /// </summary>
    public MapGrid PreGrid;
    public void Reset()
    {
        X = 0;
        Y = 0;
        CanBuildUnit = true;
        FarAttackGrid = false;
        CanMove = true;
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
