using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WaveConfig : IConfig
{
    public string _Id { get; set; }
    public string Map;
    public int UnitId;
    public float Delay;
    public Vector2Int[] Path;
    public float[] PathWait;
}
