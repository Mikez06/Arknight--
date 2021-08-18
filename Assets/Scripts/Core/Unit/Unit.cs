using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public static string[] DefaultAnimation = new string[] { "Default" };
    public static string[] StartAnimation = new string[] { "Start" };
    public Battle Battle;
    public UnitData UnitData => Database.Instance.Get<UnitData>(Id);
    public int Id;

    public UnitModel UnitModel;
    public BattleUI.UI_BattleUnit uiUnit;

    public Vector3 Position;

    public float Height;
    
    public Vector2 Position2 => new Vector2(Position.x, Position.z);

    public Vector2Int GridPos => new Vector2Int(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z));

    public Vector2 Direction = new Vector2(1, 0);

    public Tile NowGrid => Battle.Map.Tiles[Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z)];

    public StateEnum State = StateEnum.Default;
    public float Hp;

    public List<Skill> Skills = new List<Skill>();
    public Skill MainSkill;

    public List<Buff> Buffs = new List<Buff>();

    public float MaxHp;
    public float HpBase, HpAdd, HpRate, HpAddFin, HpRateFin;
    /// <summary>
    /// 攻速
    /// </summary>
    public float Agi;
    public float AgiBase, AgiAdd, AgiRate, AgiAddFin, AgiRateFin;
    /// <summary>
    /// 移速
    /// </summary>
    public float Speed;
    public float SpeedBase, SpeedRate, SpeedAdd;

    public float AttackGap;
    public float AttackGapBase, AttackGapAdd, AttackGapRate;

    public float Attack;
    public float AttackBase,AttackRate, AttackAdd, AttackRateFin, AttackAddFin;

    public float Defence;
    public float DefenceBase, DefenceRate, DefenceAdd, DefenceRateFin, DefenceAddFin;

    public float MagicDefence;
    public float MagicDefenceBase, MagicDefenceRate, MagicDefenceAdd, MagicDefenceRateFin, MagicDefenceAddFin;

    public float PowerSpeed;

    public float Team;

    public int Weight;
    public int WeightBase, WeightAdd;

    public bool IfHide;
    public bool IfHideAnti;
    protected bool hideBase;

    public bool CanAttack;

    public bool IfAlive = true;

    /// <summary>
    /// 攻击动画
    /// </summary>
    public CountDown Attacking = new CountDown();

    public Skill AttackingSkill;

    /// <summary>
    /// 死亡动画
    /// </summary>
    public CountDown Dying = new CountDown();
    /// <summary>
    /// 硬直
    /// </summary>
    //public CountDown Recover = new CountDown();

    public bool IfStun; 

    public float ScaleX = -1;
    public float TargetScaleX = -1;

    public string[] AnimationName = Unit.DefaultAnimation;
    public string[] OverWriteAnimation;
    public float AnimationSpeed = 1;

    public virtual void Init()
    {
        baseAttributeInit();
        if (UnitData.Skills != null)
            foreach (var skillId in UnitData.Skills)
            {
                LearnSkill(skillId);
            }
        CreateModel();
        Refresh();
        Hp = MaxHp;
    }

    protected virtual void baseAttributeInit()
    {
        SpeedBase = UnitData.Speed;
        HpBase = UnitData.Hp;
        AttackBase = UnitData.Attack;
        DefenceBase = UnitData.Defence;
        MagicDefenceBase = UnitData.MagicDefence;
        WeightBase = UnitData.Weight;
        PowerSpeed = 1f;
        AgiBase = 100;
        AttackGapBase = 0;
        Height = UnitData.Height;
    }

    public virtual void Refresh()
    {
        SpeedAdd = SpeedRate = 0;
        HpAdd = HpRate = HpAddFin = HpRateFin = 0;
        AttackAdd = AttackRate = AttackAddFin = AttackRateFin = 0;
        MagicDefenceAdd = MagicDefenceRate = MagicDefenceAddFin = MagicDefenceRateFin = 0;
        DefenceAdd = DefenceRate = DefenceAddFin = DefenceRateFin = 0;
        AgiAdd = AgiRate = AgiAddFin = AgiRateFin = 0;
        WeightAdd = 0;
        PowerSpeed = 1f;
        AttackGapAdd = AttackGapRate = 0;
        CanAttack = true;
        foreach (var buff in Buffs)
        {
            buff.Apply();
        }
        Speed = (SpeedBase + SpeedAdd) * (1 + SpeedRate);
        if (Speed < 0) Speed = 0;
        MaxHp= ((HpBase + HpAdd) * (1 + HpRate) + HpAddFin) * (1 + HpRateFin);
        Attack = ((AttackBase + AttackAdd) * (1 + AttackRate) + AttackAddFin) * (1 + AttackRateFin);
        Defence = ((DefenceBase + DefenceAdd) * (1 + DefenceRate) + DefenceAddFin) * (1 + DefenceRateFin);
        MagicDefence = ((MagicDefenceBase + MagicDefenceAdd) * (1 + MagicDefenceRate) + MagicDefenceAddFin) * (1 + MagicDefenceRateFin);
        Agi= ((AgiBase + AgiAdd) * (1 + AgiRate) + AgiAddFin) * (1 + AgiRateFin);
        if (MagicDefence < 0) MagicDefence = 0;
        Weight = WeightBase + WeightAdd;
        AttackGap = (AttackGapBase + AttackGapAdd) * (1 + AttackGapRate);
    }

    public void UpdateBuffs()
    {
        if (!Alive()) return;
        IfHide = hideBase;
        IfHideAnti = false;
        bool lastIfStun = IfStun;
        IfStun = false;
        foreach (var buff in Buffs.Reverse<Buff>())
        {
            buff.Update();
        }
        if (unbalance) IfStun = true;
        if (IfStun)
        {
            SetStatus(StateEnum.Stun);
        }
        if (lastIfStun && !IfStun)
        {
            SetStatus(StateEnum.Idle);
        }
        if (IfHideAnti || IfStoped()) IfHide = false;
    }
    public virtual void UpdateAction()
    {
        
    }

    protected void UpdateDie()
    {
        if (Dying.Update(SystemConfig.DeltaTime))
        {
            Finish();
        }
    }

    public virtual void DoDie(object source)
    {
        IfAlive = false;
        SetStatus(StateEnum.Die);
        Dying.Set(UnitModel.GetAnimationDuration("Die"));

        //根据伤害来源，判断击杀事件
        if (source is DamageInfo damageInfo)
        {
            if (damageInfo.Source is Skill skill)
            {
                Battle.TriggerDatas.Push(new TriggerData()
                {
                    Target = this,
                    Skill = skill,
                });
                skill.Unit.Trigger(TriggerEnum.击杀);
                Battle.TriggerDatas.Pop();
            }
        }

        //死亡事件
        Battle.TriggerDatas.Push(new TriggerData()
        {
            Target = this,
        });
        Trigger(TriggerEnum.死亡);
        Battle.TriggerDatas.Pop();
    }

    public virtual void Finish()
    {
        Battle.TriggerDatas.Push(new TriggerData()
        {
            Target = this,
        });
        Trigger(TriggerEnum.离场);
        Battle.TriggerDatas.Pop();
    }

    protected void UpdateSkills()
    {
        var inAttack = !Attacking.Finished();
        Attacking.Update(SystemConfig.DeltaTime);
        if (inAttack && Attacking.Finished())
        {
            Debug.Log("Ready to attack");
        }
        foreach (var skill in Skills)
        {
            skill.UpdateCooldown();
        }
        for (int i = Skills.Count - 1; i >= 0; i--)
        {
            if (i >= Skills.Count) continue;
            var sk = Skills[i];
            if (sk != null)
            {
                sk.Update();
            }
        }
        if (inAttack && Attacking.Finished())
        {
            SetStatus(StateEnum.Idle);
        }
    }

    protected virtual void UpdateMove()
    {

    }

    public void Trigger(TriggerEnum triggerEnum)
    {
        foreach (var skill in Skills)
        {
            if (triggerEnum == TriggerEnum.被击 && skill.SkillData.PowerType == PowerRecoverTypeEnum.受击)
            {
                skill.RecoverPower(1);
            }
            if (skill.SkillData.Trigger == triggerEnum)
            {
                skill.Start();
            }
        }
    }

    public Skill LearnSkill(int skillId)
    {
        var s = Skills.Find(x => x.Id == skillId);
        if (s != null) return s;
        var skillConfig = Database.Instance.Get<SkillData>(skillId);
        var skill = typeof(Unit).Assembly.CreateInstance(nameof(Skills) + "." + skillConfig.Type) as Skill;
        skill.Unit = this;
        skill.Id = skillId;
        skill.Init();
        if (Skills.Count > 0 && skillId < Skills.Last().Id)
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                if (Skills[i].Id > skillId)
                {
                    Skills.Insert(i, skill);
                    break;
                }
            }
        }
        else
            Skills.Add(skill);
        if (skillConfig.ExSkills != null)
            foreach (var id in skillConfig.ExSkills)
            {
                LearnSkill(id);
            }
        return skill;
    }

    public Buff AddBuff(int buffId,Skill source)
    {
        //权且加上来源判断，因为现在很多buff共用一个id会产生冲突。
        //如果需要处理buff冲突的情况，再修改这里
        var oldBuff = Buffs.FirstOrDefault(x => x.Id == buffId && x.Skill == source);
        if (oldBuff != null)
        {
            oldBuff.Reset();
            return oldBuff;
        }
        else
        {
            var config = Database.Instance.Get<BuffData>(buffId);
            var buff = typeof(Buff).Assembly.CreateInstance(nameof(Buffs) + "." + config.Type) as Buff;
            buff.Id = buffId;
            buff.Skill = source;
            buff.Unit = this;
            Buffs.Add(buff);
            buff.Init();
            Refresh();
            return buff;
        }
    }

    public void AddBuff(Buff buff)
    {
        Buffs.Add(buff);
        buff.Unit = this;
        Refresh();
    }

    public void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
        Refresh();
    }
    #region 推拉相关
    public List<IPushBuff> PushBuffs = new List<IPushBuff>();

    /// <summary>
    /// 失衡硬直
    /// </summary>
    public CountDown Unbalancing = new CountDown();

    public bool Unbalance => unbalance || !Unbalancing.Finished();

    protected bool unbalance;

    public void UpdatePush()
    {
        if (!Alive()) return;
        Unbalancing.Update(SystemConfig.DeltaTime);
        foreach (Buff buff in PushBuffs.Reverse<IPushBuff>())
        {
            buff.Update();
        }
        if (!Unbalance) return;
        Vector2 power = Vector2.zero;
        foreach (var buff in PushBuffs.ToList())
        {
            var pushPower= buff.GetPushPower();
            power += pushPower;
        }
        if (power.magnitude < 0.1f) //力太小，失衡状态结束
        {
            unbalance = false;
        }
        else
        {
            Unbalancing.Set(0.1f);
        }
        if (Unbalance)
        {
            var posChange = power * SystemConfig.DeltaTime;
            Position += new Vector3(posChange.x, 0, posChange.y);
        }
        if (!Unbalance)
        {
            RecoverBalance();
        }
    }

    protected virtual void RecoverBalance()
    {

    }

    public void AddPush(IPushBuff buff)
    {
        (buff as Buff).Unit = this;
        if (PushBuffs.Count == 0)//进入失衡状态
        {
            unbalance = true;
            BreakAllCast();            
            //SetStatus(StateEnum.Stun);
            Unbalancing.Set(0.1f);
        }
        PushBuffs.Add(buff);
    }

    public void RemovePush(IPushBuff buff)
    {
        PushBuffs.Remove(buff);
    }
    #endregion
    public void RecoverPower(float count)
    {
        if (!MainSkill.Opening.Finished())
            return;
        foreach (var skill in Skills)
        {
            skill.RecoverPower(count);
        }
    }

    public bool Alive()
    {
        return IfAlive;
    }

    public void SetStatus(StateEnum state)
    {
        if (State != state)
        {
            Debug.Log($"{UnitData.Id}从 {State} 变为 {state}");
        }
        this.State = state;
        if (state == StateEnum.Default)
            AnimationName = Unit.DefaultAnimation;
        else if (state == StateEnum.Idle)
            AnimationName = UnitData.IdleAnimation;
        else if (state == StateEnum.Move)
            AnimationName = UnitData.MoveAnimation;
        else if (state == StateEnum.Start)
            AnimationName = Unit.StartAnimation;
        AnimationSpeed = 1;
        if (state != StateEnum.Attack && !Attacking.Finished())
        {
            Attacking.Finish();
        }
    }

    public void CreateModel()
    {
        GameObject go = ResHelper.Instantiate(PathHelper.UnitPath + UnitData.Model);
        UnitModel = go.GetComponent<UnitModel>();
        UnitModel.Init(this);
    }

    public void Heal(DamageInfo heal)
    {
        Hp += heal.Attack;
        UnitModel.ShowHeal(heal);
        if (Hp > MaxHp)
            Hp = MaxHp;
    }

    public void Damage(DamageInfo damageInfo)
    {
        float damage = damageInfo.Attack * damageInfo.DamageRate;
        damage = damageWithDefence(damage, damageInfo.DamageType);
        damageInfo.FinalDamage = damage;
        float damageEx = damageInfo.Attack;
        damageEx = damageWithDefence(damageEx, damageInfo.DamageType);
        if (damage > damageEx * 1.5f) UnitModel.ShowCrit(damageInfo);
        if (!damageInfo.Avoid)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                Hp = 0;
                DoDie(damageInfo);
            }
        }
    }

    float damageWithDefence(float baseDamage,DamageTypeEnum damageType)
    {
        switch (damageType)
        {
            case DamageTypeEnum.Normal:
                baseDamage -= Defence;
                if (baseDamage < 0) baseDamage = 1;
                break;
            case DamageTypeEnum.Magic:
                baseDamage = baseDamage * (100 - MagicDefence) / 100;
                break;
        }
        return baseDamage;
    }

    public Skill GetNowAttackSkill()
    {
        for (int i = Skills.Count - 1; i >= 0; i--)
        {
            if (Skills[i].InAttackUsing())
            {
                return Skills[i];
            }
        }
        throw new Exception($"{UnitData.Id}没有普攻技能");
    }

    public virtual float Hatred()
    {
        return UnitData.Hatred * 1000;
    }

    public void BreakAllCast()
    {
        Attacking.Finish();
        foreach (var skill in Skills)
        {
            skill.BreakCast();
        }
    }

    public virtual bool IfStoped()
    {
        return false;
    }

    public string[] GetAnimation()
    {
        return OverWriteAnimation == null ? AnimationName : OverWriteAnimation;
    }
}
