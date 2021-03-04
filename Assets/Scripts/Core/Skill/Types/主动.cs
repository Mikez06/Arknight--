using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skills
{
    public class 主动 : Skill
    {
        public CountDown Cooldown = new CountDown();
        public CountDown Casting = new CountDown();

        public override void Update()
        {
            base.Update();
            if (!Unit.Recover.Finished())
            {
                UpdateTarget();
                if (Cooldown.Update(SystemConfig.DeltaTime))
                {
                    Cooldown.Set(Config.Cooldown);
                    Start();
                }
            }
            if (Casting.Update(SystemConfig.DeltaTime))
            {
                DoEffect();
            }
        }

        public virtual void Start()
        {
            var ani = Unit.UnitModel.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
            Casting.Set(ani.Duration);
        }

        public virtual void DoEffect()
        {

        }

        /// <summary>
        /// 技能能否选中某目标
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool Selectable(Unit target)
        {
            if (Config.AttackTarget == AttackTargetEnum.Self && target != Unit)
            {
                return false;
            }
            if (Config.AttackTarget == AttackTargetEnum.Enemy && target.Config.Team == Unit.Config.Team)
            {
                return false;
            }
            if (Config.AttackTarget == AttackTargetEnum.Friends && target.Config.Team != Unit.Config.Team)
            {
                return false;
            }
            if (Config.AttackPoints != null)
            {
                if (!(Unit as Units.干员).AttackPoints.Contains(target.GridPos)) return false;
            }
            else
            {
                if ((target.Position - Unit.Position).magnitude > Config.AttackRange) return false;
            }
            return true;
        }

        public virtual void UpdateTarget()
        {
            if (Target == null || !Selectable(Target))
                Target = Battle.FindNearTarget(Unit.Position, Selectable);
        }
    }
}
