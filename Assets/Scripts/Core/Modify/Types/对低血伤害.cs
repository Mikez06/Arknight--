using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 对低血伤害 : Modify, IDamageModify
    {
        public float Rate;

        public override void Init()
        {
            base.Init();
            Rate = ModifyData.Data.GetFloat("Rate");
        }

        public void Modify(DamageInfo damageInfo)
        {
            damageInfo.DamageRate *= 1 + Rate * (damageInfo.Target.MaxHp - damageInfo.Target.Hp) / damageInfo.Target.MaxHp;
        }
    }
}
