using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 闪避 : Buff,IDamageModify
    {
        public float Chance;
        public DamageTypeEnum AvoidType;

        public override void Init()
        {
            base.Init();
            Chance = BuffData.Data.GetFloat("Chance");
            AvoidType = (DamageTypeEnum)Enum.Parse(typeof(DamageTypeEnum), BuffData.Data.GetStr("AvoidType"));
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (damageInfo.DamageType == AvoidType && Battle.Random.NextDouble() < Chance)
            {
                damageInfo.Avoid = true;
            }
        }
    }
}
