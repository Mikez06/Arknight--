using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 引爆召唤 : Skill
    {
        public override void Effect(Unit target)
        {
            base.Effect(target);
            foreach (var c in (Unit as Units.干员).Children)
            {
                if (c.InputTime >= 0)
                    c.DoDie(this);
            }
        }

        public override void FindTarget()
        {
            AttackPoints.Clear();
            foreach (var c in (Unit as Units.干员).Children)
            {
                if (c.InputTime >= 0)
                    foreach (var p in SkillData.AttackPoints)
                    {
                        var point = c.PointWithDirection(p);
                        if (point.x < 0 || point.x >= Battle.Map.Tiles.GetLength(0) || point.y < 0 || point.y >= Battle.Map.Tiles.GetLength(1)) continue;
                        AttackPoints.Add(point);
                    }
            }
            base.FindTarget();
        }
    }
}
