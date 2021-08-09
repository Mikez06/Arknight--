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

        protected override void SortTarget(List<Unit> targets)
        {
            var l = targets.OrderBy(GetOrder).ThenBy(GetSortOrder1).ThenBy((x) => GetSortOrder2(x, targets)).ThenBy(x => x.Hatred()).ToList();
            targets.Clear();
            targets.AddRange(l);
        }

        float GetOrder(Unit Unit)
        {
            float result = 0;
            if (Unit is Units.干员 u)
            {
                var index = u.StopUnits.IndexOf(Unit as Units.敌人);
                if (index < 0) index = int.MaxValue;
                result = index;
            }
            else if (Unit is Units.敌人 u1)
            {
                return u1.StopUnit == Unit ? 0 : 1;
            }
            return result;
        }
    }
}
