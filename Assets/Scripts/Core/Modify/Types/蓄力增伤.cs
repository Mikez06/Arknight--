using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys 
{
    public class 蓄力增伤 : Modify, IDamageModify
    {
        public int LastTick;
        public float Rate;
        public float Time;
        public override void Init()
        {
            base.Init();
            LastTick = Battle.Tick;
            Rate = ModifyData.Data.GetFloat("Rate");
            Time = ModifyData.Data.GetFloat("Time");
        }
        public void Modify(DamageInfo damageInfo)
        {
            float time = (Battle.Tick - LastTick) * SystemConfig.DeltaTime;
            var t = time / Time;
            if (t > 1) t = 1;
            damageInfo.DamageRate *= 1 + t * (Rate - 1);
            LastTick = Battle.Tick;
        }
    }
}
