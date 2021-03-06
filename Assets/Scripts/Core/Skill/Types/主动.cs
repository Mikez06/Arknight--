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
            if (Unit.Recover.Finished())
            {
                Cooldown.Update(SystemConfig.DeltaTime);
                if (Cooldown.Finished())
                {
                    FindTarget();
                    if (Target != null)
                    {
                        Start();
                        Cooldown.Set(Config.Cooldown);
                    }
                }
            }
            if (Casting.Update(SystemConfig.DeltaTime))
            {
                DoEffect();
            }
        }

        public virtual void Start()
        {
            var duration = Unit.UnitModel.GetSkillDelay(Config.ModelAnimation, Unit.AnimationName, out float fullDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
            Casting.Set(duration);
            Debug.Log(Unit.Config._Id + "AttackStart,pointDelay:" + duration + ",fullDuration" + fullDuration);
            Unit.Attacking.Set(fullDuration);
            Unit.State = StateEnum.Attack;
            Unit.AnimationName = Config.ModelAnimation;
            Unit.AnimationSpeed = 1;
        }

        public virtual void DoEffect()
        {
            Debug.Log("DoEffect");
        }

        /// <summary>
        /// 技能能否选中某目标
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool Selectable(Unit target)
        {

            if (Config.AttackTarget == AttackTargetEnum.阻挡)
            {
                if (Unit is Units.干员 u && !u.StopUnits.Contains(target)) return false;
                if (Unit is Units.敌人 e && e.StopUnit != target) return false;
            }
            if (Config.AttackTarget == AttackTargetEnum.敌对)
            {
                if (Unit is Units.干员 u && !Config.AttackPoints.Any(x => target.GridPos == u.PointWithDirection(x))) return false;
                if (Unit is Units.敌人 e && (target.Position2 - Unit.Position2).magnitude < Config.AttackRange) return false;
            }
            if (Config.IfHeal && target.Hp == target.MaxHp) return false;
            return true;
        }

        protected virtual float targetOrder(Unit target)
        {
            switch (Config.AttackOrder)
            {
                case AttackTargetOrderEnum.血量:
                    return target.Hp / target.MaxHp;
                case AttackTargetOrderEnum.离家近:
                    return (target as Units.敌人).distanceToFinal();
                case AttackTargetOrderEnum.放置顺序:
                    return (target as Units.干员).MapIndex;
            }
            return 0;
        }

        public override void FindTarget()
        {
            //没有目标或目标不合法
            if (Target == null || !Selectable(Target))
            {
                if (Config.AttackTarget == AttackTargetEnum.阻挡)//阻挡优先算
                {
                    if (Unit is Units.干员 u) Target = u.StopUnits.FirstOrDefault();
                    else if (Unit is Units.敌人 e) Target = e.StopUnit;
                    return;
                }
                if (Config.AttackPoints == null)
                {
                    //靠半径寻找目标
                    Target = Battle.FindFirst(Unit.Position2, Config.AttackRange, Selectable, targetOrder);
                    if (Target != null) Debug.Log(Target.Config._Id);
                }
                else
                {
                    //找网格点
                    Target = Battle.FindFirst(Unit.GridPos, Config.AttackPoints, Config.TargetTeam, Selectable, targetOrder);
                }
            }
        }
    }
}
