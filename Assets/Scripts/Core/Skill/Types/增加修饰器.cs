using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 增加修饰器:Skill
    {
        public override void Effect(Unit target)
        {
            foreach (int modifyId in SkillData.ModifyDatas)
            {
                foreach (string skillId in SkillData.Data.GetArray("TargetSkill"))
                {
                    var sk = target.Skills.FirstOrDefault(x => x.SkillData.Id == skillId);
                    if (sk != null)
                    {
                        var m = ModifyManager.Instance.Get(modifyId, sk);
                        sk.Modifies.Add(m);
                    }
                }
            }
            //if (SkillData.ModifyDatas != null)
            //{
            //    foreach (var modifyId in SkillData.ModifyDatas)
            //    {
            //        var m = ModifyManager.Instance.Get(modifyId, this);
            //        foreach (var skill in target.Skills)
            //        {
            //            skill.Modifies.Add(m);
            //        }
            //    }
            //}
            base.Effect(target);
        }
    }
}
