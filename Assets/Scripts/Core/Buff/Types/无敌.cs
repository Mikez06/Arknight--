﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    /*
     无敌：无法被选中+不会受到伤害
     */
    public class 无敌 : Buff
    {
        public override void Update()
        {
            base.Update();
            Unit.IfSelectable = false;
        }
    }
}
