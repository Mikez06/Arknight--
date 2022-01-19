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
        public override void Init()
        {
            base.Init();
            damage = Skill.SkillData.BuffData[0];
            if (Unit.FirstSkill.SkillData.AttackRange > 0)
            {
                farAttackRate = BuffData.Data.GetFloat("FarAttackUnitRate", 1);
            }
        }

        public override void Update()
        {
            base.Update();
            Unit.Damage(new DamageInfo()
            {
                Attack = damage * farAttackRate * SystemConfig.DeltaTime,
                DamageType = DamageTypeEnum.Magic,
                Target = Unit,
                Source = this,
            });
        }
    }
}
