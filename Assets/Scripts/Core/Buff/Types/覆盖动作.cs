using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 覆盖动作 : Buff
    {
        public override void Init()
        {
            base.Init();
            var datas = BuffData.Data.GetArray("IdleAnimation");
            var names = new string[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                names[i] = Convert.ToString(datas[i]);
            }
            if (BuffData.Data.GetBool("AllOverWrite"))
            {
                Unit.OverWriteAnimation = names;
            }
            else
                Unit.OverWriteIdle = names;
        }

        public override void Finish()
        {
            base.Finish();
            Unit.OverWriteAnimation = null;
            Unit.OverWriteIdle = null;
        }
    }
}
