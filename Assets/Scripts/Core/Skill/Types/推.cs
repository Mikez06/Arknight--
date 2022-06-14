using UnityEngine;

namespace Skills
{
    public class 推:Skill
    {
        protected override void addBuff(Unit target)
        {
            base.addBuff(target);
            if (target.Height > 0) return;
            int power;
            Vector2 direction;
            var angle = Vector2.Angle(target.Position2 - Unit.Position2, Unit.Direction);
            if ((Unit.Position2 - target.Position2).magnitude < 0.25f || angle > 45f)
            {
                power = getPower(SkillData.PushPower - 2 + (int)Unit.PushPower, target.Weight);
                direction = target.Position2 - Unit.Position2;
            }
            else
            {
                power = getPower(SkillData.PushPower + (int)Unit.PushPower, target.Weight);
                direction = Unit.Direction;
            }
            if (power > 0)
            {
                Debug.Log($"此次推力大小为{power}");
                var basePush = target.PushBuffs.Find(x => x is Buffs.推动 b && b.Skill == this) as Buffs.推动;
                if (basePush == null)
                {
                    Buffs.推动 push = new Buffs.推动();
                    push.Skill = this;
                    push.Unit = target;
                    push.Power = power;
                    push.Direction = direction;
                    target.AddPush(push);
                }
                else
                {
                    basePush.Power = power;
                    basePush.Direction = direction;
                }
            }
        }

        static int[] pow = new int[] { 0, 100, 200, 400, 450, 530, 580 };

        public static int getPower(int power,int weight)
        {
            int level = UnityEngine.Mathf.Clamp(power - weight, -3, 3);
            return pow[level + 3];
        }
    }
}
