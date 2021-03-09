﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public Battle Battle;
    public UnitConfig Config => Database.Instance.Get<UnitConfig>(Id);
    public int Id;

    public UnitModel UnitModel;
    public BattleUI.UI_BattleUnit uiUnit;

    public Vector3 Position;
    public Vector2 Position2 => new Vector2(Position.x, Position.z);

    public Vector2Int GridPos => new Vector2Int(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z));

    public Vector2 Direction = new Vector2(1, 0);

    /// <summary>
    /// 在Z轴上加随机位移 让单位不会成为一条直线，更加自然
    /// </summary>
    public float PosOffset;

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

    public float AttackGap;

    public float Attack;

    public float Defence;

    public float MagicDefence;

    public float Power;
    public int MaxPower;
    public int PowerCount;
    public float PowerSpeed;

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
    /// 转身动画
    /// </summary>
    public CountDown Turning = new CountDown();
    public float ScaleX = -1;
    public float TargetScaleX = -1;

    public string AnimationName = "Default";
    public float AnimationSpeed = 1;

    public virtual void Init()
    {
        if (Config.Skills != null)
            foreach (var skillId in Config.Skills)
            {
                var skillConfig = Database.Instance.Get<SkillConfig>(skillId);
                var skill = typeof(Unit).Assembly.CreateInstance(nameof(Skills) + "." + skillConfig.Type) as Skill;
                skill.Unit = this;
                skill.Id = skillId;
                skill.Init();
                Skills.Add(skill);
            }
        if (Config.MainSkill != null) MainSkill = Skills.FirstOrDefault(x => x.Id == Config.MainSkill.Value);
        if (MainSkill != null)
        {
            Power = MainSkill.Config.StartPower;
            MaxPower = MainSkill.Config.MaxPower;
            PowerCount = MainSkill.Config.PowerCount;
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
        MagicDefence = Config.Defence;
        PowerSpeed = 1f;
        Agi = 100;
        foreach (var buff in Buffs)
        {
            buff.Apply();
        }
    }

    public void UpdateBuffs()
    {
        foreach (var buff in Buffs.Reverse<Buff>())
        {
            if (buff.Duration.Finished()) Buffs.Remove(buff);
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
        State = StateEnum.Die;
        AnimationName = "Die";
        AnimationSpeed = 1;
        Dying.Set(UnitModel.GetAnimationDuration("Die"));
    }

    public virtual void Finish()
    {
    }

    protected void UpdateSkills()
    {
        for (int i = Skills.Count - 1; i >= 0; i--)
        {
            if (i >= Skills.Count) continue;
            var sk = Skills[i];
            if (sk != null)
            {
                sk.Update();
            }
        }
        if (Attacking.Update(SystemConfig.DeltaTime))
        {
            SetStatus(StateEnum.Idle);
            AnimationName = "Idle";
            AnimationSpeed = 1;
        }
    }

    protected virtual void UpdateMove()
    {

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
            var config = Database.Instance.Get<BuffConfig>(buffId);
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

    public void RecoverPower(float count)
    {
        if (!MainSkill.Opening.Finished())
            return;
        Power += count;
        if (Power > MaxPower * PowerCount)
            Power = MaxPower * PowerCount;
    }

    public bool Alive()
    {
        return Hp > 0;
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
        ResourcesManager.Instance.LoadBundle(PathHelper.UnitPath + Config.Model);
        GameObject go = GameObject.Instantiate(ResourcesManager.Instance.GetAsset<GameObject>(PathHelper.UnitPath + Config.Model, Config.Model));
        UnitModel = go.GetComponent<UnitModel>();
        UnitModel.Unit = this;
        UnitModel.Init();
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
}

public class DamageInfo
{
    public object Source;
    public float Attack;
    public DamageTypeEnum DamageType;
    public float DamageRate;
    public float FinalDamage;
}
