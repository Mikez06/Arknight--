using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitConfig : IConfig
{
    public string _Id { get; set; }
    public string Type;
    public string Name;
    public string Model;
    public int Team;
    public float Hp;
    public int Damage;
    public float Radius;
    public int[] Skills;
    public float Height;
    public bool CanStop;
}
