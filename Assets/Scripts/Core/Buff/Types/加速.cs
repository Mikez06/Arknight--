using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 加速 : Buff, ISelfDamageModify
    {
        int level, MaxLevel;
        float rate;
        float triggerTime;
        CountDown Trigger = new CountDown();
        bool clear = false;
        public override void Init()
        {
            base.Init();
            MaxLevel = BuffData.Data.GetInt("MaxLevel");
            rate = BuffData.Data.GetFloat("Rate");
            triggerTime = BuffData.Data.GetFloat("Trigger");
            Trigger.Set(triggerTime);
        }

        public override void Update()
        {
            base.Update();
            if (clear)
            {
                clear = false;
                level = 0;
                Trigger.Set(triggerTime);
            }
            if (Unit.IfStoped() || Unit.IfStun)
            {
                clear = true;
            }
            else
            {
                if (Trigger.Update(SystemConfig.DeltaTime))
                {
                    if (level < MaxLevel)
                    {
                        level++;
                    }
                    Trigger.Set(triggerTime);
                }
            }
        }

        public override void Apply()
        {
            base.Apply();
            Unit.SpeedRate += level * rate;
        }

        public void Modify(DamageInfo damageInfo)
        {
            if (level > 0)
            {
                damageInfo.Attack += Unit.Speed * 600 * 2;
                level = 0;
                Trigger.Set(triggerTime);
            }
        }
    }
}
