using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 拉 : Skill
    {
        protected override void addBuff(Unit target)
        {
            base.addBuff(target);
            Buffs.拉动 push = new Buffs.拉动();
            target.AddBuff(push);
            if ((Unit.Position2 - target.Position2).magnitude < 0.25f)
            {
                //溅射型推力
                push.Power = getPower(Config.PushPower - 2, target.Weight);
                push.source = Unit;
            }
            else
            {
                push.Power = getPower(Config.PushPower, target.Weight);
                push.source = Unit;
            }
        }
        static int[] pow = new int[] { 0, 2, 10, 40, 42, 44, 46 };

        public static int getPower(int power, int weight)
        {
            int level = UnityEngine.Mathf.Clamp(power - weight, -3, 3);
            return pow[level + 3];
        }
    }
}
