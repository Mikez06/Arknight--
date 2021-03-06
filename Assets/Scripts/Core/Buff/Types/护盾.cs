using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 护盾 : Buff, IShield
    {
        public DamageTypeEnum Type;
        public float Count;
        public bool AutoReduce;
        protected float MaxCount;

        public override void Init()
        {
            base.Init();
            Type = (DamageTypeEnum)Enum.Parse(typeof(DamageTypeEnum), BuffData.Data.GetStr("ShieldType"));
            Count = Skill.SkillData.GetBuffData(Index)[0];
            AutoReduce = BuffData.Data.GetBool("AutoReduce");
            switch (BuffData.Data.GetInt("Base"))
            {
                case 0:
                    break;
                case 1:
                    Count = Count * Skill.Unit.Attack;
                    break;
                case 2:
                    Count = Count * Skill.Unit.MaxHp;
                    break;
            }
            MaxCount = Count;
        }

        public override void Update()
        {
            base.Update();
            if (AutoReduce)
            {
                Count -= MaxCount / Skill.SkillData.BuffLastTime.Value * SystemConfig.DeltaTime;
            }
            if (Count == 0) Finish();
        }

        public void Absorb(DamageInfo damageInfo)
        {
            if (Type == DamageTypeEnum.Real || Type == damageInfo.DamageType)
            {
                if (Count > damageInfo.FinalDamage)
                {
                    Count -= damageInfo.FinalDamage;
                    damageInfo.FinalDamage = 0;
                }
                else
                {
                    damageInfo.FinalDamage -= Count;
                    Count = 0;
                }
            }
        }
    }
}
