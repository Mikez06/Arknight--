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

    public virtual void Init()
    {
        updateLastTime();
    }

    public virtual void Apply()
    {

    }

    public virtual void Reset()
    {
        updateLastTime();
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
            Unit.RemoveBuff(this);
        }
    }

}
