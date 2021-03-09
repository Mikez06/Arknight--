using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class BuffConfig : IConfig
{
    public string _Id { get; set; }
    public string Type;
    public string Name;
    [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
    public float LastTime;
    public Dictionary<string, object> Data;
}