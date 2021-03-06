using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Unit
{
    public static string[] StartAnimation = new string[] { "Start" };
    public static string[] DieAnimation = new string[] { "Die" };
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
    public List<IShield> Shields = new List<IShield>();
    public List<int> IgnoreBuffs = new List<int>();

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

    public float AllBlock,Block, MagBlock;

    public float PowerSpeed, PowerSpeedAdd;

    public float HpRecoverP;
    public float HpRecover;
    public float HpRecoverBase, HpRecoverAdd, HpRecoverRate;

    public int Team;

    public int Weight;
    public float WeightBase, WeightAdd;

    public float SkillCost;
    public float SkillCostAdd;

    public float Resist;
    public float ResistAdd;

    public float AttackRange;
    public float AttackRangeAdd, AttackRangeRate;

    public int StopCount;
    public float StopCountAdd;

    public bool IfHide;
    public bool IfHideAnti;
    protected bool hideBase;

    public bool CanAttack;
    public bool CanStopOther;
    public float Hatre;

    public bool IfAlive = true;

    public bool IfSleep = false;
    public bool IfSelectable = true;//能否被技能指定为目标
    public bool CanBeHeal = false;
    public List<int> HealOnly = new List<int>();//绝食状态下，依旧可被这些单位治疗

    public float DamageReceiveRate, MagicDamageReceiveRate, HealReceiveRate;

    public float PushPower;

    public CountDown Start = new CountDown();//入场
    /// <summary>
    /// 攻击动画
    /// </summary>
    public CountDown AttackingAction = new CountDown();

    public Skill FirstSkill;
    public Skill AttackingSkill;

    public float NervePoint;
    public CountDown ElementProtect = new CountDown();

    public CountDown LifeTime;
    /// <summary>
    /// 死亡动画
    /// </summary>
    public CountDown Dying = new CountDown();
    /// <summary>
    /// 硬直
    /// </summary>
    //public CountDown Recover = new CountDown();

    public List<Units.敌人> StopUnits = new List<Units.敌人>();

    public bool IfStun;

    public float ScaleX = -1;
    public float TargetScaleX = -1;

    public string[] AnimationName;
    public string[] OverWriteAnimation;
    public string[] OverWriteIdle;
    public string[] OverWriteMove;
    public string[] OverWriteDie;
    public bool CanChangeAnimation = true;
    public float AnimationSpeed = 1;

    public virtual void Init()
    {
        baseAttributeInit();
        AnimationName = UnitData.DefaultAnimation;
        if (UnitData.IgnoreBuff != null) IgnoreBuffs.AddRange(UnitData.IgnoreBuff);
        Team = UnitData.Team;
        if (UnitData.Skills != null)
            for (int i = 0; i < UnitData.Skills.Length; i++)
            {
                int skillId = UnitData.Skills[i];
                var skill= LearnSkill(skillId);
                if (i == 0) FirstSkill = skill;
            }
        if (UnitData.LifeTime != 0) LifeTime=new CountDown(UnitData.LifeTime);
        CreateModel();
        Refresh();
        Hp = MaxHp;
    }

    protected virtual void baseAttributeInit()
    {
        SpeedBase = UnitData.Speed;
        HpBase = UnitData.Hp + UnitData.HpEx;
        AttackBase = UnitData.Attack + UnitData.AttackEx;
        DefenceBase = UnitData.Defence + UnitData.DefenceEx;
        MagicDefenceBase = UnitData.MagicDefence + UnitData.MagicDefenceEx;
        WeightBase = UnitData.Weight;
        PowerSpeed = 1f;
        AgiBase = 100 + UnitData.ExAgi;
        AttackGapBase = UnitData.AttackGap;
        Height = UnitData.Height;
        if (Battle.MapData.UnitOvDatas != null)
        {
            var ovInfo = Battle.MapData.UnitOvDatas.Find(x => x.UnitId == UnitData.Id);
            if (ovInfo != null)
            {
                HpBase = ovInfo.Hp;
                AttackBase = ovInfo.Atk;
                DefenceBase = ovInfo.Def;
                MagicDefenceBase = ovInfo.MagDef;
                AgiBase += ovInfo.Agi;               
                if (ovInfo.Speed != 0)
                    SpeedBase = ovInfo.Speed;
            }
        }
    }

    public virtual void Refresh()
    {
        float hpDown = MaxHp - Hp;
        PushPower = 0;
        SpeedAdd = SpeedRate = 0;
        HpAdd = HpRate = HpAddFin = HpRateFin = 0;
        AttackAdd = AttackRate = AttackAddFin = AttackRateFin = 0;
        MagicDefenceAdd = MagicDefenceRate = MagicDefenceAddFin = MagicDefenceRateFin = 0;
        DefenceAdd = DefenceRate = DefenceAddFin = DefenceRateFin = 0;
        AgiAdd = AgiRate = AgiAddFin = AgiRateFin = 0;
        HpRecoverP = 0;
        HpRecoverBase = UnitData.HpRecover;
        HpRecoverAdd = 0;
        WeightAdd = 0;
        AttackGapAdd = AttackGapRate = 0;
        Block = MagBlock = 0;
        SkillCostAdd = 0;
        PowerSpeedAdd = 0;
        CanAttack = true;
        CanBeHeal = true;
        ResistAdd = 0;
        AttackRangeAdd = AttackRangeRate = 0;
        DamageReceiveRate = MagicDamageReceiveRate = HealReceiveRate = 1;
        StopCountAdd = 0;
        HpRecoverRate = 0;
        Hatre = UnitData.Hatred;
        foreach (var buff in Buffs)
        {
            if (buff.Enable()) buff.Apply();
        }
        StopCount = UnitData.StopCount + (int)StopCountAdd;
        Speed = (SpeedBase + SpeedAdd) * (1 + SpeedRate) / 2;
        if (Speed < SpeedBase * 0.1f) Speed = SpeedBase * 0.1f;
        MaxHp = ((HpBase + HpAdd) * (1 + HpRate) + HpAddFin) * (1 + HpRateFin);
        Hp = MaxHp - hpDown;
        Attack = ((AttackBase + AttackAdd) * (1 + AttackRate) + AttackAddFin) * (1 + AttackRateFin);
        if (Attack < 0) Attack = 0;
        Defence = ((DefenceBase + DefenceAdd) * (1 + DefenceRate) + DefenceAddFin) * (1 + DefenceRateFin);
        if (Defence < 0) Defence = 0;
        MagicDefence = ((MagicDefenceBase + MagicDefenceAdd) * (1 + MagicDefenceRate) + MagicDefenceAddFin) * (1 + MagicDefenceRateFin);
        if (MagicDefence < 0) MagicDefence = 0;
        if (MagicDefence > 100) MagicDefence = 100;
        HpRecover = (HpRecoverBase + HpRecoverAdd);
        if (HpRecover > 0) HpRecover = HpRecover * (1 + HpRecoverRate);
        Agi = ((AgiBase + AgiAdd) * (1 + AgiRate) + AgiAddFin) * (1 + AgiRateFin);
        if (Agi < 10f) Agi = 10f;
        Weight = (int)(WeightBase + WeightAdd);
        AttackGap = (AttackGapBase + AttackGapAdd) * (1 + AttackGapRate);
        if (AttackGap < 0.1f) AttackGap = 0.1f;
        SkillCost = SkillCostAdd + 1;
        PowerSpeed = PowerSpeedAdd + 1;
        Resist = ResistAdd+1;
        AttackRange = (1 + AttackRangeAdd) * (1 + AttackRangeRate);
    }

    public void UpdateBuffs()
    {
        updateElement();
        if (!Alive()) return;
        if (Hp > MaxHp) Hp = MaxHp;
        IfHide = hideBase;
        IfHideAnti = false;
        IfSleep = false;
        IfSelectable = true;
        CanStopOther = true;
        bool lastIfStun = IfStun;
        IfStun = false;
        foreach (var buff in Buffs.Reverse<Buff>())
        {
            buff.Update();
        }
        Refresh();

        if (unbalance) IfStun = true;
        if (lastIfStun && !IfStun)
        {
            SetStatus(StateEnum.Idle);
        }
        if (IfHideAnti || IfStoped()) IfHide = false;
        foreach (var buff in Buffs.Reverse<Buff>())//计算完单位属性后，有些buff要更新显示状态
        {
            buff.UpdateView();
        }
    }
    public virtual void UpdateAction()
    {
        if (Alive())
        {
            //HP自动回复
            Hp += HpRecover * SystemConfig.DeltaTime;
            Hp += HpRecoverP * MaxHp * SystemConfig.DeltaTime;
            if (Hp > MaxHp) Hp = MaxHp;
            if (Hp < 0) DoDie(null);
        }
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
        CanChangeAnimation = true;
        SetStatus(StateEnum.Die);
        Dying.Set(UnitModel.GetAnimationDuration("Die"));

        var box = UnitModel?.GetComponent<BoxCollider>();
        if (box != null) box.enabled = false;

        Unit sourceUnit = null;
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
                Debug.Log($"{skill.Unit.UnitData.Id} 击杀了 {UnitData.Id}");
                if (skill.Unit is Units.干员 u && u.Parent != null)//召唤物杀人，算主子击杀
                    sourceUnit = u.Parent;
                else
                    sourceUnit = skill.Unit;
                sourceUnit.Trigger(TriggerEnum.击杀);
                Battle.TriggerDatas.Pop();
            }
        }

        //死亡事件
        Battle.TriggerDatas.Push(new TriggerData()
        {
            Target = this,
            User = sourceUnit,
        });
        Battle.Trigger(TriggerEnum.死亡);
        Battle.TriggerDatas.Pop();

        if (Dying.Finished()) Finish();
    }

    public virtual void Finish(bool leaveEvent=true)
    {
        foreach (var unit in StopUnits)
        {
            unit.StopUnit = null;
        }
        StopUnits.Clear();
        IfAlive = false;
        if (leaveEvent)
        {
            Battle.TriggerDatas.Push(new TriggerData()
            {
                Target = this,
            });
            Trigger(TriggerEnum.离场);
            Battle.TriggerDatas.Pop();
        }

        foreach (var buff in Buffs.ToArray())
        {
            if (!buff.BuffData.DeadRemain)
                buff.Finish();
        }
        foreach (var buff in PushBuffs.ToArray())
        {
            (buff as Buff).Finish();
        }
        foreach (var skill in Skills)
        {
            skill.Finish();
        }
        PushBuffs.Clear();
    }

    protected void UpdateSkills()
    {
        var inAttack = !AttackingAction.Finished();
        AttackingAction.Update(SystemConfig.DeltaTime);
        //if (inAttack && AttackingAction.Finished())
        //{
        //    Debug.Log("Ready to attack");
        //}
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
        foreach (var skill in Skills)
        {
            skill.UpdateOpening();
        }
        if (inAttack && AttackingAction.Finished() && State != StateEnum.Die)
        {
            SetStatus(StateEnum.Idle);
        }
    }

    protected virtual void UpdateMove()
    {

    }

    public void UpdateCollision()
    {
        if (Position.x < Battle.Map.minX - 0.5f) Position.x = Battle.Map.minX - 0.4999f;
        if (Position.z < Battle.Map.minZ - 0.5f) Position.z = Battle.Map.minZ - 0.4999f;
        if (Position.x > Battle.Map.maxX + 0.5f) Position.x = Battle.Map.maxX + 0.4999f;
        if (Position.z > Battle.Map.maxZ + 0.5f) Position.z = Battle.Map.maxZ + 0.4999f;

        if (Height > 0) return;
        var tile = Battle.Map.Tiles[GridPos.x, GridPos.y];

        if (tile.FarAttackGrid)
        {
            float x = Position2.x - GridPos.x;
            float y = Position2.y - GridPos.y;
            bool b1 = y - x > 0;
            bool b2 = x + y < 0;
            if (b1 && b2)
            {
                var t = Battle.Map.Tiles.Get(GridPos.x - 1, GridPos.y);
                if (t == null || t.FarAttackGrid) return;
                Position.x = Mathf.RoundToInt(Position.x) - 0.5001f;
            }
            if (b1 && !b2)
            {
                var t = Battle.Map.Tiles.Get(GridPos.x + 1, GridPos.y);
                if (t == null || t.FarAttackGrid) return;
                Position.z = Mathf.RoundToInt(Position.z) + 0.5001f;
            }
            if (!b1 && b2)
            {
                var t = Battle.Map.Tiles.Get(GridPos.x, GridPos.y-1);
                if (t == null || t.FarAttackGrid) return;
                Position.z = Mathf.RoundToInt(Position.z) - 0.5001f;
            }
            if (!b1 && !b2)
            {
                var t = Battle.Map.Tiles.Get(GridPos.x, GridPos.y + 1);
                if (t == null || t.FarAttackGrid) return;
                Position.x = Mathf.RoundToInt(Position.x) + 0.5001f;
            }
        }
    }

    public void Trigger(TriggerEnum triggerEnum)
    {
        for (int i = Skills.Count-1; i >= 0; i--)
        {
            Skill skill = Skills[i];
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

    public Skill LearnSkill(int skillId, Skill parent = null)
    {
        var s = Skills.Find(x => x.Id == skillId);
        if (s != null) return s;
        var skillConfig = Database.Instance.Get<SkillData>(skillId);
        var skill = typeof(Unit).Assembly.CreateInstance(nameof(Skills) + "." + skillConfig.Type) as Skill;
        skill.Unit = this;
        skill.Id = skillId;
        skill.Init();
        if (parent != null) skill.Parent = parent;
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
        if (skillConfig.Skills != null)
            foreach (var id in skillConfig.Skills)
            {
                LearnSkill(id, skill);
            }
        if (skillConfig.ExSkills != null)
            foreach (var id in skillConfig.ExSkills)
            {
                LearnSkill(id, skill);
            }
        return skill;
    }

    public Buff AddBuff(int buffId,Skill source,int index)
    {
        if (IgnoreBuffs.Contains(buffId)) return null;//免疫对应Buff
        var config = Database.Instance.Get<BuffData>(buffId);
        if (config.RelyBuff != null && !Buffs.Any(x => x.Id == config.RelyBuff.Value))
            return null;
        //权且加上来源判断，因为现在很多buff共用一个id会产生冲突。
        //如果需要处理buff冲突的情况，再修改这里
        //判断是否存在buff的升级版
        var oldBuff = Buffs.FirstOrDefault(x => (x.Id == buffId || config.Upgrade == x.Id) && (config.UnSourceCheck || x.Skill == source));
        if (oldBuff != null)
        {
            oldBuff.Reset();
            //Refresh();
            return oldBuff;
        }
        else
        {
            var buff = typeof(Buff).Assembly.CreateInstance(nameof(Buffs) + "." + config.Type) as Buff;
            buff.Index = index;
            buff.Id = buffId;
            buff.Skill = source;
            buff.Unit = this;
            Buffs.Add(buff);
            if (buff is IShield shield) Shields.Add(shield);
            buff.Init();
            //Refresh();
            return buff;
        }
    }

    public void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
        if (buff is IShield shield) Shields.Remove(shield);
        //Refresh();
    }
    #region 推拉相关
    public List<IPushBuff> PushBuffs = new List<IPushBuff>();

    /// <summary>
    /// 失衡硬直
    /// </summary>
    public CountDown Unbalancing = new CountDown();

    public bool Unbalance => unbalance || !Unbalancing.Finished();

    public bool unbalance;

    Vector2 power;

    public virtual void UpdatePush()
    {
        //if (!Alive()) return;
        Unbalancing.Update(SystemConfig.DeltaTime);
        foreach (Buff buff in PushBuffs.Reverse<IPushBuff>())
        {
            buff.Update();
        }
        if (!Unbalance) return;
        //(this as Units.敌人).CheckBlock();
        Vector2 power = Vector2.zero;
        foreach (var buff in PushBuffs.ToList())
        {
            var pushPower= buff.GetPushPower();
            power += pushPower;
        }
        if (power.magnitude < 0.1f && Unbalancing.Finished()) //力太小，失衡状态结束
        {
            unbalance = false;
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
        if (MainSkill != null && !MainSkill.Opening.Finished())
            return;
        foreach (var skill in Skills)
        {
            skill.RecoverPower(count);
        }
    }

    public virtual bool Alive()
    {
        return IfAlive;
    }

    public void SetStatus(StateEnum state)
    {
        //Log.Debug($"{UnitData.Id} 由 {this.State} 转变为 {state}");
        if (this.State == StateEnum.Die && state != StateEnum.Default) return;
        this.State = state;
        if (CanChangeAnimation)
        {
            if (state == StateEnum.Default)
                AnimationName = UnitData.DefaultAnimation;
            else if (state == StateEnum.Idle)
                AnimationName = GetIdleAnimation();
            else if (state == StateEnum.Move)
                AnimationName = GetMoveAnimation();
            else if (state == StateEnum.Start)
                AnimationName = Unit.StartAnimation;
            else if (state == StateEnum.Die)
                AnimationName = GetDieAnimation();
            AnimationSpeed = 1;
            if (state == StateEnum.Stun)
            {
                if (UnitData.StunAnimation != null)
                    AnimationName = UnitData.StunAnimation;
                else
                {
                    //没有眩晕动画的场合，暂停模型动画
                    AnimationSpeed = 0;
                }
            }
        }
        if (state != StateEnum.Attack && !AttackingAction.Finished())
        {
            AttackingAction.Finish();
        }
    }

    public virtual void CreateModel()
    {
        if (string.IsNullOrEmpty(UnitData.Model)) return;
        //Debug.Log(UnitData.Model);
        GameObject go = ResHelper.Instantiate(PathHelper.UnitPath + UnitData.Model);
        UnitModel = go.GetComponent<UnitModel>();
        UnitModel.Init(this);
    }

    public void Heal(DamageInfo heal,bool ifShowHeal)
    {
        heal.FinalDamage = heal.Attack * heal.DamageRate * HealReceiveRate;
        Hp += heal.FinalDamage;
        if (ifShowHeal) UnitModel.ShowHeal(heal);
        if (Hp > MaxHp)
            Hp = MaxHp;
    }

    public void Damage(DamageInfo damageInfo)
    {
        float damage = damageInfo.Attack * damageInfo.DamageRate;
        if (damageInfo.DamageType == DamageTypeEnum.Normal) damage *= DamageReceiveRate;
        if (damageInfo.DamageType == DamageTypeEnum.Magic) damage *= MagicDamageReceiveRate;
        damage = damageWithDefence(damage, damageInfo.DamageType,damageInfo.DefIgnore, damageInfo.DefIgnoreRate,damageInfo.MinDamageRate);
        damageInfo.FinalDamage = damage;
        float damageEx = damageInfo.Attack;
        damageEx = damageWithDefence(damageEx, damageInfo.DamageType, 0, 0, damageInfo.MinDamageRate);
        if (damage > damageEx * 1.5f) UnitModel.ShowCrit(damageInfo);
        if (AllBlock > 0 && Battle.Random.NextDouble() < AllBlock) damageInfo.Avoid = true;
        if (damageInfo.DamageType == DamageTypeEnum.Normal && Block > 0 && Battle.Random.NextDouble() < Block) damageInfo.Avoid = true;
        if (damageInfo.DamageType == DamageTypeEnum.Magic && MagBlock > 0 && Battle.Random.NextDouble() < MagBlock) damageInfo.Avoid = true;
        if (!damageInfo.Avoid)
        {
            foreach (var shield in Shields.ToArray())
            {
                shield.Absorb(damageInfo);
            }
            Hp -= damageInfo.FinalDamage;
            if (Hp <= 0)
            {
                Battle.TriggerDatas.Push(new TriggerData()
                {
                    Target = this,
                });
                Trigger(TriggerEnum.致命);

                Battle.TriggerDatas.Pop();
            }
            //致命事件过后，如果血量依旧低于0，则判定单位死亡
            if (Hp <= 0)
            {
                Hp = 0;
                DoDie(damageInfo);
            }
        }
    }

    float damageWithDefence(float damage,DamageTypeEnum damageType,float defIgnore, float defIgnoreRate,float minDamageRate)
    {
        switch (damageType)
        {
            case DamageTypeEnum.Normal:
                var defence = Mathf.Max(0, Defence * (1 - defIgnoreRate) - defIgnore);
                damage = Mathf.Max(damage * minDamageRate, damage - defence);//抛光系数0.05
                if (damage < 0) damage = 1;
                break;
            case DamageTypeEnum.Magic:
                var magDefence = Mathf.Max(0, MagicDefence * (1 - defIgnoreRate) - defIgnore);
                damage = Mathf.Max(damage * minDamageRate, damage * (100 - magDefence) / 100);
                break;
        }
        return damage;
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
        if (Skills.Count > 0)
            return Skills[0];
        else return null;
    }

    public virtual float Hatred()
    {
        return -Hatre * 100000;
    }

    public void BreakAllCast()
    {
        //AttackingAction.Finish();
        foreach (var skill in Skills)
        {
            skill.BreakCast();
        }
        SetStatus(StateEnum.Idle);
    }

    public virtual bool IfStoped()
    {
        return false;
    }

    public string[] GetAnimation()
    {
        return OverWriteAnimation == null ? AnimationName : OverWriteAnimation;
    }

    public string[] GetMoveAnimation()
    {
        return OverWriteMove == null ? UnitData.MoveAnimation : OverWriteMove;
    }
    public string[] GetIdleAnimation()
    {
        return OverWriteIdle == null ? UnitData.IdleAnimation : OverWriteIdle;
    }
    public string[] GetDieAnimation()
    {
        return OverWriteDie == null ? UnitData.DeadAnimation : OverWriteDie;
    }

    public virtual Vector2Int PointWithDirection(Vector2Int v2)
    {
        return GridPos + v2;
    }

    public Vector3 GetHitPoint()
    {
        return UnitModel.GetPoint(UnitData.HitPointName);
    }
    public void AddStop(Units.敌人 target)
    {
        StopUnits.Add(target);
        target.StopUnit = this;
        if (target.Position != Position && (target.Position - Position).magnitude < UnitData.Radius + target.UnitData.Radius)
        {
            target.Position = Position + (target.Position - Position).normalized * (UnitData.Radius + target.UnitData.Radius);
        }
    }

    public void RemoveStop(Units.敌人 target)
    {
        StopUnits.Remove(target);
        target.StopUnit = null;
    }

    public bool CanStop(Units.敌人 target)
    {
        if (Team != 0) return false;
        if (!Alive()) return false;
        if (!CanStopOther) return false;
        if (target.UnStopped) return false;
        if (StopUnits.Contains(target)) return true;
        if (target.StopUnit != null) return false;
        if (NowGrid.FarAttackGrid) return false;
        return StopUnits.Sum(x => x.StopCost) + target.StopCost <= StopCount;
    }

    public void ChangeNerve(float count)
    {
        if (!ElementProtect.Finished()) return;
        NervePoint += count;
        if (NervePoint <= 0) NervePoint = 0;
    }

    void updateElement()
    {
        ElementProtect.Update(SystemConfig.DeltaTime);
        if (NervePoint >= 1000)
        {
            NervePoint = 0;
            ElementProtect.Set(10f);
            Damage(new DamageInfo()
            {
                Target = this,
                Attack = 1000,
                DamageType = DamageTypeEnum.Real,
            });
            AddBuff(Database.Instance.GetIndex<BuffData>("眩晕"), Battle.RuleUnit.Skills[0], 0);
        }
    }
}
