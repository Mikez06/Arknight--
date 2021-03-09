using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 激活技能 : Skill
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Start();
        }
    }
}
