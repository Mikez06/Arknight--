using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs

{
    public class 中毒 : Buff
    {
        float farAttackRate = 1;
        float damage;
        float triggerTime;
        CountDown Trigger = new CountDown();
        DamageTypeEnum DamageType;
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
            if (triggerTime < SystemConfig.DeltaTime) triggerTime = SystemConfig.DeltaTime;
            Trigger.Set(triggerTime);
            if (Unit.FirstSkill != null && Unit.FirstSkill.SkillData.AttackRange > 0)
            {
                farAttackRate = BuffData.Data.GetFloat("FarAttackUnitRate", 1);
            }
            DamageType = (DamageTypeEnum)Enum.Parse(typeof(DamageTypeEnum), BuffData.Data.GetStr("DamageType"));
        }

        public override void Update()
        {
            base.Update();
            if (Trigger.Update(SystemConfig.DeltaTime))
            {
                Unit.Damage(new DamageInfo()
                {
                    Attack = damage * farAttackRate * triggerTime,
                    DamageType = DamageType,
                    Target = Unit,
                    Source = this,
                });
                Trigger.Set(triggerTime);
            }
        }
    }
}
