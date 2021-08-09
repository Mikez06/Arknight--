using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skills
{
    public class 持续获得费用 : Skill
    {
        float cost;
        public override void Update()
        {
            base.Update();
        }

        public override void Hit(Unit target)
        {
            base.Hit(target);
            cost += SkillData.CostCount / SkillData.OpenTime * SystemConfig.DeltaTime;
            if (cost > 1)
            {
                var c = Mathf.Floor(cost);
                cost -= c;
                Battle.Cost += c;
            }
        }

        protected override void OnOpenEnd()
        {
            base.OnOpenEnd();
            if (cost != 0)
            {
                cost = 0;
                Battle.Cost += 1;
            }
        }
    }
}
