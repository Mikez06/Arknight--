using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 修改回费速度 : 非指向技能
    {
        public override void Effect(Unit target)
        {
            Battle.CostCountSpeed *= SkillData.Data.GetFloat("CostSpeed");
            base.Effect(target);
        }
    }
}
