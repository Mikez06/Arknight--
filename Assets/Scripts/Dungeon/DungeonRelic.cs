using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DungeonRelic
{
    public int Id;
    public RelicData RelicData => Database.Instance.Get<RelicData>(Id);
}