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
            if (SkillData.ModifyDatas != null)
            {
                foreach (var modifyId in SkillData.ModifyDatas)
                {
                    var m = ModifyManager.Instance.Get(modifyId, this);
                    foreach (var skill in target.Skills)
                    {
                        skill.Modifies.Add(m);
                    }
                }
            }
            base.Effect(target);
        }
    }
}
