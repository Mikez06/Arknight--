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
            if (Target != null)
            {
                TargetPos = Target.Position;
                Position = Target.Position;
            }
            Delay.Set(BulletData.Data.GetFloat("delay"));
            BulletModel.transform.localScale = new UnityEngine.Vector3(Skill.Unit.ScaleX, 1, 1);

        }

        public override void Update()
        {
            Delay.Update(SystemConfig.DeltaTime);
            if (Delay.Finished())
            {
                if (Target == null)
                {
                    Skill.Hit(TargetPos.ToV2(), this);
                }
                else if (Target.Alive())
                {
                    Skill.Hit(Target, this);
                }
                Finish();
            }
        }
    }
}
