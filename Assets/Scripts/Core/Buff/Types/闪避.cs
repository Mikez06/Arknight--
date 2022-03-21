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
        public float OpenRate;

        public override void Init()
        {
            base.Init();
            Chance = BuffData.Data.GetFloat("Chance");
            OpenRate = BuffData.Data.GetFloat("OpenRate");
            AvoidType = (DamageTypeEnum)Enum.Parse(typeof(DamageTypeEnum), BuffData.Data.GetStr("AvoidType"));
        }

        public void Modify(DamageInfo damageInfo)
        {
            var Chance = this.Chance;
            if (OpenRate != 0 && !Skill.Unit.MainSkill.Opening.Finished()) Chance *= OpenRate;
            if ((AvoidType == DamageTypeEnum.Real || damageInfo.DamageType == AvoidType) && Battle.Random.NextDouble() < Chance)
            {
                damageInfo.Avoid = true;
            }
        }
    }
}
