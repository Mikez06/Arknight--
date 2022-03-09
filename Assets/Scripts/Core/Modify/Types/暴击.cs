using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 暴击 : Modify, IDamageModify
    {
        public float Chance,Rate,Chance1;
        public override void Init()
        {
            base.Init();
            Chance = ModifyData.Data.GetFloat("Chance");
            Chance1 = ModifyData.Data.GetFloat("Chance1");
            Rate = ModifyData.Data.GetFloat("Rate");
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (Chance1 != 0 && Unit.MainSkill != null && !Unit.MainSkill.Opening.Finished() && Battle.Random.NextDouble() < Chance1)
            {
                damageInfo.DamageRate *= Rate;
            }
            else if (Chance >= 1 || Battle.Random.NextDouble() < Chance)
            {
                damageInfo.DamageRate *= Rate;
            }
        }
    }
}
