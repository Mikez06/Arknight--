using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class BulletConfig : IConfig
{
    public string _Id { get; set; }
    public string Type;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float Speed;
    public string Model;
}

