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
    public UnitConfig UnitConfig => Database.Instance.Get<UnitConfig>(UnitId);

}
