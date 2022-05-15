using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skills
{
    public class 强制撤退 : Skill
    {
        public CountDown Dying = new CountDown();
        public override void Init()
        {
            base.Init();
        }

        public override void Effect(Unit target)
        {
            if (Dying.Finished()) Dying.Set(SkillData.Data.GetFloat("Time"));
            base.Effect(target);
        }

        public override void Update()
        {
            if (Unit.State == StateEnum.Die || Unit.State == StateEnum.Default)
            {
                Dying.Finish();
            }
            if (Dying.Update(SystemConfig.DeltaTime))
            {
                Unit.DoDie(this);
                Battle.Cost += Mathf.FloorToInt(Unit.UnitData.Cost * Unit.UnitData.LeaveReturn);
            }
            base.Update();
        }

        public override void Finish()
        {
            base.Finish();
            Dying.Finish();
        }
    }
}
