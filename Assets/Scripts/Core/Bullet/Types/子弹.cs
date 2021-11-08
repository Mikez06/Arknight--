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
        float moveHeight;//0:直线 1:抛物线
        float tickTime;
        public override void Init()
        {
            base.Init(); 
            if (Target.Alive())
                TargetPos = Target.UnitModel.GetPoint(Target.UnitData.HitPointName);
            moveHeight = BulletData.Data.GetFloat("MoveHeight");
            Debug.Log("高度:" + moveHeight);
            if (moveHeight == 0) Direction = TargetPos - this.Position;
        }
        public override void Update()
        {
            tickTime += SystemConfig.DeltaTime;
            if (Target.Alive())
                TargetPos = Target.UnitModel.GetPoint(Target.UnitData.HitPointName);
            if (moveHeight == 0)
            {
                Position = posBy(tickTime);
            }
            else
            {
                Position = posBy(tickTime);
                Direction = posBy(tickTime + SystemConfig.DeltaTime) - posBy(tickTime);
            }
            //Vector3 delta = TargetPos - Postion;
            //if (delta.magnitude < BulletData.Speed * SystemConfig.DeltaTime)
            //{
            //    Finish();
            //    if (Target.Alive())
            //        Skill.Hit(Target);
            //}
            //else
            //{
            //    if (moveHeight == 0)
            //        Postion += delta.normalized * BulletData.Speed * SystemConfig.DeltaTime;
            //    //Direction = TargetPos - this.Postion;
            //}
        }

        Vector3 posBy(float time)
        {
            Vector3 Postion;
            float totalTime = (TargetPos - StartPosition).magnitude / BulletData.Speed;
            if (time > totalTime)
            {
                Finish();
                if (Target.Alive())
                    Skill.Hit(Target);
            }
            Postion = StartPosition + (TargetPos - StartPosition) * (time / totalTime);
            if (moveHeight > 0)
            {
                float t = time / totalTime;
                Postion.y += (-5 * t * t + 5 * t) * moveHeight;
            }
            return Postion;
        }
    }
}
