using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class m3融毁加攻 : Buff
    {
        public CountDown c = new CountDown();

        public override void Init()
        {
            base.Init();
            Unit.BreakAllCast();
        }

        public override void Update()
        {
            if (c.Finished()) c.Set(1);
            if (c.Update(SystemConfig.DeltaTime))
            {
                Unit.Refresh();
            }
            base.Update();
        }

        public override void Apply()
        {
            var p = (Unit as Units.干员).Parent;
            Unit.AttackRate += Skill.SkillData.BuffData[0] * p.MainSkill.Opening.value / p.MainSkill.SkillData.OpenTime;
        }
    }
}
