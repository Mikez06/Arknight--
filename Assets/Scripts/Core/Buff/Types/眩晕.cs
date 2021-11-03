using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 眩晕 : Buff
    {
        bool ifChangeAnimation;

        public override void Init()
        {
            base.Init();
            ifChangeAnimation = BuffData.Data.GetBool("ChangeAnimation");
        }

        public override void Update()
        {
            base.Update();
            Unit.IfStun = true;
            Unit.CanStopOther = false;
            if (ifChangeAnimation) changeAnimation();
        }

        protected virtual void changeAnimation()
        {
            Unit.SetStatus(StateEnum.Stun);
            //Unit.AnimationName = Unit.StunAnimation;
            //Unit.AnimationSpeed = 1;
        } 
    }
}
