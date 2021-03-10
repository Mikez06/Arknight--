using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card
{
    public UnitConfig Config => Database.Instance.Get<UnitConfig>(UnitId);
    public int UnitId;
    public int Level;

    public int UsingSkill;
}
