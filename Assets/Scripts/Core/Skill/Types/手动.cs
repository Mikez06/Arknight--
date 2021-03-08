using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    /// <summary>
    /// 手动技能一定会用到Unit的Power，当
    /// </summary>
    public class 手动 : 主动
    {
        public bool Open;

        public override void Update()
        {
            base.Update();
            if (Open)
            {
                Open = true;
            }
        }

        public override void UpdateCooldown()
        {
            if (this == Unit.MainSkill && !Cooldown.Finished())
            {
                if (Cooldown.Update(SystemConfig.DeltaTime))
                {
                    OnClose();
                }
                Unit.Power -= Config.MaxPower / Config.Cooldown * SystemConfig.DeltaTime;
            }
            else
            {
                Cooldown.Update(SystemConfig.DeltaTime);
            }
        }

        public override bool Ready()
        {
            return Open && base.Ready();
        }

        public void OpenSkill()
        {
            Open = true;
            OnOpen();
        }

        protected virtual void OnOpen()
        {

        }

        protected virtual void OnClose()
        {

        }
    }
}
