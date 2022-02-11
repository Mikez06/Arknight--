using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    public class 延迟打击 : Bullet
    {
        public CountDown Delay = new CountDown();

        public override void Init()
        {
            base.Init();
            Position = Target.Position;
            Delay.Set(BulletData.Data.GetFloat("delay"));        
        }

        public override void Update()
        {
            Delay.Update(SystemConfig.DeltaTime);
            if (Delay.Finished())
            {
                Skill.Hit(Target, this);
                Finish();
            }
        }
    }
}
