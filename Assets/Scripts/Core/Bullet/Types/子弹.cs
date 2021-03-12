using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bullets
{
    public class 子弹 : Bullet
    {
        public override void Update()
        {
            if (Target.Alive())
                TargetPos = Target.UnitModel.SkeletonAnimation.transform.position + new Vector3(Target.Config.HitPointX * (Target.Direction.x > 0 ? 1 : -1), Target.Config.HitPointY, 0);
            Vector3 delta = TargetPos - Postion;
            if (delta.magnitude < Config.Speed * SystemConfig.DeltaTime)
            {
                Finish();
                if (Target.Alive())
                    Skill.Hit(Target);
            }
            else
            {
                Postion += delta.normalized * Config.Speed * SystemConfig.DeltaTime;
            }
        }
    }
}
