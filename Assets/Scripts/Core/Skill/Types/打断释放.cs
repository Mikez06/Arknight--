using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 打断释放:Skill
    {
        public override void BreakCast()
        {
            if (!Casting.Finished() && Unit.Alive())
            {
                Cast();
            }
            base.BreakCast();
        }
    }
}
