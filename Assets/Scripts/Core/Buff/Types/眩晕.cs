using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 眩晕 : Buff
    {
        public override void Update()
        {
            base.Update();
            Unit.IfStun = true;
            Unit.CanStopOther = false;
            changeAnimation();
        }

        protected virtual void changeAnimation()
        {
            Unit.SetStatus(StateEnum.Stun);
            //Unit.AnimationName = Unit.StunAnimation;
            //Unit.AnimationSpeed = 1;
        } 
    }
}
