using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 获得费用 : Skill
    {
        public override void Cast()
        {
            Battle.Cost += SkillData.CostCount;
            base.Cast();
        }
    }
}
