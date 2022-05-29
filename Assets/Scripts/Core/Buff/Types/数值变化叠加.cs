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
        public int MaxLevel;

        public override void Init()
        {
            base.Init();
            Level = 1;
            MaxLevel = BuffData.Data.GetInt("MaxLevel");
        }

        public override void Reset()
        {
            base.Reset();
            Level++;
            if (Level > MaxLevel && MaxLevel != 0) Level = MaxLevel;
        }

        protected override float GetValue(int i)
        {
            return base.GetValue(i) * Level;
        }
    }
}
