using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;
    Dictionary<string, List<PathPoint>> dic = new Dictionary<string, List<PathPoint>>();

    private void Awake()
    {
        Instance = this;
        foreach (var onePath in GetComponentsInChildren<OnePath>())
        {
            var l = new List<PathPoint>();
            dic.Add(onePath.name, l);
            int index = 0;
            foreach (Transform tr in onePath.transform)
            {
                l.Add(new PathPoint()
                {
                    Pos = tr.position,
                    Delay = onePath.Delays.Length < index ? onePath.Delays[index] : 0,
                });
                index++;
            }
        }
    }

    public List<PathPoint> GetPath(string pathName)
    {
        return dic[pathName];
    }
}

public class PathPoint
{
    public Vector3 Pos;
    public float Delay;
}
