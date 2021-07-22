using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public Unit Unit;

    public List<Unit> Targets = new List<Unit>();
    protected Battle Battle => Unit.Battle;

    public SkillData Config => Database.Instance.Get<SkillData>(Id);

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

    protected List<Unit> LastTargets = new List<Unit>();

    /// <summary>
    /// 连发计时
    /// </summary>
    public CountDown Bursting = new CountDown();

    /// <summary>
    /// 连发计数
    /// </summary>
    public int BurstCount = -1;

    /// <summary>
    /// 开启时间
    /// </summary>
    public CountDown Opening = new CountDown();

    public float Power;
    public int MaxPower;
    public int PowerCount;

    public virtual void Init()
    {
        if (Config.AttackPoints != null)
        {
            AttackPoints = new List<Vector2Int>();
            UpdateAttackPoints();
        }

        Power = Config.StartPower;
        MaxPower = Config.MaxPower;
        PowerCount = Config.PowerCount;
    }

    public virtual void Update()
    {
        UpdateCooldown();
        if (Ready())
        {
            Start();
        }
        

        if (Casting.Update(SystemConfig.DeltaTime))
        {
            Cast();
        }

        if (Bursting.Update(SystemConfig.DeltaTime))
        {
            Burst();
        }

        if (!Casting.Finished()) //抬手期间，如果无有效目标，则取消抬手
        {
            if (!Config.RegetTarget && Targets.All(x => !x.Alive()))
            {
                Unit.State = StateEnum.Idle;
                BreakCast();
            }
        }
        if (Config.StopBreak)
        {
            if (Unit.IfStoped())
            {
                BreakCast();
            }
        }
    }

    public bool Useable()
    {
        if (Config.StopBreak && Unit.IfStoped()) return false;
        if (!Cooldown.Finished()) return false;
        return true;
    }

    public virtual void UpdateCooldown()
    {
        if (!Opening.Finished())
        {
            if (Opening.Update(SystemConfig.DeltaTime)) { Unit.OverWriteAnimation = null; }
            Power -= MaxPower / Config.OpenTime * SystemConfig.DeltaTime;
        }
        Cooldown.Update(SystemConfig.DeltaTime);
    }

    public virtual bool Ready()
    {
        if (!Unit.Recover.Finished() && Config.UseType != SkillUseTypeEnum.被动) return false;
        switch (Config.ReadyType)
        {
            case SkillReadyEnum.特技激活:
                if (Opening.Finished()) return false;
                break;
            case SkillReadyEnum.禁止主动:
                return false;
            default:
                break;
        }
        if (Config.AttackMode == AttackModeEnum.跟随攻击 && !Unit.Attacking.Finished()) return false;

        if (Config.UseType == SkillUseTypeEnum.手动) //手动技能在技能开启时可以使用
            return !Opening.Finished();
        //自动充能技在有充能时才能使用
        if (Config.MaxPower > 0 && Config.UseType == SkillUseTypeEnum.自动 && Power < MaxPower) return false;
        //不管什么技能 都要遵循技能CD
        return Cooldown.Finished();
    }

    public virtual void ResetCooldown(float attackSpeed)
    {
        //自动充能技，除了走冷却外，还要走Power
        //if (Config.MaxPower > 0)
        //    Power -= MaxPower;

        Cooldown.Set(Config.Cooldown * attackSpeed);
    }

    public void RecoverPower(float count)
    {
        if (PowerCount == 0) return;
        if (!Opening.Finished())
            return;
        Power += count;
        if (Power > MaxPower * PowerCount)
        {
            Power = MaxPower * PowerCount;
        }
    }

    #region 主动相关
    public bool CanOpen()
    {
        return Opening.Finished() && Power >= MaxPower && Useable();
    }

    public void DoOpen()
    {
        Debug.Log("OpenSkill");
        if (Config.StopOtherSkill)
        {
            Unit.BreakAllCast();
        }
        Opening.Set(Config.OpenTime);
        Unit.OverWriteAnimation = Config.OverwriteAnimation;
        OnOpen();
    }

    protected virtual void OnOpen()
    {

    }

    protected virtual void OnFinish()
    {

    }

    #endregion

    #region 使用流程
    /// <summary>
    /// 技能抬手
    /// </summary>
    public virtual void Start()
    {
        if (!Useable()) return;
        if (Targets.Count == 0)
        {
            FindTarget();
        }
        if (Targets.Count > 0)
        {
            var scaleX = (Targets[0].Position - Unit.Position).x > 0 ? 1 : -1;
            if (scaleX != Unit.ScaleX)
            {
                Unit.TargetScaleX = scaleX;
            }
        }
        else
        {
            return;
        }
        //走到这里技能就真的用出来了
        if (string.IsNullOrEmpty(Config.ModelAnimation))
        {
            Debug.Log(Unit.Config.Id + "的" + Config.Id + "没有抬手,直接使用");
            Cast();
            ResetCooldown(1);
        }
        else
        {
            var duration = Unit.UnitModel.GetSkillDelay(Config.ModelAnimation, Unit.GetAnimation(), out float fullDuration, out float beginDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
            float attackSpeed = 1 / Unit.Agi * 100;
            var baseAttackGap = Math.Max(Config.Cooldown, fullDuration);//基础攻击间隔，至少等于攻击动作持续时间
            if (Config.AttackMode == AttackModeEnum.跟随攻击)
            {
                attackSpeed = attackSpeed * (baseAttackGap + Unit.AttackGap) / baseAttackGap;
            }
            duration = duration * attackSpeed;
            fullDuration = fullDuration * attackSpeed;
            Casting.Set(duration);
            Debug.Log(Unit.Config.Id + "的" + Config.Id + "AttackStart,pointDelay:" + duration + ",fullDuration" + fullDuration + ",beginDuration" + beginDuration + ",Time:" + Time.time);
            Unit.Attacking.Set(fullDuration);
            Unit.State = StateEnum.Attack;
            Unit.AnimationName = Config.ModelAnimation;
            Unit.AnimationSpeed = 1 / attackSpeed * (beginDuration + fullDuration) / fullDuration;
            ResetCooldown(attackSpeed);
            if (duration == 0)
            {
                Cast();
            }
        }

        if (Config.StartEffect != null)
        {
            var ps = EffectManager.Instance.GetEffect(Config.StartEffect);
            ps.transform.position = Unit.UnitModel.SkeletonAnimation.transform.position;
            ps.transform.localScale = new Vector3(Unit.TargetScaleX, 1, 1);
            ps.PS.Play();
        }
    }

    /// <summary>
    /// 实际生效点
    /// </summary>
    public virtual void Cast()
    {
        if (Config.RegetTarget) FindTarget();//对于某些技能，无法攻击到已经离开攻击区域的单位

        if (Targets.Count > 0)
        {
            foreach (var t in Targets) Effect(t);
        }
        CastExSkill();
        if (Config.BurstCount > 0)
        {
            Burst();
        }
        Targets.Clear();
    }

    protected virtual void CastExSkill()
    {
        if (Config.ExSkills != null)
            foreach (var skillId in Config.ExSkills)
            {
                Unit.Skills.Find(x => x.Id == skillId).Cast();
            }
    }

    protected virtual void Burst()
    {
        if (BurstCount == -1)
        {
            BurstCount = Config.BurstCount;
            LastTargets.Clear();
            LastTargets.AddRange(Targets);
        }
        else
        {
            if (Config.BurstFind) //当目标为随机时
            {
                LastTargets.Clear();
                LastTargets.AddRange(GetAttackTarget());
            }
            foreach (var target in LastTargets)
            {
                Effect(target);
            }
        }
        BurstCount--;
        if (BurstCount != -1)

            if (Config.BurstDelay > 0)
                Bursting.Set(Config.BurstDelay);
            else
                Burst();
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
            var startPoint = Unit.UnitModel.GetPoint(Config.ShootPoint);
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
        if (Config.HitEffect != null)
        {
            var ps = EffectManager.Instance.GetEffect(Config.HitEffect);
            ps.transform.position = target.UnitModel.SkeletonAnimation.transform.position;
            ps.PS.Play();
        }
        if (Config.Buffs != null)
            foreach (var buffId in Config.Buffs)
            {
                target.AddBuff(buffId, this);
            }
        if (Unit.Skills[0] == this && Unit.MainSkill != null && Unit.MainSkill.Config.PowerType == PowerRecoverTypeEnum.攻击)
        {
            Unit.RecoverPower(1);
        }
        if (Config.DamageRate > 0)
        {
            if (Config.AreaRange != 0)
            {
                var targets = Battle.FindAll(target.Position2, Config.AreaRange, Config.TargetTeam);
                foreach (var t in targets)
                {
                    if (Config.EffectEffect != null)
                    {
                        var ps = EffectManager.Instance.GetEffect(Config.EffectEffect);
                        ps.transform.position = target.UnitModel.SkeletonAnimation.transform.position;
                        ps.PS.Play();
                    }
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
            {
                if (Config.EffectEffect != null)
                {
                    var ps = EffectManager.Instance.GetEffect(Config.EffectEffect);
                    ps.transform.position = target.UnitModel.SkeletonAnimation.transform.position;
                    ps.PS.Play();
                }
                if (Config.IfHeal)
                {
                    target.Heal(Unit.Attack * Config.DamageRate);
                }
                else
                {
                    target.Damage(new DamageInfo()
                    {
                        Source = this,
                        Attack = Unit.Attack,
                        DamageRate = Config.DamageRate,
                        DamageType = Config.DamageType,
                    });
                }
            }
        }
    }
    #endregion

    public virtual void FindTarget()
    {      
        Targets.Clear();
        Targets.AddRange(GetAttackTarget());
    }

    List<Unit> tempTargets = new List<Unit>();
    List<Unit> GetAttackTarget()
    {
        tempTargets.Clear();
        if (AttackPoints == null)//根据攻击范围进行索敌
        {
            tempTargets.AddRange(Battle.FindAll(Unit.Position2, Config.AreaRange, Config.TargetTeam));
        }
        else
        {
            tempTargets.AddRange(Battle.FindAll(AttackPoints, Config.TargetTeam));
        }
        if (tempTargets.Count > 0)
        {
            //首先计算出所有目标的仇恨优先级，然后再选出攻击个数的实际目标
            SortTarget(tempTargets);
            FilterTarget(tempTargets);
        }
        return tempTargets;
    }

    protected virtual void SortTarget(List<Unit> targets)
    {
        Func<Unit, float> firstOrder = x => 0, secondOrder = x => 0;

        switch (Config.AttackOrder2)
        {
            case AttackTargetOrder2Enum.飞行:
                firstOrder = x => -x.Config.Height;
                break;
            case AttackTargetOrder2Enum.远程:
                firstOrder = x => x.Skills.Count == 0 ? 0 : -x.Skills[0].Config.AttackRange;
                break;
            case AttackTargetOrder2Enum.Buff:
                break;
            case AttackTargetOrder2Enum.Tag:
                //firstOrder = x => x.Config.Tags == null ? 0 : -x.Config.Tags.Count();
                break;
            default:
                break;
        }

        switch (Config.AttackOrder)
        {
            case AttackTargetOrderEnum.终点距离:
                secondOrder = x => (x as Units.敌人).distanceToFinal();
                break;
            case AttackTargetOrderEnum.血量升序:
                secondOrder = x => x.Hp;
                break;
            case AttackTargetOrderEnum.血量降序:
                secondOrder = x => -x.Hp;
                break;
            case AttackTargetOrderEnum.血量比例升序:
                secondOrder = x => x.Hp / x.MaxHp;
                break;
            case AttackTargetOrderEnum.血量比例降序:
                secondOrder = x => -x.Hp / x.MaxHp;
                break;
            case AttackTargetOrderEnum.放置降序:
                secondOrder = x => (x as Units.干员).InputTime;
                break;
            case AttackTargetOrderEnum.区域顺序:
                secondOrder = x => Math.Abs(x.Position2.x - Unit.Position2.x) + Math.Abs(x.Position2.y - Unit.Position2.y);
                break;
            case AttackTargetOrderEnum.防御降序:
                secondOrder = x => -x.Defence;
                break;
            case AttackTargetOrderEnum.防御升序:
                secondOrder = x => x.Defence;
                break;
            case AttackTargetOrderEnum.攻击力升序:
                secondOrder = x => x.Attack;
                break;
            case AttackTargetOrderEnum.攻击力降序:
                secondOrder = x => -x.Attack;
                break;
            case AttackTargetOrderEnum.自身距离升序:
                secondOrder = x => -(x.Position - Unit.Position).magnitude;
                break;
            case AttackTargetOrderEnum.自身距离降序:
                secondOrder = x => (x.Position - Unit.Position).magnitude;
                break;
            case AttackTargetOrderEnum.血量未满随机:
                targets.RemoveAll(x => x.Hp == x.MaxHp);
                secondOrder = x => Battle.Random.Next(0, 1000);
                break;
            case AttackTargetOrderEnum.重量升序:
                secondOrder = x => x.Weight;
                break;
            case AttackTargetOrderEnum.重量降序:
                secondOrder = x => -x.Weight;
                break;
            case AttackTargetOrderEnum.随机:
                secondOrder = x =>
                {
                    var a = Battle.Random.Next(0, 1000);
                    Debug.Log(a); return a;
                };
                break;
            case AttackTargetOrderEnum.隐身优先:
                secondOrder = x => x.IfHide ? 0 : 1;
                break;
            case AttackTargetOrderEnum.未眩晕优先:
                secondOrder = x => x.State == StateEnum.stun ? 1 : 0;
                break;
            case AttackTargetOrderEnum.飞行优先:
                secondOrder = x => -x.Config.Height;
                break;
            case AttackTargetOrderEnum.未阻挡优先:
                secondOrder = x => (x as Units.敌人).StopUnit == null ? 0 : 1;
                break;
            case AttackTargetOrderEnum.沉睡优先:
                throw new Exception();
                break;
            case AttackTargetOrderEnum.无抵抗优先:
                throw new Exception();
                break;
        }
        var l = targets.OrderBy(firstOrder).ThenBy(secondOrder).ThenBy(x => x.Hatred()).ToList();
        targets.Clear();
        targets.AddRange(l);
    }

    protected virtual void FilterTarget(List<Unit> targets)
    {
        for (int i = targets.Count()-1; i >= Config.DamageCount; i--)
        {
            targets.RemoveAt(i);
        }
    }

    public void UpdateAttackPoints()
    {
        if (AttackPoints == null) return;
        AttackPoints.Clear();
        foreach (var p in Config.AttackPoints)
        {
            var point = (Unit as Units.干员).PointWithDirection(p);
            if (point.x < 0 || point.x >= Battle.Map.Grids.GetLength(0) || point.y < 0 || point.y >= Battle.Map.Grids.GetLength(1)) continue;
            AttackPoints.Add(point);
        }
    }

    public void BreakCast()
    {
        Casting.Finish();
        Bursting.Finish();
        BurstCount = -1;
    }
}

