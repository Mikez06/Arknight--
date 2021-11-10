using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 消去伤害 : Buff
    {
        public override void Finish()
        {
            Unit.Damage(new DamageInfo()
            {
                Attack = Unit.MaxHp * BuffData.Data.GetFloat("DamageRate"),
                Source = this,
                DamageType = DamageTypeEnum.Real,
            });
            base.Finish();
        }
    }
}
