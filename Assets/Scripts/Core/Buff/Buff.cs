using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class Buff
{
    public BuffData BuffData => Database.Instance.Get<BuffData>(Id);
    public int Id;

    [System.NonSerialized]
    public Unit Unit;
    public Skill Skill;
    protected Battle Battle => Skill.Unit.Battle;

    public CountDown Duration = new CountDown();

    public Effect LastingEffect;

    public Buff RelayBuff;
    public bool Dead;

    public virtual void Init()
    {
        updateLastTime();
        if (BuffData.LastingEffect.HasValue)
        {
            LastingEffect = EffectManager.Instance.GetEffect(BuffData.LastingEffect.Value);
            LastingEffect.transform.SetParent(Unit.UnitModel.transform);
            LastingEffect.transform.position = Unit.UnitModel.GetPoint(Database.Instance.Get<EffectData>(BuffData.LastingEffect.Value).BindPoint);
        }
        Log.Debug($"{Unit.UnitData.Id} 新获得了buff {BuffData.Id}");
    }

    public virtual void Apply()
    {

    }

    public virtual void Reset()
    {
        updateLastTime();
        if (BuffData.Upgrade != null)
        {
            if (LastingEffect != null)
            {
                EffectManager.Instance.ReturnEffect(LastingEffect);
                LastingEffect = null;
            }
            //Finish();
            //Unit.RemoveBuff(this);
            Unit.AddBuff(BuffData.Upgrade.Value, this.Skill);
        }
        if (BuffData.IfSwitch)
        {
            Finish();
        }
    }

    protected virtual void updateLastTime()
    {
        float lastTime = BuffData.LastTime;
        if (Skill.SkillData.BuffLastTime != null)
        {
            lastTime = Skill.SkillData.BuffLastTime.Value;
        }
        Duration.Set(lastTime);
    }

    public virtual void Update()
    {
        if (Skill.SkillData.BuffRely)//单位离开技能范围，或施法者死亡时，buff自动消失
        {
            if (!Skill.Unit.Alive() || !Skill.GetAttackTarget().Contains(Unit) || (Skill.SkillData.OpenTime > 0 && Skill.Opening.Finished()))
            {
                Finish();
            }
        }

        if (BuffData.RelyBuff != null)
        {
            if (RelayBuff == null) RelayBuff = Unit.Buffs.FirstOrDefault(x => x.Id == BuffData.RelyBuff.Value);
            if (RelayBuff == null || RelayBuff.Dead) Finish();
        }

        if (BuffData.Resist)
        {
            Duration.Update(SystemConfig.DeltaTime / Unit.Resist);
        }
        else
            Duration.Update(SystemConfig.DeltaTime);
        if (Duration.Finished())
        {
            Finish();
        }
    }

    public virtual void UpdateView()
    {

    }

    public virtual void Finish()
    {
        Dead = true;
        Unit.RemoveBuff(this);
        if (LastingEffect != null)
        {
            EffectManager.Instance.ReturnEffect(LastingEffect);
            LastingEffect = null;
        }
        Log.Debug($"{Unit.UnitData.Id} 失去了buff {BuffData.Id}");
    }
}
