using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageInfo
{
    public Unit Target;
    public int AllCount;
    public object Source;
    public float Attack;
    public DamageTypeEnum DamageType;
    public float DamageRate = 1;
    public float FinalDamage;
    public float DefIgnore;
    public float DefIgnoreRate;
    public bool Avoid;
    public bool Block;
    //public List<int> BuffIds = new List<int>();
    //public List<Dictionary<string, object>> BuffDatas = new List<Dictionary<string, object>>();
}
