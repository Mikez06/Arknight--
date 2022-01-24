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
            int count0 = SkillData.Data.GetInt("PowerCount");
            int count1 = SkillData.Data.GetInt("PowerCount2");
            int power = count0 < count1 ? Battle.Random.Next(count0, count1) : count0;
            foreach (var skill in target.Skills)
            {
                skill.RecoverPower(power, !SkillData.Data.GetBool("IgnoreTip"), SkillData.Data.GetBool("IgnorePrevent"));
            }
            base.Hit(target);
        }
    }
}
