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
            if (datas != null)
            {
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
            var datas1 = BuffData.Data.GetArray("DieAnimation");
            if (datas1 != null)
            {
                var names = new string[datas1.Length];
                for (int i = 0; i < datas1.Length; i++)
                {
                    names[i] = Convert.ToString(datas1[i]);
                }
                if (BuffData.Data.GetBool("AllOverWrite"))
                {
                    Unit.OverWriteAnimation = names;
                }
                else
                    Unit.OverWriteDie = names;
            }
            var datas2 = BuffData.Data.GetArray("MoveAnimation");
            if (datas2 != null)
            {
                var names = new string[datas2.Length];
                for (int i = 0; i < datas2.Length; i++)
                {
                    names[i] = Convert.ToString(datas2[i]);
                }
                if (BuffData.Data.GetBool("AllOverWrite"))
                {
                    Unit.OverWriteAnimation = names;
                }
                else
                    Unit.OverWriteMove = names;
            }
            Unit.SetStatus(Unit.State);
        }

        public override void Finish()
        {
            base.Finish();
            Unit.OverWriteAnimation = null;
            Unit.OverWriteIdle = null;
            Unit.OverWriteDie = null;
        }
    }
}
