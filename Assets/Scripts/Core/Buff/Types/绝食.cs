using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 绝食 : Buff
    {
        public override void Apply()
        {
            base.Apply();
            Unit.CanBeHeal = false;
        }
    }
}
