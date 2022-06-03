using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 额外目标与伤害:Modify,ITargetModify,IDamageModify
    {
        public float Chance;
        public float Rate;
        public int Count;

        bool success;

        public override void Init()
        {
            base.Init();
            Chance = ModifyData.Data.GetFloat("Chance");
            Count = ModifyData.Data.GetInt("Count");
            Rate = ModifyData.Data.GetFloat("Rate");
        }

        public int Modify(int count)
        {
            if (Battle.Random.NextDouble() < Chance)
            {
                success = true;
                count += Count;
            }
            return count;
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (success)
            {
                damageInfo.DamageRate *= Rate;
                success = false;
            }
        }
    }
}
