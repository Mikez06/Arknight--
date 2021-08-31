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
            //Log.Debug($"{Unit.UnitData.Id}隐身了");
            Unit.IfHide = true;
        }
    }
}
