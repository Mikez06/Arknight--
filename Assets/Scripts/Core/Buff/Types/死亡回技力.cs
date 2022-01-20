using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 死亡回技力 : Skill
    {
        public float RecoverCount;
        public override void Init()
        {
            base.Init();
            RecoverCount = SkillData.Data.GetFloat("RecoverPowerCount", 1f);
        }
        public override void Cast()
        {
            if (Targets.Contains(Battle.TriggerDatas.Peek().Target))
            {
                Unit.MainSkill.RecoverPower(RecoverCount, true, true);
            }
            base.Cast();
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
