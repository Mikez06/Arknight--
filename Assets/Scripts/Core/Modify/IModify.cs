using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Modify
{
    public int Id;

    public ModifyData ModifyData => Database.Instance.Get<ModifyData>(Id);

    public Skill Skill;

    protected Unit Unit => Skill.Unit;

    protected Battle Battle => Skill.Unit.Battle;

    public virtual void Init()
    {

    }
}

public interface ISelfDamageModify
{
    void Modify(DamageInfo damageInfo);
}

public interface IDamageModify
{
    void Modify(DamageInfo damageInfo);
}

public interface ITargetModify
{
    int Modify(int count);
}