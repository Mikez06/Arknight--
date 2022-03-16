using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 对飞行伤害:Modify,IDamageModify
    {
        public void Modify(DamageInfo damageInfo)
        {
            if (damageInfo.Target.Height > 0)
            {
                damageInfo.DamageRate *= ModifyData.Data.GetFloat("DamageRate");
            }
            
        }
    }
}
