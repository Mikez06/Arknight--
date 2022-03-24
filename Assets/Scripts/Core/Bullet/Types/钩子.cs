using UnityEngine;

namespace Bullets
{
    public class 钩子 : Bullet
    {
        PullLineModel PullLine;
        float tickTime;
        bool arrive;
        Buffs.拉动 pull;
        public override void Init()
        {
            base.Init();
            if (Target != null && Target.Alive())
                TargetPos = Target.GetHitPoint();
            if (BulletData.FaceCamera == 1) BulletModel.transform.eulerAngles = new Vector3(60, 0, 0);
            float scaleX = 1;
            if (BulletData.ScaleX == 1) scaleX = Target.ScaleX;
            if (BulletData.ScaleX == 2) scaleX = Skill.Unit.ScaleX;
            BulletModel.transform.localScale = new Vector3(scaleX, 1, 1);
            if (!string.IsNullOrEmpty(BulletData.Line))
            {
                PullLine = BulletManager.Instance.GetLine(BulletData.Line);
                //PullLine.SetStart(StartPosition);
                PullLine.SetStart(Skill.Unit.UnitModel.GetPoint(Skill.SkillData.ShootPoint));
                PullLine.SetEnd(StartPosition);
            }
        }
        public override void Update()
        {
            tickTime += SystemConfig.DeltaTime;
            if (Target != null && Target.Alive())
                TargetPos = Target.GetHitPoint();
            //PullLine.SetStart(Skill.Unit.UnitModel.GetPoint(Skill.SkillData.ShootPoint));
            if (arrive)
            {
                if (pull == null) Finish();
                else if (pull.Dead) Finish();
                else PullLine?.SetEnd(Target.GetHitPoint());
            }
            else
            {
                Position = getPosOfTime(tickTime);
                PullLine?.SetEnd(Position);
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
                if (BulletModel != null)
                {
                    BulletManager.Instance.Return(BulletModel);
                    BulletModel = null;
                }
                //Finish();
                if (Target == null)
                    Skill.Hit(TargetPos.ToV2(), this);
                else if (Target.Alive())
                    Skill.Hit(Target, this);
                pull = (Skill as Skills.拉).pull;
            }
            Postion = StartPosition + (TargetPos - StartPosition) * (time / totalTime);
            return Postion;
        }

        public override void Finish()
        {
            base.Finish();
            if (PullLine != null)
            {
                BulletManager.Instance.Return(PullLine);
            }
        }
    }
}
