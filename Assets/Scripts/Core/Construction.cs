using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Construction
{
    public int Cost;
    public int BuildTime;
    public int UnitId;
    public UnitData UnitConfig => Database.Instance.Get<UnitData>(UnitId);

}
