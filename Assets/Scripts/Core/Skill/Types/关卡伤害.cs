using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 关卡伤害:Skill
    {
        public override void Effect(Unit target)
        {
            Battle.DoDamage(SkillData.Data.GetInt("LevelDamage"));
            base.Effect(target);
        }
    }
}
