using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 无视防御 : Buff, ISelfDamageModify
    {
        public float Chance;
        public float OpenChance;
        public float Value;
        public float Rate;

        public override void Init()
        {
            base.Init();
            Chance = BuffData.Data.GetFloat("Chance");
            OpenChance = BuffData.Data.GetFloat("OpenChance");
            Value = BuffData.Data.GetFloat("Value");
            Rate = BuffData.Data.GetFloat("Rate");
        }

        public void Modify(DamageInfo damageInfo)
        {
            bool success = false;
            if (Unit.MainSkill.Opening.Finished())
            {
                success = Battle.Random.NextDouble() < Chance;
            }
            else
            {
                success = Battle.Random.NextDouble() < OpenChance;
            }
            if (success)
            {
                damageInfo.DefIgnoreRate = Rate;
                damageInfo.DefIgnore = Value;
            }
        }
    }
}
