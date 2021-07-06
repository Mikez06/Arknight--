using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Buff
{
    public BuffData Config => Database.Instance.Get<BuffData>(Id);
    public int Id;

    public Unit Unit;
    public Skill Skill;
    protected Battle Battle => Skill.Unit.Battle;

    public CountDown Duration = new CountDown();

    public virtual void Init()
    {
        Duration.Set(Config.LastTime);
    }

    public virtual void Apply()
    {

    }

    public virtual void Reset()
    {
        Duration.Set(Config.LastTime);
    }

    public virtual void Update()
    {
        Duration.Update(SystemConfig.DeltaTime);
    }

}
