using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 伤害连发:Skill
    {
        //public override void Cast(Unit target)
        //{
        //    Effect(target);
        //    CastExSkill(target);
        //}
        //public override void Hit(Unit target)
        //{
        //    var targets = Battle.FindAll(AttackPoints, Config.TargetTeam, Selectable).ToList();
        //    if (!targets.Contains(target)) targets.Add(target);
        //    for (int i = 0; i < (int)Config.BurstCount; i++)
        //    {
        //        var t = targets[Battle.Random.Next(0, targets.Count)];
        //        t.Damage(new DamageInfo()
        //        {
        //            Source = this,
        //            Attack = Unit.Attack,
        //            DamageRate = Config.DamageRate,
        //            DamageType = Config.DamageType,
        //        });
        //        if (!t.Alive())
        //        {
        //            targets.Remove(t);
        //            if (targets.Count == 0) break;
        //        }
        //    }
        //}
    }
}
