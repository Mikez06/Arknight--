using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PathManager : MonoBehaviour
{

}

[System.Serializable]
public class PathPoint
{
    public Vector3 Pos;
    public float Delay;
    public bool DirectMove;
    public bool HideMove;
}
