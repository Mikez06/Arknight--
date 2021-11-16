using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地牢模式下相机行为控制，写的很随便
/// TODO:拖拽时相机会震动
/// </summary>
public class DungeonCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = DungeonManager.Instance.DungeonUnit;
        var dungeon = DungeonManager.Instance.Dungeon;
        if (player == null) return;

        var _mainCamera = Camera.main;

        var pPos = _mainCamera.WorldToScreenPoint(player.transform.position);
        var p = new Vector2(pPos.x / Screen.width, pPos.y / Screen.height);

        float _mouseX = Input.GetAxis("Mouse X") * 0.1f;
        float _mouseY = Input.GetAxis("Mouse Y") * 0.2f;
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //相机位置的偏移量（Vector3类型，实现原理是：向量的加法）
            moveDir = (_mouseX * -_mainCamera.transform.right + _mouseY * -_mainCamera.transform.forward);

            //限制y轴的偏移量
            moveDir.y = 0;
        }

        float speed = 0.015f;
        //主角必须在屏幕中心范围内
        if (p.x < 0.2f) moveDir.x = -speed;
        if (p.x > 0.8f) moveDir.x = speed;
        if (p.y < 0.2f) moveDir.z = -speed;
        if (p.y > 0.8f ) moveDir.z = speed;
        //相机不能超出地牢太多
        if (transform.position.x < 1.2f) moveDir.x = speed;
        if (transform.position.x > dungeon.Tiles.GetLength(0) - 1.2f ) moveDir.x = -speed;
        if (transform.position.z < -2f ) moveDir.z = speed;
        if (transform.position.z < -2 + dungeon.Tiles.GetLength(1) * 0.25f) moveDir.z = speed;

        _mainCamera.transform.position += moveDir;
    }
}
