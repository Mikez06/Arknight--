using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 不可阻挡 : Buff
    {
        public override void Apply()
        {
            base.Apply();
            if (Unit is Units.敌人 u)
            {
                u.UnStopped = true;
            }
        }
    }
}
