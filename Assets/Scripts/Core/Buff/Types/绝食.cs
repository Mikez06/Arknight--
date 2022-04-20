using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 绝食 : Buff
    {
        int unitId = -1;
        public override void Apply()
        {
            base.Apply();
            Unit.CanBeHeal = false;
            var str= BuffData.Data.GetStr("HealOnly");
            if (!string.IsNullOrEmpty(str))
            {
                unitId = Database.Instance.GetIndex<UnitData>(str);
                Unit.HealOnly.Add(unitId);
            }
        }

        public override void Finish()
        {
            base.Finish();
            Unit.HealOnly.Remove(unitId);
        }
    }
}
