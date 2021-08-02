using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 测试Buff:Buff
    {
        public override void Apply()
        {
            base.Apply();
            Log.Debug($"{Unit.UnitData.Id}加了buff：{Config.Id}");
        }

        public override void Reset()
        {
            Log.Debug($"{Unit.UnitData.Id}更新buff：{Config.Id}");
            base.Reset();
        }
    }
}
