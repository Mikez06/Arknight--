using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

public static class PosHelper
{
    public static Vector2 ScreenToUI(this Vector2 self)
    {
        return new Vector2(self.x * GRoot.inst.width / Screen.width, self.y * GRoot.inst.height / Screen.height);
    }

    public static Vector2 WorldToUI(this Vector3 self)
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(self); //Stage.inst.touchPosition.ScreenToUI();
        pos.y = Screen.height - pos.y;
        return ScreenToUI(pos);
    }
    public static Vector2 ToV2(this Vector3 self)
    {
        return new Vector2(self.x, self.z);
    }
    public static Vector3 ToV3(this Vector2 self)
    {
        return new Vector3(self.x, 0, self.y);
    }
}

