using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 睡眠:Buff
    {
        public override void Update()
        {
            base.Update();
            Unit.IfSleep = true;
            Unit.IfStun = true;
            Unit.CanStopOther = false;
            Unit.SetStatus(StateEnum.Stun);
        }
    }
}
