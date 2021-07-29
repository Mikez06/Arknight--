using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public Battle Battle;
    public UnitData Config => Database.Instance.Get<UnitData>(Id);
    public int Id;

    public UnitModel UnitModel;
    public BattleUI.UI_BattleUnit uiUnit;

    public Vector3 Position;
    public Vector2 Position2 => new Vector2(Position.x, Position.z);

    public Vector2Int GridPos => new Vector2Int(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z));

    public Vector2 Direction = new Vector2(1, 0);

    public MapGrid NowGrid => Battle.Map.Grids[Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z)];

    public StateEnum State = StateEnum.Default;
    public float Hp;
    public float MaxHp;
    public List<Skill> Skills = new List<Skill>();
    public Skill MainSkill;

    public List<Buff> Buffs = new List<Buff>();

    /// <summary>
    /// 攻速
    /// </summary>
    public float Agi;
    /// <summary>
    /// 移速
    /// </summary>
    public float Speed;
    public float SpeedRate;

    public float AttackGap;

    public float Attack;
    public float AttackRate;

    public float Defence;
    public float DefenceRate;

    public float MagicDefence;
    public float MagicDefecneRate;

    public float PowerSpeed;

    public int Weight;

    public bool IfHide;

    public bool IfAlive = true;

    /// <summary>
    /// 攻击动画
    /// </summary>
    public CountDown Attacking = new CountDown();
    /// <summary>
    /// 死亡动画
    /// </summary>
    public CountDown Dying = new CountDown();
    /// <summary>
    /// 硬直
    /// </summary>
    public CountDown Recover = new CountDown();  
    /// <summary>
    /// 失衡
    /// </summary>
    public CountDown Unbalance;


    public float ScaleX = -1;
    public float TargetScaleX = -1;

    public string AnimationName = "Default";
    public string OverWriteAnimation;
    public float AnimationSpeed = 1;

    public virtual void Init()
    {
        if (Config.Skills != null)
            foreach (var skillId in Config.Skills)
            {
                LearnSkill(skillId);
            }
        CreateModel();
        Refresh();
        Hp = MaxHp;
    }

    public virtual void Refresh()
    {
        Speed = Config.Speed;
        MaxHp = Config.Hp;
        Attack = Config.Attack;
        Defence = Config.Defence;
        MagicDefence = Config.MagicDefence;
        Weight = Config.Weight;
        PowerSpeed = 1f;
        Agi = 100;
        AttackGap = 0;
        SpeedRate = 0;
        foreach (var buff in Buffs)
        {
            buff.Apply();
        }
        Speed = Speed * (1 - SpeedRate);
        if (Speed < 0) Speed = 0;
        Attack = Attack * (1 + AttackRate);
        Defence = Defence * (1 + DefenceRate);
        MagicDefence = MagicDefence + MagicDefecneRate;
        if (MagicDefecneRate < 0) MagicDefecneRate = 0;
    }

    public void UpdateBuffs()
    {
        foreach (var buff in Buffs.Reverse<Buff>())
        {
            if (buff.Duration.Finished()) RemoveBuff(buff);
            else buff.Update();
        }
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

    public virtual void DoDie()
    {
        IfAlive = false;
        SetStatus(StateEnum.Die);
        Dying.Set(UnitModel.GetAnimationDuration("Die"));
    }

    public virtual void Finish()
    {
    }

    protected void UpdateSkills()
    {
        var inAttack = !Attacking.Finished();
        Attacking.Update(SystemConfig.DeltaTime);
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

    public Skill LearnSkill(int skillId)
    {
        var s = Skills.Find(x => x.Id == skillId);
        if (s != null) return s;
        var skillConfig = Database.Instance.Get<SkillData>(skillId);
        var skill = typeof(Unit).Assembly.CreateInstance(nameof(Skills) + "." + skillConfig.Type) as Skill;
        skill.Unit = this;
        skill.Id = skillId;
        skill.Init();
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
        var oldBuff = Buffs.FirstOrDefault(x => x.Id == buffId);
        if (oldBuff != null)
        {
            oldBuff.Skill = source;
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
        Refresh();
    }

    public void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
        Refresh();
    }

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
        this.State = state;
        AnimationName = state.ToString();
        AnimationSpeed = 1;
        if (state != StateEnum.Attack && !Attacking.Finished())
        {
            Attacking.Finish();
        }
    }

    public void CreateModel()
    {
        GameObject go = ResHelper.Instantiate(PathHelper.UnitPath + Config.Model);
        UnitModel = go.GetComponent<UnitModel>();
        UnitModel.Unit = this;
        UnitModel.Init();
    }

    public void Heal(float heal)
    {
        Hp += heal;
        if (Hp > MaxHp)
            Hp = MaxHp;
    }

    public void Damage(DamageInfo damageInfo)
    {
        float damage = damageInfo.Attack * damageInfo.DamageRate;
        switch (damageInfo.DamageType)
        {
            case DamageTypeEnum.Normal:
                damage -= Defence;
                if (damage < 0) damage = 1;
                break;
            case DamageTypeEnum.Magic:
                damage = damage * (100 - MagicDefence) / 100;
                break;
        }
        damageInfo.FinalDamage = damage;
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            DoDie();
        }
    }

    public virtual float Hatred()
    {
        return Config.Hatred * 1000;
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

    public string GetAnimation()
    {
        return string.IsNullOrEmpty(OverWriteAnimation) ? AnimationName : OverWriteAnimation;
    }
}

public class DamageInfo
{
    public object Source;
    public float Attack;
    public DamageTypeEnum DamageType;
    public float DamageRate;
    public float FinalDamage;
}
