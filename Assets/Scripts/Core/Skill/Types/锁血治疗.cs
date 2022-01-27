using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 锁血治疗 : Skill
    {
        public CountDown HealStarting = new CountDown();
        public CountDown Healing = new CountDown();


        float healCount;

        public override void Init()
        {
            base.Init();
        }

        public override void Update()
        {
            //base.Update();
            if (HealStarting.Update(SystemConfig.DeltaTime))
            {
                startHeal();
            }
            if (!Healing.Finished())
            {
                if (Healing.Update(SystemConfig.DeltaTime))
                {
                    Unit.SetStatus(StateEnum.Idle);
                    Unit.Hp= healCount;
                }
                else
                {

                    Unit.Hp += healCount * SystemConfig.DeltaTime;
                }
            }
        }

        public override void Cast()
        {
            var targetHp = SkillData.Data.GetFloat("LockHp") * Unit.MaxHp;
            if (targetHp <= 0) targetHp = 1;
            if (targetHp > Unit.Hp)
                Unit.Hp = targetHp;
            healCount = SkillData.Data.GetFloat("HealCount") * Unit.MaxHp;
            if (healCount != 0)
            {
                HealStarting.Set(SkillData.Data.GetFloat("HealStart"));
                if (HealStarting.Finished())
                {
                    startHeal();
                }
            }
            base.Cast();
        }

        void startHeal()
        {
            Healing.Set(SkillData.Data.GetFloat("HealTime"));
        }
    }
}
