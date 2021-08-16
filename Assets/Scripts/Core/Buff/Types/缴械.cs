using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 缴械 : Buff
    {
        public override void Apply()
        {
            base.Apply();
            Unit.CanAttack = false;
        }
    }
}
