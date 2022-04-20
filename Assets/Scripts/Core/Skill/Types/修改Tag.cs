using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 修改Tag : 非指向技能
    {
        int chance;
        public override void Effect(Unit target)
        {
            var tag = SkillData.Data.GetStr("Tag");
            if (string.IsNullOrEmpty(tag))
            {
                chance++;
                tag = chance.ToString();
            }
            Battle.ChangeWaveTag(tag);
            base.Effect(target);
        }
    }
}
