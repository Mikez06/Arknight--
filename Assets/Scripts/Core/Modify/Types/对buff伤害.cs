using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 对buff伤害 : Modify, IDamageModify
    {
        public void Modify(DamageInfo damageInfo)
        {
            if (ModifyData.Buff != null)
            {
                if (damageInfo.Target.Buffs.Any(x => x.Id == ModifyData.Buff.Value))
                {
                    damageInfo.DamageRate *= ModifyData.Data.GetFloat("DamageRate");
                }
            }
        }
    }
}
