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
            UpdateCooldown();
            if (Unit.Recover.Finished())
            {
                if (Ready())
                {
                    if (Target != null && Unit.Turning.Finished())//已有目标，说明在等待转身结束
                    {
                        if (Target.Alive())
                            Start();
                        else
                            Target = null;
                    }
                    if (Target == null) //没有目标 尝试找一个
                    {
                        FindTarget();
                        if (Target != null)
                        {
                            var scaleX = (Target.Position - Unit.Position).x > 0 ? 1 : -1;
                            if (scaleX != Unit.ScaleX)
                            {
                                Unit.TargetScaleX = scaleX;
                                Unit.Turning.Set(SystemConfig.TurningTime);
                            }
                            else
                            {
                                Start();
                            }
                        }
                    }
                }
            }


            if (Casting.Update(SystemConfig.DeltaTime))
            {
                if (!Selectable(Target))
                {
                    Target = null;
                    FindTarget();
                }
                if (Target != null)
                    Cast();
            }
        }

        public virtual void UpdateCooldown()
        {
            if (this == Unit.MainSkill && !Cooldown.Finished())
            {
                Cooldown.Update(SystemConfig.DeltaTime);
                Unit.Power -= Config.MaxPower / Config.Cooldown * SystemConfig.DeltaTime;
            }
            else
            {
                Cooldown.Update(SystemConfig.DeltaTime);
            }
        }

        public virtual bool Ready()
        {
            if (Unit.State == StateEnum.Attack) return false;
            if (this == Unit.MainSkill)
                return Unit.Power >= Unit.MaxPower;
            else
                return Cooldown.Finished();
        }

        public virtual void ResetCooldown()
        {
            if (this == Unit.MainSkill)
            {
                Cooldown.Set(Config.Cooldown);
            }
            else
                Cooldown.Set(Config.Cooldown);
        }

        public virtual void Start()
        {
            ResetCooldown();
            var duration = Unit.UnitModel.GetSkillDelay(Config.ModelAnimation, Unit.AnimationName, out float fullDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
            Casting.Set(duration);
            Debug.Log(Unit.Config._Id + "AttackStart,pointDelay:" + duration + ",fullDuration" + fullDuration + ",Time:" + Time.time);
            Unit.Attacking.Set(fullDuration);
            Unit.Recover.Set(duration);
            Unit.State = StateEnum.Attack;
            Unit.AnimationName = Config.ModelAnimation;
            Unit.AnimationSpeed = 1;
        }


        public override void Cast()
        {
            HashSet<Unit> targets = null;
            if (Config.HitCount > 1)
            {
                var all = Battle.FindUnits(AttackPoints, Config.TargetTeam, Selectable).ToList();
                all.Remove(Target);//主目标一定可以选到
                targets = new HashSet<Unit>();
                targets.Add(Target);
                for (int i = 0; i < Config.HitCount - 1; i++)
                {
                    if (all.Count == 0) break;
                    var t = all[Battle.Random.Next(0, all.Count)];
                    all.Remove(t);
                    targets.Add(t);
                }
            }

            if (targets != null)
            {
                foreach (var target in targets) Effect(target);
            }
            else
                Effect(Target);

            Target = null;
        }

        public virtual void Effect(Unit target)
        {

            if (Config.Bullet == null)
            {
                Hit(target);
            }
            else
            {
                //创建一个子弹
                //TODO 改为从表里读取发射点 而不是现找模型
                var startPoint = Unit.UnitModel.SkeletonAnimation.transform.position + new Vector3(Unit.Config.AttackPointX * (Unit.Direction.x > 0 ? 1 : -1), Unit.Config.AttackPointY, 0);
                Debug.Log(startPoint + "," + (target.UnitModel.SkeletonAnimation.transform.position + new Vector3(target.Config.HitPointX * (target.Direction.x > 0 ? 1 : -1), target.Config.HitPointY, 0)));
                Battle.CreateBullet(Config.Bullet.Value, startPoint, Vector3.zero, target, this);
            }
        }

        public override void Hit(Unit target)
        {
            if (Unit.Skills[0]==this && Unit.MainSkill!=null&& Unit.MainSkill.Config.PowerType == MainSkillTypeEnum.主动)
            {
                Unit.RecoverPower(1);
            }
            target.Damage(new DamageInfo()
            {
                Source = this,
                Attack = Unit.Attack,
                DamageRate = Config.DamageRate,
                DamageType = Config.DamageType,
            });
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
            //if (Config.AttackTarget == AttackTargetEnum.敌对)
            //{
            //    if (Unit is Units.干员 u && !AttackPoints.Any(x => x == target.GridPos)) return false;
            //    if (Unit is Units.敌人 e && (target.Position2 - Unit.Position2).magnitude < Config.AttackRange) return false;
            //}
            if (Config.IfHeal && target.Hp == target.MaxHp) return false;
            if (!target.Alive()) return false;
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
                if (Config.AttackStop)//阻挡优先算
                {
                    if (Unit is Units.干员 u) Target = u.StopUnits.FirstOrDefault();
                    else if (Unit is Units.敌人 e) Target = e.StopUnit;
                }
                if (Target != null) return;
                if (AttackPoints == null)
                {
                    //靠半径寻找目标
                    Target = Battle.FindFirst(Unit.Position2, Config.AttackRange, Selectable, targetOrder);
                    if (Target != null) Debug.Log(Target.Config._Id);
                }
                else
                {
                    //找网格点
                    Target = Battle.FindFirst(AttackPoints, Config.TargetTeam, Selectable, targetOrder);
                }
            }
        }
    }
}
