using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 近战 : Skill
    {
        public override void FindTarget()
        {
            Targets.Clear();
            if (Unit is Units.干员 u)
            {
                Targets.AddRange(u.StopUnits);
                if (Targets.Count == 0) Targets.AddRange(getAttackTarget());
            }
            else if (Unit is Units.敌人 u1)
            {
                if (u1.StopUnit != null)
                    Targets.Add(u1.StopUnit);
            }
        }
    }
}
