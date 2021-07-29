using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum DirectionEnum
{
    Up,
    Down,
    Left,
    Right,
}

public class DirectionHelper
{
    public static Vector2 DirectionToInt(DirectionEnum direction)
    {
        switch (direction)
        {
            case DirectionEnum.Down:
                return Vector2.down;
            case DirectionEnum.Up:
                return Vector2.up;
            case DirectionEnum.Left:
                return Vector2.left;
            case DirectionEnum.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }
}