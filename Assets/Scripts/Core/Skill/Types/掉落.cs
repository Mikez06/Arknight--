using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 掉落 : Skill
    {
        float DropSpeed = 1;
        public override void Init()
        {
            base.Init();
            DropSpeed = SkillData.Data.GetFloat("DropSpeed");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Hit(Unit target, Bullet bullet = null)
        {
            //base.Hit(target);
            var X = Unit.GridPos.x;
            var Y = Unit.GridPos.y;
            if (target.Position.x <  X - 0.5f || target.Position.x > X + 0.5f || target.Position.z < Y - 0.5f || target.Position.z > Y + 0.5f) return;
            if (target.Start.Finished())
            {
                if (target.Alive())
                {
                    target.DoDie(this);
                }
                target.Position.y -= DropSpeed * SystemConfig.DeltaTime;
            }
            target.unbalance = false;
        }
    }
}
