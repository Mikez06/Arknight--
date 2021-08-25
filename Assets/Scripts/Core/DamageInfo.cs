using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageInfo
{
    public object Source;
    public float Attack;
    public DamageTypeEnum DamageType;
    public float DamageRate = 1;
    public float FinalDamage;
    public float DefIgnore;
    public bool Avoid;
    public List<int> BuffIds = new List<int>();
    public List<Dictionary<string, object>> BuffDatas = new List<Dictionary<string, object>>();
}
