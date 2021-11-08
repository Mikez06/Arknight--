using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 对个数伤害 : Modify, IDamageModify
    {
        public int Count;
        public float Rate;
        public override void Init()
        {
            base.Init();
            Count = ModifyData.Data.GetInt("Count");
            Rate = ModifyData.Data.GetFloat("Rate");
        }
        public void Modify(DamageInfo damageInfo)
        {
            if (damageInfo.AllCount == Count)
            {
                damageInfo.DamageRate *= Rate;
            }
        }
    }
}
