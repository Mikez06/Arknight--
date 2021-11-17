using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card : ICard
{
    public UnitData UnitData => Database.Instance.Get<UnitData>(UnitId);
    public int UnitId { get; set; }
    public int Upgrade { get; set; }
    public int Level { get; set; }

    public int DefaultUsingSkill;
}

public interface ICard
{
    public int UnitId { get; }
    public int Upgrade { get; }
    public int Level { get; }
}