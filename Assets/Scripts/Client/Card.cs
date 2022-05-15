using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card : ICard
{
    [JsonIgnore]
    public UnitData UnitData => Database.Instance.Get<UnitData>(UnitId);
    [JsonIgnore]
    public int Id => Database.Instance.GetIndex<UnitData>(UnitId);

    public string UnitId { get; set; }
    public int Upgrade { get; set; }
    public int Level { get; set; }

    public int DefaultUsingSkill;
}

public interface ICard
{
    public string UnitId { get; }
    public int Upgrade { get; }
    public int Level { get; }
}