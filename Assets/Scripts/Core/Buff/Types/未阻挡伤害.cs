using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 未阻挡伤害 : Buff, IDamageModify
    {
        public float Rate;
        public override void Init()
        {
            base.Init();
            Rate = BuffData.Data.GetFloat("Rate");
        }
        public void Modify(DamageInfo damageInfo)
        {
            if (!Enable()) return;
            if (Unit is Units.干员 u && !u.StopUnits.Contains(damageInfo.Target))
            {
                damageInfo.DamageRate *= Rate;
            }
            if (Unit is Units.敌人 u1 && damageInfo.GetSourceUnit() != u1.StopUnit)
            {
                damageInfo.DamageRate *= Rate;
            }
        }
    }
}
