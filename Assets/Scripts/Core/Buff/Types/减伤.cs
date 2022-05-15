using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 减伤 : Buff, IShield
    {

        int buffId = -1;
        float rate;

        public override void Init()
        {
            base.Init();
            rate = BuffData.Data.GetFloat("Rate");
            string buffName = BuffData.Data.GetStr("TargetBuffNeed");
            if (!string.IsNullOrEmpty(buffName))
                buffId = Database.Instance.GetIndex<BuffData>(buffName);
        }

        public void Absorb(DamageInfo damageInfo)
        {
            if (buffId == -1 || (damageInfo.GetSourceUnit() != null && damageInfo.GetSourceUnit().Buffs.Any(x => x.Id == buffId)))
            {
                damageInfo.FinalDamage *= rate;
            }
        }
    }
}
