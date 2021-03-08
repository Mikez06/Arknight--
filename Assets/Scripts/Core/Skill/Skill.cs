using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public Unit Unit;

    public Unit Target;
    protected Battle Battle => Unit.Battle;

    public SkillConfig Config => Database.Instance.Get<SkillConfig>(Id);

    public int Id;

    public List<Vector2Int> AttackPoints;


    /// <summary>
    /// 冷却时间，对于单位主技能来说
    /// </summary>
    public CountDown Cooldown = new CountDown();
    /// <summary>
    /// 技能抬手
    /// </summary>
    public CountDown Casting = new CountDown();

    public CountDown Opening = new CountDown();

    protected bool ifUsePower => this == Unit.MainSkill;

    public virtual void Init()
    {
        if (Config.AttackPoints != null)
        {
            AttackPoints = new List<Vector2Int>();
            UpdateAttackPoints();
        }
    }

    public virtual void Update()
    {
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
        if (!Opening.Finished())
        {
            Opening.Update(SystemConfig.DeltaTime);
            Unit.Power -= Config.MaxPower / Config.OpenTime * SystemConfig.DeltaTime;
        }
        Cooldown.Update(SystemConfig.DeltaTime);
    }

    public virtual bool Ready()
    {
        if (Unit.State == StateEnum.Attack) return false;
        //if (ifUsePower && Unit.Power < Unit.MaxPower) return false;
        if (Config.UseType == SkillUseTypeEnum.手动) //手动技能在阻回时可以使用
            return !Opening.Finished();
        //自动充能技在有充能时才能使用
        if (ifUsePower && Config.UseType == SkillUseTypeEnum.自动 && Unit.Power < Unit.MaxPower) return false;
        //不管什么技能 都要遵循技能CD
        return Cooldown.Finished();
    }

    public virtual void ResetCooldown()
    {
        //自动充能技，除了走冷却外，还要走Power
        if (ifUsePower && Config.UseType == SkillUseTypeEnum.自动)
            Unit.Power -= Unit.MaxPower;

        Cooldown.Set(Config.Cooldown);
    }

    #region 主动相关
    public bool CanOpen()
    {
        return Opening.Finished() && Unit.Power >= Unit.MaxPower;
    }

    public void DoOpen()
    {
        Debug.Log("OpenSkill");
        Opening.Set(Config.OpenTime);
    }

    #endregion

    #region 使用流程
    public virtual void Start()
    {
        if (!Selectable(Target)) return;
        ResetCooldown();
        var duration = Unit.UnitModel.GetSkillDelay(Config.ModelAnimation, Unit.AnimationName, out float fullDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
        Casting.Set(duration);
        Debug.Log(Unit.Config._Id+"的"+Config._Id + "AttackStart,pointDelay:" + duration + ",fullDuration" + fullDuration + ",Time:" + Time.time);
        Unit.Recover.Set(duration);
        Unit.Attacking.Set(fullDuration);
        Unit.State = StateEnum.Attack;
        Unit.AnimationName = Config.ModelAnimation;
        Unit.AnimationSpeed = 1;
    }

    public virtual void Cast()
    {
        HashSet<Unit> targets = null;
        if (Config.HitCount > 1)
        {
            var all = Battle.FindUnits(AttackPoints, Config.TargetTeam, Selectable).ToList();
            //all.Remove(Target);//主目标一定可以选到
            targets = new HashSet<Unit>();
            for (int i = 0; i < Config.HitCount - 1; i++)
            {
                if (all.Count == 0) break;
                var t = all[Battle.Random.Next(0, all.Count)];
                all.Remove(t);
                targets.Add(t);
            }
            if (targets.Count == 0) targets.Add(Target);//主目标当保底
        }

        if (targets != null)
        {
            foreach (var target in targets) Effect(target);
        }
        else
            Effect(Target);

        Target = null;
    }

    /// <summary>
    /// 技能对一个单位实际生效的效果
    /// </summary>
    /// <param name="target"></param>
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
            //Debug.Log(startPoint + "," + (target.UnitModel.SkeletonAnimation.transform.position + new Vector3(target.Config.HitPointX * (target.Direction.x > 0 ? 1 : -1), target.Config.HitPointY, 0)));
            Battle.CreateBullet(Config.Bullet.Value, startPoint, Vector3.zero, target, this);
        }
    }

    /// <summary>
    /// 伤害判定阶段
    /// </summary>
    /// <param name="target"></param>
    public virtual void Hit(Unit target)
    {
        if (Unit.Skills[0] == this && Unit.MainSkill != null && Unit.MainSkill.Config.PowerType == PowerRecoverTypeEnum.主动)
        {
            Unit.RecoverPower(1);
        }
        if (Config.AreaRange != null)
        {
            var targets = Battle.FindAll(target.Position2, Config.AreaRange.Value, Config.TargetTeam, Selectable);
            foreach (var t in targets)
            {
                t.Damage(new DamageInfo()
                {
                    Source = this,
                    Attack = Unit.Attack,
                    DamageRate = t == target ? Config.DamageRate : Config.AreaDamage,
                    DamageType = Config.DamageType,
                });
            }
        }
        else
            target.Damage(new DamageInfo()
            {
                Source = this,
                Attack = Unit.Attack,
                DamageRate = Config.DamageRate,
                DamageType = Config.DamageType,
            });
    }
    #endregion
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
            case AttackTargetOrderEnum.飞行:
                //优先攻击飞行，次要攻击离家近的
                return (target as Units.敌人).distanceToFinal() - target.Config.Height == 0 ? 0 : -100;
        }
        return 0;
    }

    public virtual void FindTarget()
    {
        //没有目标或目标不合法
        if (Config.AttackTarget == AttackTargetEnum.阻挡)//阻挡优先算
        {
            if (Unit is Units.干员 u) Target = u.StopUnits.FirstOrDefault();
            else if (Unit is Units.敌人 e) Target = e.StopUnit;
        }
        if (Target != null) return;
        if (AttackPoints == null)
        {
            //靠半径寻找目标
            Target = Battle.FindFirst(Unit.Position2, Config.AttackRange, Config.TargetTeam, Selectable, targetOrder);
            if (Target != null)
                Debug.Log(Target.Config._Id);
        }
        else
        {
            //找网格点
            Target = Battle.FindFirst(AttackPoints, Config.TargetTeam, Selectable, targetOrder);
        }
    }

    public void UpdateAttackPoints()
    {
        AttackPoints.Clear();
        foreach (var p in Config.AttackPoints)
        {
            var point = (Unit as Units.干员).PointWithDirection(p);
            if (point.x < 0 || point.x >= Battle.Map.Grids.GetLength(0) || point.y < 0 || point.y >= Battle.Map.Grids.GetLength(1)) continue;
            AttackPoints.Add(point);
        }
    }
}

