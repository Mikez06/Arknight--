using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 回复技力 : Skill
    {
        public override void Hit(Unit target)
        {
            foreach (var skill in target.Skills)
            {
                skill.RecoverPower(SkillData.Data.GetFloat("PowerCount"), true);
            }
            base.Hit(target);
        }
    }
}
