using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 打断攻击:Buff
    {
        public override void Init()
        {
            base.Init();
            Unit.BreakAllCast();
        }
    }
}
