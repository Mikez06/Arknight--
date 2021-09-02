using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Units
{
    public class 普通单位 : Unit
    {
        public override void UpdateAction()
        {
            base.UpdateAction();

            if (this.State == StateEnum.Die)
            {
                UpdateDie();
            }
            else
            {
                UpdateSkills();
            }
        }
    }
}
