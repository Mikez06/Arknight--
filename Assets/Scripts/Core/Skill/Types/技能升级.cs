using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 技能升级 : Skill
    {
        int[] start, end;

        public override void Init()
        {
            base.Init();
            var datas = SkillData.Data.GetArray("s0");
            start = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                start[i] = Database.Instance.GetIndex<SkillData>(Convert.ToString(datas[i]));
            }
            datas = SkillData.Data.GetArray("s1");
            end = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                end[i] = Database.Instance.GetIndex<SkillData>(Convert.ToString(datas[i]));
            }
        }

        public override void Effect(Unit target)
        {
            base.Effect(target);
            for (int i = 0; i < start.Length; i++)
            {
                var skill = target.Skills.FirstOrDefault(x => x.Id == start[i]);
                if (skill != null)
                {
                    skill.DoUpgrade(end[i]);
                }
                else
                {
                    Unit.LearnSkill(end[i]);
                }
            }
        }
    }
}
