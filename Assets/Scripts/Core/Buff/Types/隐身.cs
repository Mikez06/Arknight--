using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 隐身 : Buff
    {
        public override void Update()
        {
            base.Update();
            Unit.IfHide = true;
        }
    }
}
