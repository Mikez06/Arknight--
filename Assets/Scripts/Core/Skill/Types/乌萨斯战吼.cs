using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 乌萨斯战吼 : 获得费用
    {
        public override void FindTarget()
        {
            if (Unit.MainSkill.Opening.Finished()) return;
            Targets.Clear();
            var t = Battle.TriggerDatas.Peek().Target;
            var t1 = Battle.TriggerDatas.Peek().User;
            if (t1 != null && t1.UnitData.Profession == UnitTypeEnum.先锋)
            {
                Targets.Add(t1);
            }
        }
    }
}
