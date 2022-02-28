﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public Unit Unit;

    public Skill Parent;

    public List<Modify> Modifies = new List<Modify>();

    public List<Unit> Targets = new List<Unit>();
    protected Battle Battle => Unit.Battle;

    public SkillData SkillData => Database.Instance.Get<SkillData>(Id);

    public int Id;

    public int StartId = -1;//升级前id

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
    public int MaxPower => (int)(Unit.SkillCost * MaxPowerBase);
    public float MaxPowerBase;
    public int PowerCount;
    public int UseCount;

    Effect ReadyEffect;

    public CountDown LoopingStart = new CountDown();
    public CountDown LoopingEnd = new CountDown();
    public Effect LoopStartEffect, LoopCastEffect;

    public virtual void Init()
    {
        if (SkillData.Modifys != null)
        {
            for (int i = 0; i < SkillData.Modifys.Length; i++)
            {
                Modifies.Add(ModifyManager.Instance.Get(SkillData.Modifys[i]));
            }
        }
        else
        {

        }

        if (SkillData.AttackPoints != null)
        {
            AttackPoints = new List<Vector2Int>();
            UpdateAttackPoints();
        }

        MaxPowerBase = SkillData.MaxPower;
        PowerCount = SkillData.PowerCount;
        Reset();
    }

    public Skill GetFinalParent()
    {
        var result = this;
        while (result.Parent != null)
            result = result.Parent;
        return result;
    }

    public virtual void Reset()
    {
        Opening.Finish();
        UseCount = 0;
        Power = SkillData.StartPower;
        BreakCast();
        Cooldown.Finish();
    }

    public virtual void Update()
    {
        if (LoopingStart.Update(SystemConfig.DeltaTime))
        {
            if (SkillData.LoopCastEffect != null)
            {
                LoopCastEffect = EffectManager.Instance.GetEffect(SkillData.LoopCastEffect.Value);
                LoopCastEffect.Init(Unit, Unit, Unit.Position, Unit.Direction);
            }
        }
        if (LoopingEnd.Update(SystemConfig.DeltaTime))
        {
            Unit.UnitModel.ChangeToEnd();
            if (LoopCastEffect != null)
            {
                EffectManager.Instance.ReturnEffect(LoopCastEffect);
                LoopCastEffect = null;
            }
        }

        if (SkillData.PowerType == PowerRecoverTypeEnum.自动)
        {
            RecoverPower(Unit.PowerSpeed * SystemConfig.DeltaTime);
        }

        if (SkillData.ReadyEffect != null)
        {
            if (ReadyEffect == null && Power >= MaxPower)
            {
                ReadyEffect = EffectManager.Instance.GetEffect(SkillData.ReadyEffect.Value);
                ReadyEffect.transform.SetParent(Unit.UnitModel.transform);
                ReadyEffect.transform.position = Unit.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.ReadyEffect.Value).BindPoint);
            }
            else if (ReadyEffect != null && Power < MaxPower)
            {
                EffectManager.Instance.ReturnEffect(ReadyEffect);
                ReadyEffect = null;
            }
        }

        if (SkillData.AutoUse && Power == MaxPower)
        {
            DoOpen();
        }
        
        if (!Casting.Finished()) //抬手期间，如果无有效目标，则取消抬手
        {
            if (!SkillData.RegetTarget && Targets.All(x => !CanUseTo(x)))
            {
                Log.Debug($"{Unit.UnitData.Id}的{SkillData.Name}全部目标不合法,强制打断抬手动作{Time.time}");
                BreakCast();
            }
        }

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


        if (SkillData.StopBreak)
        {
            if (Unit.IfStoped() && Unit.AttackingSkill == this)
            {
                BreakCast();
            }
        }
    }

    //与ready不同的是，被动技能也会受此函数影响
    public bool Useable()
    {
        if (SkillData.MaxUseCount != 0 && UseCount >= SkillData.MaxUseCount) return false;
        if (GetFinalParent() == Unit.FirstSkill && !Unit.CanAttack)
        {
            //Debug.Log($"{Unit.UnitData.Id} 因为缴械无法使用{SkillData.Id}");
            return false;
        }
        if (SkillData.StopBreak && Unit.IfStoped()) return false;
        if (!Cooldown.Finished()) return false;

        if (SkillData.OpenDisable && !Unit.MainSkill.Opening.Finished()) return false;
        if (SkillData.EnableBuff != null && !SkillData.EnableBuff.All(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;
        if (SkillData.DisableBuff != null && SkillData.DisableBuff.Any(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;
        return true;
    }

    public bool CanUseTo(Unit target)
    {
        if (target == null) return false;
        if (SkillData.IfHeal && (!target.CanBeHeal || target.Hp == target.MaxHp)) return false;
        if (!target.IfSelectable) return false;
        switch (SkillData.TargetFilter)
        {
            case SkillTargetFilterEnum.仅自己:
                if (target != Unit) return false;
                break;
            case SkillTargetFilterEnum.自己以外:
                if (target == Unit) return false;
                break;
            case SkillTargetFilterEnum.召唤物:
                if (target != Unit && !(Unit as Units.干员).Children.Contains(target)) return false;
                break;
            case SkillTargetFilterEnum.仅召唤:
                if (!(Unit as Units.干员).Children.Contains(target)) return false;
                break;
        }
        if (SkillData.TargetHpLess != 0 && target.Hp / target.MaxHp > SkillData.TargetHpLess) return false;
        if (SkillData.UnitLimit != null && !SkillData.UnitLimit.Contains(target.Id)) return false;
        if ((SkillData.TargetTeam >> target.Team) % 2 == 0) return false;
        if (SkillData.ProfessionLimit != UnitTypeEnum.无 && SkillData.ProfessionLimit != target.UnitData.Profession) 
            return false;
        if (!SkillData.AttackFly && target.Height > 0) return false;
        if (!target.Alive() && !SkillData.DeadFind) return false;
        if (!SkillData.AntiHide && target.IfHide) return false;
        if (SkillData.TargetDisableBuff != null)
        {
            if (SkillData.TargetDisableBuff.Any(x => target.Buffs.Any(y => y.Id == x))) return false;
        }
        if (SkillData.TargetEnableBuff != null)
        {
            if (SkillData.TargetEnableBuff.All(x => target.Buffs.Any(y => y.Id == x))) return false;
        }
        return true;
    }

    public virtual void UpdateCooldown()
    {
        if (!Opening.Finished())
        {
            if (Opening.Update(SystemConfig.DeltaTime))
            {
                Battle.TriggerDatas.Push(new TriggerData()
                {
                    Target = Unit,
                    Skill = this,
                });
                Unit.Trigger(TriggerEnum.技能结束);
                Battle.TriggerDatas.Pop();
                OnOpenEnd();
            }
            //Power -= MaxPower / Config.OpenTime * SystemConfig.DeltaTime;
        }
        if (Cooldown.Update(SystemConfig.DeltaTime))
        {
            if (Unit.AttackingSkill == this)
                Unit.AttackingSkill = null;
        }
    }

    public virtual bool Ready()
    {
        //if (!LoopingStart.Finished()) return false;
        if (Unit.IfStun)
            return false;
        if (SkillData.UseType == SkillUseTypeEnum.被动) return false;
        switch (SkillData.ReadyType)
        {
            case SkillReadyEnum.特技激活:
                if (Unit.MainSkill == null)
                {
                    if (Opening.Finished()) return false;
                }
                else
                {
                    if (Unit.MainSkill.Opening.Finished()) return false;
                }
                break;
            case SkillReadyEnum.禁止主动:
                return false;
            case SkillReadyEnum.充能释放:
                if (Power < MaxPower) return false;
                break;
            default:
                break;
        }
        if (SkillData.StopBreak && Unit.IfStoped()) return false;

        if (SkillData.AttackMode == AttackModeEnum.跟随攻击 && Unit.AttackingSkill != null) return false;

        if (SkillData.UseType == SkillUseTypeEnum.手动) //手动技能在技能开启时可以使用
            return !Opening.Finished();
        //自动充能技在有充能时才能使用,另外要和自动开启技能区分开
        if (SkillData.MaxPower > 0 && SkillData.UseType == SkillUseTypeEnum.自动 && Power < MaxPower &&!SkillData.AutoUse) return false;
        //不管什么技能 都要遵循技能CD
        return Cooldown.Finished();
    }

    public virtual bool InAttackUsing()
    {
        if (SkillData.UseType == SkillUseTypeEnum.被动) return false;
        if (SkillData.EnableBuff != null && !SkillData.EnableBuff.All(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;
        if (SkillData.DisableBuff != null && SkillData.DisableBuff.Any(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;
        if (SkillData.AttackPoints == null) return false;

        if (SkillData.ReadyType == SkillReadyEnum.特技激活 &&
            ((SkillData.MaxPower > 0 && !Opening.Finished())
            || (SkillData.MaxPower == 0 && !Unit.MainSkill.Opening.Finished())
            ))
        {
            return true;
        }
        if (SkillData.ReadyType == SkillReadyEnum.None)
        {
            return true;
        }
        return false;
    }

    public virtual void ResetCooldown(float attackSpeed)
    {
        //TODO 读Unit的攻击间隔变化
        var cooldown = (SkillData.Cooldown == 0 && SkillData.AttackMode == AttackModeEnum.跟随攻击 ? Unit.AttackGap : SkillData.Cooldown) * attackSpeed;
        //Debug.Log(SkillData.Id + "cooldown:" + cooldown);
        //if (cooldown < 0.1f) cooldown = 0.1f;
        Cooldown.Set(cooldown);
    }

    public void RecoverPower(float count, bool withTip = false,bool ignoreOpening=false)
    {
        if (PowerCount == 0) return;
        if (!Opening.Finished() && !ignoreOpening)
            return;
        if (withTip)
        {
            Unit.UnitModel.ShowPower(count);
        }
        Power += count;
        if (Power > MaxPower * PowerCount)
        {
            Power = MaxPower * PowerCount;
        }
    }

    #region 主动相关
    public bool CanOpen()
    {
        if (SkillData.ReadyType == SkillReadyEnum.充能释放 && SkillData.UseType == SkillUseTypeEnum.手动 && !SkillData.NoTargetAlsoUse)
        {
            var target = GetAttackTarget();
            if (target.Count == 0)
            {
                return false;
            }
        }
        return Opening.Finished() && Power >= MaxPower && Useable();
    }

    public void DoOpen()
    {
        Debug.Log("OpenSkill");
        if (SkillData.StopOtherSkill)
        {
            Unit.BreakAllCast();
        }
        if (SkillData.ReadyType == SkillReadyEnum.特技激活)
        {
            Power -= MaxPower;
            Opening.Set(SkillData.OpenTime);
        }
        if (SkillData.ReadyType == SkillReadyEnum.充能释放)
        {
            Start();
        }
        var animation = SkillData.OverwriteAnimation;
        if (SkillData.OverwriteAnimationDown != null && Unit is Units.干员 u && u.Direction_E == DirectionEnum.Up) animation = SkillData.OverwriteAnimationDown;
        Unit.OverWriteAnimation = animation;
        if (animation != null && animation.Length > 1)
        {
            if (SkillData.LoopStartEffect != null)
            {
                LoopStartEffect = EffectManager.Instance.GetEffect(SkillData.LoopStartEffect.Value);
                LoopStartEffect.Init(Unit, Unit, Unit.Position, Unit.Direction);
            }
            LoopingStart.Set(Unit.UnitModel.GetAnimationDuration(animation[0]));
            LoopingEnd.Set(Opening.value - Unit.UnitModel.GetAnimationDuration(animation[2]));
        }
        OnSkillOpen();
    }

    #endregion

    #region 使用流程

    protected virtual float GetSkillDelay(string[] animationName, string[] lastState, out float fullDuration, out float beginDuration)
    {
        return Unit.UnitModel.GetSkillDelay(animationName, lastState, out fullDuration, out beginDuration);
    }

    float lastSpeed;
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
            if (Targets[0] != Unit && SkillData.ModelAnimation != null)//默认不填动作的技能不需要转身
            {
                var scaleX = (Targets[0].Position - Unit.Position).x > 0 ? 1 : -1;
                if (scaleX != Unit.ScaleX)
                {
                    Unit.TargetScaleX = scaleX;
                }
            }
        }
        else if (!SkillData.NoTargetAlsoUse)
        {
            return;
        }

        //走到这里技能就真的用出来了
        UseCount++;
        //Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "使用次数:" + UseCount);
        if (SkillData.ReadyType == SkillReadyEnum.充能释放)
        {
            Power -= MaxPower;
            Opening.Set(SkillData.OpenTime);
        }

        if (SkillData.ModelAnimation == null)
        {
            //Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "没有动画,直接使用");
            ResetCooldown(1);
            Cast();
        }
        else
        {
            var animation = SkillData.ModelAnimation;
            if (SkillData.ModelAnimationDown != null && Unit is Units.干员 u && u.Direction_E == DirectionEnum.Up) animation = SkillData.ModelAnimationDown;
            var duration = GetSkillDelay(SkillData.OverwriteAnimation == null ? animation: SkillData.OverwriteAnimation, Unit.GetAnimation(), out float fullDuration, out float beginDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
            float attackSpeed = 1f / Unit.Agi * 100;//攻速影响冷却时间
            if (SkillData.AttackMode == AttackModeEnum.固定间隔) attackSpeed = 1;
            ResetCooldown(attackSpeed);
            //float aniSpeed = 1;//动画表现上的攻速
            if (fullDuration * attackSpeed != Cooldown.value)
            {
                //动画时间已经超出攻击间隔了，此时攻速被攻击间隔强制拉快，动画速度也会被强制拉快
                //动画时间低于攻击间隔时，动画也会被拉长
                attackSpeed = Cooldown.value / fullDuration;
                attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, Unit.UnitData.MaxAnimationScale);
            }
            duration = duration * attackSpeed;
            fullDuration = fullDuration * attackSpeed;
            this.lastSpeed = 1f / attackSpeed;
            Unit.AttackingAction.Set(fullDuration);
            Unit.State = StateEnum.Attack;
            Unit.AnimationName = animation;
            Unit.AttackingSkill = this;
            //Debug.Log(SkillData.ModelAnimation);
            if (SkillData.OverwriteAnimation == null)
            {
                Unit.UnitModel?.BreakAnimation();//防止覆盖动画被打断
                Unit.AnimationSpeed = 1 / attackSpeed * (beginDuration + fullDuration) / fullDuration;
            }
            duration = (duration + beginDuration) * fullDuration / (beginDuration + fullDuration);
            Casting.Set(duration);
            Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "AttackStart,pointDelay:" + duration + ",fullDuration" + fullDuration + ",beginDuration" + beginDuration + ",Time:" + Time.time);
            if (duration == 0)
            {
                Cast();
            }
        }

        if (SkillData.StartEffect != null)
        {
            var ps = EffectManager.Instance.GetEffect(SkillData.StartEffect.Value);
            //ps.transform.position = Unit.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.StartEffect.Value).BindPoint);
            ps.Init(Unit, Unit, Unit.Position, Unit.Direction, lastSpeed);
            //ps.transform.localScale = new Vector3(Unit.TargetScaleX, 1, 1);
            //ps.Play();
        }
    }

    /// <summary>
    /// 实际生效点
    /// </summary>
    public virtual void Cast()
    {
        if (SkillData.RegetTarget) FindTarget();//对于某些技能，无法攻击到已经离开攻击区域的单位

        if (Targets.Count > 0)
        {
            foreach (var t in Targets.ToArray()) Effect(t);
        }
        CastExSkill();
        if (SkillData.BurstCount > 0)
        {
            Burst();
        }
        Targets.Clear();
        if (SkillData.CastEffect != null)
        {
            var ps = EffectManager.Instance.GetEffect(SkillData.CastEffect.Value);
            //ps.transform.position = Unit.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.CastEffect.Value).BindPoint);
            ps.Init(Unit, Unit, Unit.Position, Unit.Direction, lastSpeed);
            //ps.transform.localScale = new Vector3(Unit.TargetScaleX, 1, 1);
            //ps.Play();
        }
    }

    protected virtual void CastExSkill()
    {
        if (SkillData.ExSkills != null)
            foreach (var skillId in SkillData.ExSkills)
            {
                Unit.Skills.Find(x => x.Id == skillId).Start();
            }
    }

    protected virtual void Burst()
    {
        if (BurstCount == -1)
        {
            BurstCount = SkillData.BurstCount;
            LastTargets.Clear();
            LastTargets.AddRange(Targets);
        }
        else
        {
            if (SkillData.BurstFind) //当目标为随机时
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

            if (SkillData.BurstDelay > 0)
                Bursting.Set(SkillData.BurstDelay);
            else
                Burst();
    }

    /// <summary>
    /// 技能对一个单位实际生效的效果
    /// </summary>
    /// <param name="target"></param>
    public virtual void Effect(Unit target)
    {
        if (!CanUseTo(target)) return;
        if (SkillData.Bullet == null)
        {
            Hit(target);
        }
        else
        {
            //创建一个子弹
            var startPoint = Unit.UnitModel.GetPoint(SkillData.ShootPoint);
            //Debug.Log($"攻击{target.Config.Name}:{target.Hp} 起点：{startPoint}");
            Battle.CreateBullet(SkillData.Bullet.Value, startPoint, Vector3.zero, target, this);
        }
    }

    /// <summary>
    /// 伤害判定阶段
    /// </summary>
    /// <param name="target"></param>
    public virtual void Hit(Unit target,Bullet bullet=null)
    {
        if (SkillData.HitEffect != null)
        {
            var ps = EffectManager.Instance.GetEffect(SkillData.HitEffect.Value);
            //ps.transform.position = target.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.HitEffect.Value).BindPoint);
            if (bullet != null)
            {
                ps.Init(Unit, target, bullet.TargetPos, bullet.Direction);
                //ps.transform.rotation = bullet.BulletModel.transform.rotation;
                //ps.transform.Rotate(new Vector3(0, 0, 1),90);
            }
            else ps.Init(Unit, target, target.GetHitPoint(), Vector3.zero); //ps.transform.rotation = Quaternion.identity;
            //ps.Play();
        }
        if (SkillData.UseType == SkillUseTypeEnum.自动 && SkillData.MaxPower == 0 && SkillData.ModelAnimation != null)//三个条件判断技能是否为普攻，判断条件存疑
        {
            foreach (var skill in Unit.Skills)
            {
                if (skill.SkillData.PowerType == PowerRecoverTypeEnum.攻击)
                {
                    skill.RecoverPower(1);
                }
            }
        }
        if (SkillData.DamageRate > 0)
        {
            OnAttack(target);
            if (SkillData.AreaRange != 0)
            {
                var targets = Battle.FindAll(target.Position2, SkillData.AreaRange, SkillData.TargetTeam);
                foreach (var t in targets)
                {
                    addBuff(target);
                    if (SkillData.EffectEffect != null)
                    {
                        var ps = EffectManager.Instance.GetEffect(SkillData.EffectEffect.Value);
                        ps.Init(Unit, target, bullet != null ? bullet.Position : Unit.Position, bullet != null ? bullet.Direction : Unit.Direction.ToV3());
                        //ps.transform.position = target.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.EffectEffect.Value).BindPoint);
                        //ps.Play();
                    }
                    t.Damage(GetDamageInfo(target, t == target ? SkillData.AreaMainDamage : SkillData.AreaDamage));
                    if (!SkillData.IfHeal) OnBeAttack(target);
                }
            }
            else
            {
                addBuff(target);
                if (SkillData.EffectEffect != null)
                {
                    var ps = EffectManager.Instance.GetEffect(SkillData.EffectEffect.Value);
                    ps.transform.position = target.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.EffectEffect.Value).BindPoint);
                    ps.Play();
                }
                if (SkillData.IfHeal)
                {
                    target.Heal(GetDamageInfo(target), !SkillData.DamageWithFrameRate);
                }
                else
                {
                    target.Damage(GetDamageInfo(target));
                }
                if (!SkillData.IfHeal) OnBeAttack(target);
            }
        }
        else
        {
            addBuff(target);
        }
        removeBuff(target);
    }

    protected virtual void addBuff(Unit target)
    {
        if (SkillData.Buffs != null)
        {
            if (SkillData.BuffChance == 0 || Battle.Random.NextDouble() < SkillData.BuffChance)
            {
                for (int i = 0; i < SkillData.Buffs.Length; i++)
                {
                    int buffId = SkillData.Buffs[i];
                    target.AddBuff(buffId, this, i);
                }
            }
        }
    }

    protected virtual void removeBuff(Unit target)
    {
        if (SkillData.BuffRemoves != null)
            foreach (var buffId in SkillData.BuffRemoves)
            {
                var buff = target.Buffs.Find(x => x.Id == buffId);
                if (buff != null) buff.Finish();
            }
    }
    #endregion

    public virtual void FindTarget()
    {      
        Targets.Clear();
        Targets.AddRange(GetAttackTarget());
    }

    List<Unit> tempTargets = new List<Unit>();
    public List<Unit> GetAttackTarget()
    {
        tempTargets.Clear();
        if (SkillData.UseEventUser)
        {
            //正在事件当中，技能去取事件目标
            var t = Battle.TriggerDatas.Peek().User;
            if (t != null && CanUseTo(t))
                tempTargets.Add(t);
        }
        if (SkillData.UseEventTarget)
        {
            //正在事件当中，技能去取事件目标
            var t = Battle.TriggerDatas.Peek().Target;
            if (t != null && CanUseTo(t))
                tempTargets.Add(t);
        }
        if (!SkillData.UseEventTarget && !SkillData.UseEventUser)
        {
            if (AttackPoints == null && !SkillData.AttackAreaWithMain)//根据攻击范围进行索敌
            {
                tempTargets.AddRange(Battle.FindAll(Unit.Position2, SkillData.AttackRange * Unit.AttackRange, SkillData.TargetTeam, !SkillData.DeadFind));
            }
            else
            {
                var attackPoints = SkillData.AttackAreaWithMain ? Unit.GetNowAttackSkill().AttackPoints : AttackPoints;
                tempTargets.AddRange(Battle.FindAll(attackPoints, SkillData.TargetTeam, !SkillData.DeadFind));
            }
        }
        orderTargets(tempTargets);
        return tempTargets;
    }

    protected void orderTargets(List<Unit> targets)
    {
        targets.RemoveAll(x => !CanUseTo(x));
        if (targets.Count > 0)
        {
            //首先计算出所有目标的仇恨优先级，然后再选出攻击个数的实际目标
            SortTarget(targets);
            FilterTarget(targets);
        }
    }

    protected virtual void SortTarget(List<Unit> targets)
    {
        targets.RemoveAll(OrderFilter);
        var l = targets.OrderBy(GetSortOrder1).ThenBy(GetSortOrder2).ThenBy(x => x.Hatred()).ToList();
        targets.Clear();
        targets.AddRange(l);
    }

    protected virtual bool OrderFilter(Unit unit)
    {
        switch (SkillData.AttackOrder)
        {
            case AttackTargetOrderEnum.血量升序:
            case AttackTargetOrderEnum.血量未满随机:
            case AttackTargetOrderEnum.血量比例升序:
                return unit.Hp == unit.MaxHp;
            default:
                break;
        }
        return false;
    }

    protected virtual float GetSortOrder1(Unit unit)
    {
        float result = 0;
        switch (SkillData.AttackOrder2)
        {
            case AttackTargetOrder2Enum.近身:
                if (unit is Units.干员 u)
                {
                    if (u.StopUnits.Contains(Unit))
                    {
                        result = -1;
                    }
                }
                break;
            case AttackTargetOrder2Enum.飞行:
                result = -unit.Height;
                break;
            case AttackTargetOrder2Enum.远程:
                result = unit.Skills.Count == 0 ? 0 : -unit.FirstSkill.SkillData.AttackRange;
                Debug.Log($"{unit.UnitData.Id} , {result}");
                break;
            case AttackTargetOrder2Enum.Buff:
                break;
            case AttackTargetOrder2Enum.Tag:
                //firstOrder = x => x.Config.Tags == null ? 0 : -x.Config.Tags.Count();
                break;
            case AttackTargetOrder2Enum.召唤物:
                result = ((Unit as Units.干员).Children.Contains(unit) || unit == Unit) ? 0 : 1;
                break;
            default:
                break;
        }
        return result;
    }

    protected virtual float GetSortOrder2(Unit x)
    {
        float result = 0;
        switch (SkillData.AttackOrder)
        {
            case AttackTargetOrderEnum.无:
                break;
            case AttackTargetOrderEnum.终点距离:
                result = (x as Units.敌人).distanceToFinal();
                break;
            case AttackTargetOrderEnum.血量升序:
                result = x.Hp;
                break;
            case AttackTargetOrderEnum.血量降序:
                result = -x.Hp;
                break;
            case AttackTargetOrderEnum.血量比例升序:
                result = x.Hp / x.MaxHp;
                break;
            case AttackTargetOrderEnum.血量比例降序:
                result = -x.Hp / x.MaxHp;
                break;
            case AttackTargetOrderEnum.放置降序:
                result = -(x as Units.干员).InputTime;
                break;
            case AttackTargetOrderEnum.区域顺序:
                result = Math.Abs(x.Position2.x - Unit.Position2.x) + Math.Abs(x.Position2.y - Unit.Position2.y);
                break;
            case AttackTargetOrderEnum.防御降序:
                result = -x.Defence;
                break;
            case AttackTargetOrderEnum.防御升序:
                result = x.Defence;
                break;
            case AttackTargetOrderEnum.攻击力升序:
                result = x.Attack;
                break;
            case AttackTargetOrderEnum.攻击力降序:
                result = -x.Attack;
                break;
            case AttackTargetOrderEnum.自身距离升序:
                result = (x.Position - Unit.Position).magnitude;
                break;
            case AttackTargetOrderEnum.自身距离降序:
                result = -(x.Position - Unit.Position).magnitude;
                break;
            case AttackTargetOrderEnum.血量未满随机:
                result = Battle.Random.Next(0, 1000);
                break;
            case AttackTargetOrderEnum.重量升序:
                result = x.Weight;
                break;
            case AttackTargetOrderEnum.重量降序:
                result = -x.Weight;
                break;
            case AttackTargetOrderEnum.随机:
                result = Battle.Random.Next(0, 1000);
                break;
            case AttackTargetOrderEnum.隐身优先:
                result = x.IfHide ? 0 : 1;
                break;
            case AttackTargetOrderEnum.未眩晕优先:
                result = x.State == StateEnum.Stun ? 1 : 0;
                break;
            case AttackTargetOrderEnum.飞行优先:
                result = -x.Height;
                break;
            case AttackTargetOrderEnum.未阻挡优先:
                result = (x as Units.敌人).StopUnit == null ? 0 : 1;
                break;
            case AttackTargetOrderEnum.沉睡优先:
                throw new Exception();
                break;
            case AttackTargetOrderEnum.无抵抗优先:
                throw new Exception();
                break;
        }
        return result;
    }

    protected virtual void FilterTarget(List<Unit> targets)
    {
        if (SkillData.DamageCount != 0)
        {
            int targetCount = GetTargetCount();
            for (int i = targets.Count() - 1; i >= targetCount; i--)
            {
                targets.RemoveAt(i);
            }
        }
    }

    protected virtual int GetTargetCount()
    {
        int result = SkillData.DamageCount;
        foreach (var modify in Modifies)
        {
            if (modify is ITargetModify targetModify)
            {
                result = targetModify.Modify(result);
            }
        }
        return result;
    }

    public void UpdateAttackPoints()
    {
        if (AttackPoints == null) return;
        AttackPoints.Clear();
        foreach (var p in SkillData.AttackPoints)
        {
            var point = Unit.PointWithDirection(p);
            if (point.x < 0 || point.x >= Battle.Map.Tiles.GetLength(0) || point.y < 0 || point.y >= Battle.Map.Tiles.GetLength(1)) continue;
            AttackPoints.Add(point);
        }
    }

    public void BreakCast()
    {
        Targets.Clear();
        if (Unit.AttackingSkill == this)
        {
            Unit.AttackingSkill = null;
            Unit.AttackingAction.Finish();
        }
        Unit.UnitModel?.BreakAnimation();
        Casting.Finish();
        Bursting.Finish();
        //Opening.Finish();
        BurstCount = -1;
    }

    protected virtual void OnOpenEnd()
    {
        Unit.OverWriteAnimation = null;
        if (SkillData.UpgradeSkill != null)
        {
            DoUpgrade(SkillData.UpgradeSkill.Value);
        }
        if (LoopStartEffect != null)
        {
            EffectManager.Instance.ReturnEffect(LoopStartEffect);
            LoopStartEffect = null;
        }
    }

    protected virtual void OnAttack(Unit target)
    {
        Battle.TriggerDatas.Push(new TriggerData()
        {
            Target = target,
            Skill = this,
        });
        Unit.Trigger(TriggerEnum.攻击);
        Battle.TriggerDatas.Pop();
    }

    protected virtual void OnBeAttack(Unit target)
    {
        Battle.TriggerDatas.Push(new TriggerData()
        {
            User = Unit,
            Target = target,
            Skill = this,
        });
        target.Trigger(TriggerEnum.被击);
        Battle.TriggerDatas.Pop();
    }

    protected virtual void OnSkillOpen()
    {
        Battle.TriggerDatas.Push(new TriggerData()
        {
            Skill = this,
        });
        Unit.Trigger(TriggerEnum.释放技能);
        Battle.TriggerDatas.Pop();
    }

    protected DamageInfo GetDamageInfo(Unit target, float damageRate=1)
    {
        var cooldown = SkillData.Cooldown;
        if (cooldown < SystemConfig.DeltaTime) cooldown = SystemConfig.DeltaTime;
        var result = new DamageInfo()
        {
            Target = target,
            AllCount = tempTargets.Count,
            Source = this,
            DamageRate = damageRate * SkillData.DamageRate * (SkillData.DamageWithFrameRate ? cooldown : 1),
            DamageType = SkillData.DamageType,
        };
        switch (SkillData.DamageBase)
        {
            case 0:
                result.Attack = Unit.Attack;
                break;
            case 1:
                result.Attack = target.MaxHp;
                break;
            case 2:
                result.Attack = result.DamageRate;
                result.DamageRate = 1;
                break;
        }
        foreach (var buff in target.Buffs)
        {
            if (buff is IDamageModify damageModify)
            {
                damageModify.Modify(result);
            }
        }
        foreach (var modify in Modifies)
        {
            if (modify is IDamageModify damageModify)
            {
                damageModify.Modify(result);
            }
        }
        return result;
    }

    public void DoUpgrade(int skillId)
    {
        if (StartId == -1) StartId = Id;
        Id = skillId;
        Modifies.Clear();
        if (SkillData.Modifys != null)
        {
            for (int i = 0; i < SkillData.Modifys.Length; i++)
            {
                Modifies.Add(ModifyManager.Instance.Get(SkillData.Modifys[i]));
            }
        }
        else
        {

        }

        if (SkillData.AttackPoints != null)
        {
            AttackPoints = new List<Vector2Int>();
            UpdateAttackPoints();
        }

        MaxPowerBase = SkillData.MaxPower;
        PowerCount = SkillData.PowerCount;
        Reset();
    }
    public void Finish()
    {
        if (ReadyEffect != null)
        {
            EffectManager.Instance.ReturnEffect(ReadyEffect);
            ReadyEffect = null;
        }
        if (LoopStartEffect != null)
        {
            EffectManager.Instance.ReturnEffect(LoopStartEffect);
            LoopStartEffect = null;
        }
        if (LoopCastEffect != null)
        {
            EffectManager.Instance.ReturnEffect(LoopCastEffect);
            LoopCastEffect = null;
        }
    }
}

