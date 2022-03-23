using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 回复技力1 : Skill
    {
        public override void Hit(Unit target, Bullet bullet = null)
        {
            float count0 = SkillData.Data.GetFloat("PowerCount");
            foreach (var skill in target.Skills)
            {
                skill.RecoverPower(count0 * skill.MaxPower, !SkillData.Data.GetBool("IgnoreTip"), SkillData.Data.GetBool("IgnorePrevent"));
            }
            base.Hit(target);
        }
    }
}
