using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class OneWave
{
    public WaveData WaveData => Database.Instance.Get<WaveData>(WaveId);
    public int WaveId;
    public float Time;
}

