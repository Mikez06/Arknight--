using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 额外伤害加 : Modify, IDamageModify
    {
        public float Rate;
        public override void Init()
        {
            base.Init();
            Rate = ModifyData.Data.GetFloat("Rate");
        }

        public void Modify(DamageInfo damageInfo)
        {
            damageInfo.DamageRate += Rate;
        }
    }
}
