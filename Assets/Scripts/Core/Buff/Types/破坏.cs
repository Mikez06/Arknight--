using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 破坏:Buff
    {
        public override void Init()
        {
            base.Init();
            bool success = false;
            foreach (var skill in Unit.Skills)
            {
                if (skill.SkillData.CanDestory)
                {
                    success = true;
                    skill.Destroyed = true;
                }
            }
            if (!success) Finish();
        }

        public override void Finish()
        {
            base.Finish();
            foreach (var skill in Unit.Skills)
            {
                if (skill.SkillData.CanDestory)
                {
                    skill.Destroyed = false;
                }
            }
        }
    }
}
