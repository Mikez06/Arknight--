using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Modify
{
    public int Id;

    public ModifyData ModifyData ;

    protected Battle Battle => BattleManager.Instance.Battle;

    public virtual void Init()
    {
        ModifyData = Database.Instance.Get<ModifyData>(Id);
    }
}

public interface IDamageModify
{
    void Modify(DamageInfo damageInfo);
}

public interface ITargetModify
{
    int Modify(int count);
}