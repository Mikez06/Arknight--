using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 事件目标替换:Skill
    {
        public override void FindTarget()
        {
            GetAttackTarget();
            var t = Battle.TriggerDatas.Peek().Target;
            if (t != null && CanUseTo(t) && tempTargets.Contains(t))
            {
                Targets.Add(t);
            }
        }
    }
}
