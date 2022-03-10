using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 数值变化衰减 : 数值变化
    {
        protected override float GetValue(int i)
        {
            return base.GetValue(i) * Duration.value / Skill.SkillData.BuffLastTime.Value;
        }

    }
}
