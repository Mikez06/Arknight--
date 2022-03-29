using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 修改Tag : 非指向技能
    {
        public override void Effect(Unit target)
        {
            Battle.ChangeWaveTag(SkillData.Data.GetStr("Tag"));
            base.Effect(target);
        }
    }
}
