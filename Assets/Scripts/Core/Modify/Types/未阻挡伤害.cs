using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 未阻挡伤害:Modify, IDamageModify
    {
        public float Rate;
        public override void Init()
        {
            base.Init();
            Rate = ModifyData.Data.GetFloat("Rate");
        }
        public void Modify(DamageInfo damageInfo)
        {
            if (Unit is Units.干员 u && !u.StopUnits.Contains(damageInfo.Target))
            {
                damageInfo.Attack *= Rate;
            }
        }
    }
}
