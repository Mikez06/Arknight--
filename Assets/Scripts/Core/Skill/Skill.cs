using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public Unit Unit;

    protected Modify[] Modifies;

    public List<Unit> Targets = new List<Unit>();
    protected Battle Battle => Unit.Battle;

    public SkillData SkillData => Database.Instance.Get<SkillData>(Id);

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
    public float MaxPower;
    public int PowerCount;
    public int UseCount;

    Effect ReadyEffect;

    public virtual void Init()
    {
        if (SkillData.Modifys != null)
        {
            Modifies = new Modify[SkillData.Modifys.Length];
            for (int i = 0; i < SkillData.Modifys.Length; i++)
            {
                Modifies[i] = ModifyManager.Instance.Get(SkillData.Modifys[i]);
            }
        }
        else
        {
            Modifies = new Modify[0];
        }

        if (SkillData.AttackPoints != null)
        {
            AttackPoints = new List<Vector2Int>();
            UpdateAttackPoints();
        }

        MaxPower = SkillData.MaxPower;
        PowerCount = SkillData.PowerCount;
        Reset();
    }

    public virtual void Reset()
    {
        Power = SkillData.StartPower;
        BreakCast();
        Cooldown.Finish();
    }

    public virtual void Update()
    {
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

    public bool Useable()
    {
        if (this == Unit.Skills[0] && !Unit.CanAttack)
        {
            //Debug.Log($"{Unit.UnitData.Id} 因为缴械无法使用{SkillData.Id}");
            return false;
        }
        if (SkillData.StopBreak && Unit.IfStoped()) return false;
        if (!Cooldown.Finished()) return false;
        return true;
    }

    public bool CanUseTo(Unit target)
    {
        if (target == null) return false;
        if (!target.IfSelectable) return false;
        if (SkillData.SelfOnly && target != Unit) return false;
        if ((SkillData.TargetTeam >> target.Team) % 2 == 0) return false;
        if (SkillData.ProfessionLimit != UnitTypeEnum.无 && SkillData.ProfessionLimit != target.UnitData.Profession) return false;
        if (!SkillData.AttackFly && target.Height > 0) return false;
        if (!target.Alive()) return false;
        if (!SkillData.AntiHide && target.IfHide) return false;
        return true;
    }

    public virtual void UpdateCooldown()
    {
        if (!Opening.Finished())
        {
            if (Opening.Update(SystemConfig.DeltaTime))
            {
                Unit.OverWriteAnimation = null;
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
        if (Unit.IfStun && SkillData.UseType != SkillUseTypeEnum.被动) 
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

        if (SkillData.EnableBuff != null && !SkillData.EnableBuff.All(x => Unit.Buffs.Any(y => y.Id == x))) 
            return false;
        if (SkillData.DisableBuff != null && SkillData.DisableBuff.Any(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;

        if (SkillData.StopBreak && Unit.IfStoped()) return false;

        if (SkillData.AttackMode == AttackModeEnum.跟随攻击 && Unit.AttackingSkill != null) return false;

        if (SkillData.UseType == SkillUseTypeEnum.手动) //手动技能在技能开启时可以使用
            return !Opening.Finished();
        //自动充能技在有充能时才能使用
        if (SkillData.MaxPower > 0 && SkillData.UseType == SkillUseTypeEnum.自动 && Power < MaxPower) return false;
        //不管什么技能 都要遵循技能CD
        return Cooldown.Finished();
    }

    public virtual bool InAttackUsing()
    {
        if (SkillData.EnableBuff != null && !SkillData.EnableBuff.All(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;
        if (SkillData.DisableBuff != null && SkillData.DisableBuff.Any(x => Unit.Buffs.Any(y => y.Id == x)))
            return false;
        if (SkillData.AttackPoints == null) return false;
        if (SkillData.ReadyType == SkillReadyEnum.特技激活&&!Opening.Finished())
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

    public void RecoverPower(float count, bool withTip = false)
    {
        if (withTip)
        {
            Unit.UnitModel.ShowPower(count);
        }
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
        if (SkillData.ReadyType == SkillReadyEnum.充能释放 && SkillData.UseType == SkillUseTypeEnum.手动)
        {
            var target = getAttackTarget();
            if (target.Count == 0)
            {
                Log.Debug($"技能{SkillData.Name} 无可选目标，使用失败");
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
        Unit.OverWriteAnimation = SkillData.OverwriteAnimation;
        OnSkillOpen();
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
            if (Targets.Count == 1 && Targets[0] != Unit && SkillData.ModelAnimation != null)//默认不填动作的技能不需要转身
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
        if (SkillData.MaxUseCount != 0 && UseCount >= SkillData.MaxUseCount) return;//使用次数不在ready里判断，因为被动技能不会走ready的逻辑

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
            var duration = Unit.UnitModel.GetSkillDelay(SkillData.ModelAnimation, Unit.GetAnimation(), out float fullDuration, out float beginDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
            float attackSpeed = 1f / Unit.Agi * 100;//攻速影响冷却时间
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
            Unit.AttackingAction.Set(fullDuration);
            Unit.State = StateEnum.Attack;
            Unit.AnimationName = SkillData.ModelAnimation;
            Unit.AttackingSkill = this;
            //Debug.Log(SkillData.ModelAnimation);
            Unit.UnitModel?.BreakAnimation();
            Unit.AnimationSpeed = 1 / attackSpeed * (beginDuration + fullDuration) / fullDuration;
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
            ps.transform.position = Unit.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.StartEffect.Value).BindPoint);
            ps.transform.localScale = new Vector3(Unit.TargetScaleX, 1, 1);
            ps.Play();
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
            foreach (var t in Targets) Effect(t);
        }
        CastExSkill();
        if (SkillData.BurstCount > 0)
        {
            Burst();
        }
        Targets.Clear();
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
                LastTargets.AddRange(getAttackTarget());
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
    public virtual void Hit(Unit target)
    {
        if (SkillData.HitEffect != null)
        {
            var ps = EffectManager.Instance.GetEffect(SkillData.HitEffect.Value);
            ps.transform.position = target.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.HitEffect.Value).BindPoint);
            ps.Play();
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
                    if (SkillData.EffectEffect != null)
                    {
                        var ps = EffectManager.Instance.GetEffect(SkillData.EffectEffect.Value);
                        ps.transform.position = target.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.EffectEffect.Value).BindPoint);
                        ps.Play();
                    }
                    t.Damage(GetDamageInfo(target, t != target));
                    OnBeAttack(target);
                }
            }
            else
            {
                if (SkillData.EffectEffect != null)
                {
                    var ps = EffectManager.Instance.GetEffect(SkillData.EffectEffect.Value);
                    ps.transform.position = target.UnitModel.GetPoint(Database.Instance.Get<EffectData>(SkillData.EffectEffect.Value).BindPoint);
                    ps.Play();
                }
                if (SkillData.IfHeal)
                {
                    target.Heal(GetDamageInfo(target));
                }
                else
                {
                    target.Damage(GetDamageInfo(target));
                }
                OnBeAttack(target);
            }
        }
        addBuff(target);
    }

    protected virtual void addBuff(Unit target)
    {
        if (SkillData.Buffs != null)
            foreach (var buffId in SkillData.Buffs)
            {
                target.AddBuff(buffId, this);
            }
    }
    #endregion

    public virtual void FindTarget()
    {      
        Targets.Clear();
        Targets.AddRange(getAttackTarget());
    }

    List<Unit> tempTargets = new List<Unit>();
    protected List<Unit> getAttackTarget()
    {
        tempTargets.Clear();
        if (SkillData.UseEventTarget)
        {
            //正在事件当中，技能去取事件目标
            var t = Battle.TriggerDatas.Peek().Target;
            if (t != null && CanUseTo(t))
                tempTargets.Add(t);
        }
        else //if (tempTargets.Count == 0)
        {
            if (AttackPoints == null&&!SkillData.AttackAreaWithMain)//根据攻击范围进行索敌
            {
                tempTargets.AddRange(Battle.FindAll(Unit.Position2, SkillData.AttackRange, SkillData.TargetTeam));
            }
            else
            {
                var attackPoints = SkillData.AttackAreaWithMain ?Unit.GetNowAttackSkill().AttackPoints : AttackPoints;
                tempTargets.AddRange(Battle.FindAll(attackPoints, SkillData.TargetTeam));
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
                    if (u.StopUnits.Contains(unit))
                    {
                        result += 1;
                    }
                }
                break;
            case AttackTargetOrder2Enum.飞行:
                result = -unit.Height;
                break;
            case AttackTargetOrder2Enum.远程:
                result = unit.Skills.Count == 0 ? 0 : -unit.Skills[0].SkillData.AttackRange;
                Debug.Log($"{unit.UnitData.Id} , {result}");
                break;
            case AttackTargetOrder2Enum.Buff:
                break;
            case AttackTargetOrder2Enum.Tag:
                //firstOrder = x => x.Config.Tags == null ? 0 : -x.Config.Tags.Count();
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
                result = -(x.Position - Unit.Position).magnitude;
                break;
            case AttackTargetOrderEnum.自身距离降序:
                result = (x.Position - Unit.Position).magnitude;
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
            Debug.Log($"{SkillData.Id} ,{targets.Count}");
            int targetCount = GetTargetCount();
            for (int i = targets.Count() - 1; i >= targetCount; i--)
            {
                targets.RemoveAt(i);
            }
            Debug.Log($"{SkillData.Id} :{targets.Count}");
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
        if (Unit.AttackingSkill == this) Unit.AttackingSkill = null;
        Unit.UnitModel?.BreakAnimation();
        Casting.Finish();
        Bursting.Finish();
        BurstCount = -1;
    }

    protected virtual void OnOpenEnd()
    {

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

    protected DamageInfo GetDamageInfo(Unit target, bool IfAOE = false)
    {
        var result = new DamageInfo()
        {
            Target = target,
            Source = this,
            Attack = SkillData.BaseOnMaxHp ? target.MaxHp : Unit.Attack,
            DamageRate = IfAOE ? SkillData.AreaDamage : SkillData.DamageRate,
            DamageType = SkillData.DamageType,
        };
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
}

