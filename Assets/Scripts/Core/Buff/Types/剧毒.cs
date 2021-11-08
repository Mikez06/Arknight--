using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 剧毒 : Buff
    {
        public CountDown gap = new CountDown();
        public int triggerCount;
        public float maxTime;
        public float maxDamageRate;
        public float gapTime;

        public override void Init()
        {
            base.Init();
            maxDamageRate = BuffData.Data.GetFloat("MaxDamageRate");
            maxTime = BuffData.Data.GetFloat("MaxTime");
            gapTime = BuffData.Data.GetFloat("TriggerGap");
        }

        public override void Update()
        {
            gap.Update(SystemConfig.DeltaTime);
            if (gap.Finished())
            {
                triggerCount++;
                float damage = maxDamageRate * Unit.MaxHp * triggerCount / maxTime * gapTime * gapTime;
                Unit.Damage(new DamageInfo()
                {
                    Attack = damage,
                    DamageType = DamageTypeEnum.Real,
                    Target = Unit,
                    Source = this,
                });
                gap.Set(gapTime);
            }
            base.Update();
        }
    }
}
