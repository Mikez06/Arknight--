﻿using System;
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
            if (target.Height > 0) return;
            float power = getPower(SkillData.PushPower, target.Weight);

            if (power > 0)
            {
                Buffs.拉动 push = new Buffs.拉动();
                push.Skill = this;
                push.Source = Unit;
                push.Unit = target;
                push.Power = getPower(SkillData.PushPower, target.Weight);
                if (SkillData.PushPower - target.Weight > -1) push.FullDuration = 1;
                else push.FullDuration = 0.5f;
                push.Init();
                target.AddPush(push);
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
