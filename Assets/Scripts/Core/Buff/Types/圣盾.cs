using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 圣盾 : Buff, IShield
    {
        public int Count;

        public override void Init()
        {
            base.Init();
            Count = BuffData.Data.GetInt("Count");
        }

        public void Absorb(DamageInfo damageInfo)
        {
            if (damageInfo.FinalDamage > 0)
            {
                Count--;
                damageInfo.FinalDamage = 0;
            }
            if (Count == 0) Finish();
        }
    }
}
