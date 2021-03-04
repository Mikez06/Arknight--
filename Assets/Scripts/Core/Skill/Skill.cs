using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Skill
{
    public Unit Unit;

    public Unit Target;
    protected Battle Battle => Unit.Battle;

    public SkillConfig Config => Database.Instance.Get<SkillConfig>(Id);

    public int Id;

    public virtual void Init()
    {

    }

    public virtual void Update()
    {

    }

    public virtual bool CanUse()
    {
        return true;
    }
}

