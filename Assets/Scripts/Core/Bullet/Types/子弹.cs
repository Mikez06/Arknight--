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
        Vector3 targetPos;
        public override void Update()
        {
            if (Target.Alive())
                targetPos = Target.UnitModel.SkeletonAnimation.transform.position + new Vector3(Target.Config.HitPointX * (Target.Direction.x > 0 ? 1 : -1), Target.Config.HitPointY, 0);
            Vector3 delta = targetPos - Postion;
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
