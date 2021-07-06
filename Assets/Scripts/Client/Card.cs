using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card
{
    public UnitData Config => Database.Instance.Get<UnitData>(UnitId);
    public int UnitId;
    public int Level;

    public int UsingSkill;
}
