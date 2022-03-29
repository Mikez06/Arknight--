using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 拉 : Skill
    {
        public Buffs.拉动 pull;
        protected override void addBuff(Unit target)
        {
            base.addBuff(target);
            if (target.Height > 0) return;
            float power = getPower(SkillData.PushPower + (int)Unit.PushPower, target.Weight);

            if (power > 0)
            {
                pull = new Buffs.拉动();
                pull.Skill = this;
                pull.Source = Unit;
                pull.Unit = target;
                pull.Power = getPower(SkillData.PushPower + (int)Unit.PushPower, target.Weight);
                if (SkillData.PushPower - target.Weight > -1) pull.FullDuration = 1;
                else pull.FullDuration = 0.5f;
                pull.Init();
                target.AddPush(pull);
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
