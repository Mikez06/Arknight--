using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 持续获得费用 : Skill
    {
        public override void Update()
        {
            base.Update();
            if (Opening.Finished())
            {
                Battle.Cost += SkillData.DamageRate / SkillData.Cooldown * SystemConfig.DeltaTime;
            }
        }
    }
}
