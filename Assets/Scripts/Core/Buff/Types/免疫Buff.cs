using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 免疫Buff : Buff
    {
        List<int> buffs = new List<int>();
        public override void Init()
        {
            base.Init();
            var datas = BuffData.Data.GetArray("Buffs");
            if (datas != null)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    int buffId = Database.Instance.GetIndex<BuffData>(Convert.ToString(datas[i]));
                    if (!Unit.IgnoreBuffs.Contains(buffId))
                    {
                        buffs.Add(buffId);
                        Unit.IgnoreBuffs.Add(buffId);
                    }
                }
            }
        }
        public override void Finish()
        {
            foreach (var id in buffs)
            {
                Unit.IgnoreBuffs.Remove(id);
            }
            buffs.Clear();
            base.Finish();
        }
    }
}
