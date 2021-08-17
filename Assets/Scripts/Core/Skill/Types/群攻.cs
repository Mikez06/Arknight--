using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 群攻:近战
    {
        protected override int GetTargetCount()
        {
            int result = (Unit as Units.干员).StopCount;
            foreach (var modify in Modifies)
            {
                if (modify is ITargetModify targetModify)
                {
                    result = targetModify.Modify(result);
                }
            }
            return result;
        }
    }
}
