using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
    public int Hp;
    public int Attack;
    public int Defence;
    public int MagicDefence;
    public int Damage;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float Speed;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float Radius;
    public int[] Skills;
    public int? MainSkill;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float Height;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float HitPointX;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float HitPointY;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float AttackPointX;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float AttackPointY;
    public bool CanStop;
    public bool CanSetHigh;
    public bool CanSetGround;
    public int Cost;
    public int ResetTime;
    public int StopCount;
    public UnitTypeEnum UnitType;
}
