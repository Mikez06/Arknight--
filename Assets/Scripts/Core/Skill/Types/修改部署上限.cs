using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 修改部署上限 : Skill
    {
        public override void Effect(Unit target)
        {
            if (SkillData.Data.ContainsKey("BuildCount"))
            {
                Battle.BuildCount = SkillData.Data.GetInt("BuildCount");
            }
            Battle.BuildCount += SkillData.Data.GetInt("BuildCountAdd");
            base.Effect(target);
        }
    }
}
