using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 治愈 : Buff
    {
        float lowHpCount;
        float lowHpRate;
        float damage;
        float triggerTime;
        CountDown Trigger = new CountDown();
        public override void Init()
        {
            base.Init();
            switch (BuffData.Data.GetInt("DamageBase"))
            {
                case 0:
                    damage = Skill.SkillData.GetBuffData(Index)[0];
                    break;
                case 1:
                    damage = Skill.SkillData.GetBuffData(Index)[0] * Skill.Unit.Attack;
                    break;
                case 2:
                    damage = Skill.SkillData.GetBuffData(Index)[0] * Unit.MaxHp;
                    break;
            }
            triggerTime = BuffData.Data.GetFloat("TriggerTime");
            lowHpCount = BuffData.Data.GetFloat("HpLess");
            lowHpRate = BuffData.Data.GetFloat("HpLessRate");
            if (triggerTime < SystemConfig.DeltaTime) triggerTime = SystemConfig.DeltaTime;
            Trigger.Set(triggerTime);
        }

        public override void Update()
        {
            base.Update();
            if (Trigger.Update(SystemConfig.DeltaTime))
            {
                float d = damage;
                if (Unit.Hp / Unit.MaxHp < lowHpCount) d *= lowHpRate;
                Unit.Heal(new DamageInfo()
                {
                    Attack = d * triggerTime,
                    Target = Unit,
                    Source = this,
                }, false);
                Trigger.Set(triggerTime);
            }
        }
    }
}
