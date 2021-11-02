using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 叠加一层时，效果为降低单位的攻速
 叠加二层时，效果为眩晕单位，同时阻止单位动画
 
 */
namespace Buffs
{
    public class 冻结 : Buff
    {
        public override void Init()
        {
            base.Init();
            Unit.AnimationSpeed = 0;
            Unit.CanChangeAnimation = false;
        }

        public override void Update()
        {
            base.Update();
            Unit.IfStun = true;
        }

        public override void Finish()
        {
            base.Finish();
            Unit.CanChangeAnimation = true;
            Unit.AnimationSpeed = 1;
        }
    }
}