using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 设置属性:Buff
    {
        public override void Init()
        {
            base.Init();
        }

        public override void Apply()
        {
            foreach (var data in Config.Data)
            {
                Unit.GetType().GetField(data.Key).SetValue(Unit, data.Value);
            }
        }
    }
}
