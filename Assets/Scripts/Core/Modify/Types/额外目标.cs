using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modifys
{
    public class 额外目标 : Modify, ITargetModify
    {
        public float Chance;
        public int Count;
        public override void Init()
        {
            base.Init();
            Chance = ModifyData.Data.GetFloat("Chance");
            Count = ModifyData.Data.GetInt("Count");
        }
        public int Modify(int count)
        {
            if (Battle.Random.NextDouble() < Chance)
            {
                count += Count;
            }
            return count;
        }
    }
}
