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
                if (Targets.Count < GetTargetCount())
                {
                    Targets.AddRange(GetAttackTarget());
                    var a = Targets.Distinct().ToList();
                    Targets.Clear();
                    Targets.AddRange(a);
                }
            }
            else if (Unit is Units.敌人 u1)
            {
                if (u1.StopUnit != null)
                    Targets.Add(u1.StopUnit);
            }
            orderTargets(Targets);
        }

        protected override void SortTarget(List<Unit> targets)
        {
            targets.RemoveAll(OrderFilter);
            var l = targets.OrderBy(GetOrder).ThenBy(GetSortOrder1).ThenBy((x) => GetSortOrder2(x)).ThenBy(x => x.Hatred()).ToList();
            targets.Clear();
            targets.AddRange(l);
        }

        float GetOrder(Unit unit)
        {
            float result = 0;
            if (this.Unit is Units.干员 u)
            {
                var index = u.StopUnits.IndexOf(unit as Units.敌人);
                if (index < 0) index = int.MaxValue;
                result = index;
            }
            else if (this.Unit is Units.敌人 u1)
            {
                return u1.StopUnit == unit ? 0 : 1;
            }
            return result;
        }
    }
}
