using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 召唤绑定 : Skill
    {
        public override void Update()
        {
            var children = (Unit as Units.干员).Children;
            if (children.Count > 0 && children[0].InputTime < 0)
            {
                if (!Opening.Finished())
                {
                    Opening.Finish();
                    OnOpenEnd();
                }
                Power = 0;
            }
            base.Update();
        }
    }
}
