using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 对远程伤害 : Modify, IDamageModify
    {
        public float Rate;

        public override void Init()
        {
            base.Init();
            Rate = ModifyData.Data.GetFloat("Rate");
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (damageInfo.Target.FirstSkill.SkillData.AttackRange > 0)
            {
                damageInfo.DamageRate *= Rate;
            }
        }
    }
}
