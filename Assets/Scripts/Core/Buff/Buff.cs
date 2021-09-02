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

    public virtual void Init()
    {
        updateLastTime();
        if (BuffData.LastingEffect.HasValue)
        {
            LastingEffect = EffectManager.Instance.GetEffect(BuffData.LastingEffect.Value);
            LastingEffect.transform.SetParent(Unit.UnitModel.transform);
            LastingEffect.transform.position = Unit.UnitModel.GetPoint(Database.Instance.Get<EffectData>(BuffData.LastingEffect.Value).BindPoint);
        }
    }

    public virtual void Apply()
    {

    }

    public virtual void Reset()
    {
        updateLastTime();
        if (BuffData.Upgrade != null)
        {
            Unit.RemoveBuff(this);
            Unit.AddBuff(BuffData.Upgrade.Value, this.Skill);
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
        Duration.Update(SystemConfig.DeltaTime);
        if (Duration.Finished())
        {
            Finish();
        }
    }

    public void Finish()
    {
        Unit.RemoveBuff(this);
        if (LastingEffect != null)
        {
            EffectManager.Instance.ReturnEffect(LastingEffect);
        }
    }
}
