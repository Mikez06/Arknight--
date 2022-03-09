using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bullets
{
    public class 大飞镖 : Bullet
    {
        HashSet<Unit> DamagedUnits = new HashSet<Unit>();

        bool arrive;
        CountDown LifeTime;
        CountDown TriggerTime = new CountDown();
        float radius;

        float moveHeight;//0:直线 1:抛物线
        float tickTime;
        public override void Init()
        {
            base.Init();
            if (Target.Alive())
                TargetPos = Target.GetHitPoint();
            moveHeight = BulletData.Data.GetFloat("MoveHeight");
            LifeTime = new CountDown(BulletData.Data.GetFloat("LifeTime"));
            radius = BulletData.Data.GetFloat("Radius");
            //Debug.Log("高度:" + moveHeight);
            if (moveHeight == 0 && BulletData.FaceCamera == 2) Direction = TargetPos - this.Position;
            if (BulletData.FaceCamera == 1) BulletModel.transform.eulerAngles = new Vector3(60, 0, 0);
            float scaleX = 1;
            if (BulletData.ScaleX == 1) scaleX = Target.ScaleX;
            if (BulletData.ScaleX == 2) scaleX = Skill.Unit.ScaleX;
            BulletModel.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        public override void Update()
        {
            tickTime += SystemConfig.DeltaTime;
            if (Target.Alive())
                TargetPos = Target.GetHitPoint();
            if (!arrive)
            {
                if (moveHeight == 0)
                {
                    Position = getPosOfTime(tickTime);
                }
                else
                {
                    Position = getPosOfTime(tickTime);
                    if (BulletData.FaceCamera == 2)
                        Direction = getPosOfTime(tickTime + SystemConfig.DeltaTime) - Position;
                }
            }
            if (DamagedUnits.Count > 0 && TriggerTime.Finished())
                TriggerTime.Set(BulletData.Data.GetFloat("Trigger"));
            if (TriggerTime.Update(SystemConfig.DeltaTime))
                DamagedUnits.Clear();
            var targets = Battle.FindAll(Position, radius, Skill.SkillData.TargetTeam);
            foreach (var t in targets)
            {
                if (!DamagedUnits.Contains(t))
                {
                    DamagedUnits.Add(t);
                    Skill.Hit(t, this);
                }
            }
            if (LifeTime.Update(SystemConfig.DeltaTime))
            {
                Finish();
            }
        }

        Vector3 getPosOfTime(float time)
        {
            Vector3 Postion;
            float totalTime = (TargetPos - StartPosition).magnitude / BulletData.Speed;
            if (time > totalTime)
            {
                Position = TargetPos;
                arrive = true;
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
