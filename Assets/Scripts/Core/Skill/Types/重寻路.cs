using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 重寻路 : Skill
    {
        CountDown c = new CountDown();
        public override void Init()
        {
            base.Init();
        }

        public override void Update()
        {
            base.Update();
            /*延迟几帧进行重寻路防止出bug*/
            //if (c.Update(SystemConfig.DeltaTime))
            //{
                
            //}
        }

        public override void Start()
        {
            if (!Useable()) return;
            if (Targets.Count == 0)
            {
                FindTarget();
            }
            if (Targets.Count == 0 && !SkillData.NoTargetAlsoUse) return;
                UseCount++; AstarPath.active.Scan();
            //由于地块通行性发生变化，通知所有敌人
            foreach (var unit in Battle.Enemys)
            {
                (unit as Units.敌人).NeedResetPath = true;
            }
            //c.Set(0.05f);
        }
    }
}
