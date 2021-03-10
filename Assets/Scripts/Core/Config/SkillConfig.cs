using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SkillConfig : IConfig
{
    public string _Id { get; set; }
    public string Type;
    public string Name;
    public SkillReadyEnum ReadyType;
    public AttackTargetEnum AttackTarget;
    public AttackTargetOrderEnum AttackOrder;
    public Vector2Int[] AttackPoints;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float AttackRange;
    public DamageTypeEnum DamageType;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float Cooldown;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float OpenTime;
    public int TargetTeam;
    public bool IfHeal;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float DamageRate;
    public int? Bullet;
    public string ModelAnimation;
    public int StartPower;
    public int MaxPower;
    public int PowerCount;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float? AreaRange;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float AreaDamage;
    public PowerRecoverTypeEnum PowerType;
    public SkillUseTypeEnum UseType;
    public int HitCount;
    public int[] Skills;
    public int[] ExSkills;
    public int[] Buffs;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float[] BuffData;
    public bool AttackFly;
    public string StartEffect;
    public string HitEffect;
    public string EffectEffect;
}
