using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class m3融毁加攻 : Buff
    {

        public override void Init()
        {
            base.Init();
            Unit.BreakAllCast();
        }

        public override void Apply()
        {
            var p = (Unit as Units.干员).Parent;
            Unit.AttackRate += Skill.SkillData.GetBuffData(Index)[0] * p.MainSkill.Opening.value / p.MainSkill.SkillData.OpenTime;
        }
    }
}
