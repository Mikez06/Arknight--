using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 线性剧毒 : Buff
    {
        public CountDown gap = new CountDown();
        public int triggerCount;
        public float maxTime;
        public float startDamageRate;
        public float maxDamageRate;
        public float gapTime;
        public int DamageBase;

        public override void Init()
        {
            base.Init();
            startDamageRate = BuffData.Data.GetFloat("StartDamageRate");
            maxDamageRate = BuffData.Data.GetFloat("MaxDamageRate");
            maxTime = BuffData.Data.GetFloat("MaxTime");
            gapTime = BuffData.Data.GetFloat("TriggerGap");
            DamageBase = BuffData.Data.GetInt("DamageBase");
        }

        public override void Update()
        {
            gap.Update(SystemConfig.DeltaTime);
            if (gap.Finished())
            {
                triggerCount++;
                float damageBase = 0;
                switch (BuffData.Data.GetInt("DamageBase"))
                {
                    case 0:
                        damageBase = Skill.SkillData.GetBuffData(Index)[0];
                        break;
                    case 1:
                        damageBase = Skill.SkillData.GetBuffData(Index)[0] * Skill.Unit.Attack;
                        break;
                    case 2:
                        damageBase = Skill.SkillData.GetBuffData(Index)[0] * Unit.MaxHp;
                        break;
                }
                var c = triggerCount / maxTime * gapTime;
                if (c > 1) c = 1;
                float damage = damageBase * ((maxDamageRate - startDamageRate) * c + startDamageRate);
                //Log.Debug($"第{triggerCount} 跳伤害为{damage}");
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
