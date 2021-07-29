namespace Skills
{
    public class 推:Skill
    {
        protected override void addBuff(Unit target)
        {
            base.addBuff(target);
            Buffs.推动 push = new Buffs.推动();
            target.AddBuff(push);
            if ((Unit.Position2 - target.Position2).magnitude < 0.25f)
            {
                //溅射型推力
                push.Power = getPower(Config.PushPower-2, target.Weight);
                push.Direction = target.Position2 - Unit.Position2;
            }
            else
            {
                push.Power= getPower(Config.PushPower, target.Weight);
                push.Direction = Unit.Direction;
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
