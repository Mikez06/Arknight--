using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PullLineModel : MonoBehaviour
{
    LineRenderer[] ls;

    private void Awake()
    {
        ls = GetComponentsInChildren<LineRenderer>();
    }

    public void SetStart(Vector3 startPos)
    {
        foreach (var l in ls)
        {
            l.SetPosition(0, startPos);
        }
    }

    public void SetEnd(Vector3 endPos)
    {
        foreach (var l in ls)
        {
            l.SetPosition(1, endPos);
        }
    }
}

