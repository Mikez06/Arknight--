using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillConfig : IConfig
{
    public string _Id { get; set; }
    public string Type;
    public AttackTargetEnum AttackTarget;
    public Vector2Int[] AttackPoints;
    public float AttackRange;
    public float Cooldown;
}
