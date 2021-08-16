using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Buff
{
    public BuffData BuffData => Database.Instance.Get<BuffData>(Id);
    public int Id;

    public Unit Unit;
    public Skill Skill;
    protected Battle Battle => Skill.Unit.Battle;

    public CountDown Duration = new CountDown();

    public virtual void Init()
    {
        Duration.Set(BuffData.LastTime);
    }

    public virtual void Apply()
    {

    }

    public virtual void Reset()
    {
        Duration.Set(BuffData.LastTime);
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
