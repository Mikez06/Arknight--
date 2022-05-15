using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public class 持续施法子弹 : Bullet
    {
        CountDown Trigger = new CountDown();
        CountDown LifeTime = new CountDown();
        float trigger;
        PullLineModel PullLine;

        public override void Init()
        {
            base.Init();
            trigger = Skill.SkillData.Cooldown;
            Position = GetTargetPos(Target);
            Skill.Unit.AttackingSkill = Skill;
            Skill.Unit.OverWriteAnimation = Skill.SkillData.OverwriteAnimation;
            Skill.Unit.AttackingAction.Set(float.PositiveInfinity);
            LifeTime.Set(BulletData.Data.GetFloat("LifeTime"));
            if (!string.IsNullOrEmpty(BulletData.Line))
            {
                PullLine = BulletManager.Instance.GetLine(BulletData.Line);
                //PullLine.SetStart(StartPosition);
                PullLine.SetStart(Skill.Unit.UnitModel.GetPoint(Skill.SkillData.ShootPoint));
                PullLine.SetEnd(Position);
            }
        }

        public override void Update()
        {
            base.Update();
            Skill.Unit.AttackingSkill = Skill;
            Skill.Unit.OverWriteAnimation = Skill.SkillData.OverwriteAnimation;
            Skill.Unit.AttackingAction.Set(float.PositiveInfinity);
            if (PullLine != null)
            {
                PullLine.SetStart(Skill.Unit.UnitModel.GetPoint(Skill.SkillData.ShootPoint));
            }
            if (Trigger.Finished())
            {
                Skill.Hit(Target, this);
                Trigger.Set(trigger);
            }
            Trigger.Update(SystemConfig.DeltaTime);
            if (LifeTime.Update(SystemConfig.DeltaTime)) Finish();
            if (!Skill.Unit.Alive() || !Target.Alive() || Skill.Unit.IfStun)
            {
                Finish();
            }
        }

        public override void Finish()
        {
            base.Finish();
            Skill.Unit.OverWriteAnimation = null;
            Skill.Unit.AttackingSkill = null;
            Skill.Unit.AttackingAction.Finish();
            Skill.Unit.BreakAllCast();
            if (PullLine != null)
            {
                BulletManager.Instance.Return(PullLine);
            }
        }
    }
}
