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
        public float Chance;
        public override void Init()
        {
            base.Init();
            Rate = ModifyData.Data.GetFloat("Rate");
            Chance = ModifyData.Data.GetFloat("Chance");
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (Chance == 0 || Battle.Random.NextDouble() < Chance)
                damageInfo.DamageRate += Rate;
        }
    }
}
