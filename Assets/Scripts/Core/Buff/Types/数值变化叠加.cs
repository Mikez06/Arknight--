using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 数值变化叠加 : 数值变化
    {
        public int Level;

        public override void Init()
        {
            base.Init();
            Level = 1;
        }

        public override void Reset()
        {
            base.Reset();
            Level++;
        }

        protected override float GetValue(int i)
        {
            return base.GetValue(i) * Level;
        }
    }
}
