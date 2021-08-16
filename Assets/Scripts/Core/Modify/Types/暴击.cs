using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 暴击 : Modify, IDamageModify
    {
        public float Chance,Rate;
        public override void Init()
        {
            base.Init();
            Chance = ModifyData.Data.GetFloat("Chance");
            Rate = ModifyData.Data.GetFloat("Rate");
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (Battle.Random.NextDouble() < Chance)
            {
                damageInfo.DamageRate *= Rate;
            }
        }
    }
}
