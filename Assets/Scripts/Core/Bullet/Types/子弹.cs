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
                TargetPos = Target.GetHitPoint();
            moveHeight = BulletData.Data.GetFloat("MoveHeight");
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

        Vector3 getPosOfTime(float time)
        {
            Vector3 Postion;
            float totalTime = (TargetPos - StartPosition).magnitude / BulletData.Speed;
            if (time > totalTime)
            {
                Position = TargetPos;
                Finish();
                if (Target.Alive())
                    Skill.Hit(Target, this);
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
