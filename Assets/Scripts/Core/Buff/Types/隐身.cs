using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 隐身 : Buff
    {
        CountDown rehide = new CountDown();
        float rehideTime;

        public override void Init()
        {
            base.Init();
            rehideTime = this.BuffData.Data.GetFloat("HideTime");
        }

        public override void Update()
        {
            base.Update();
            if (Unit.IfStoped())
            {
                LastingEffect?.gameObject.SetActive(false);
                rehide.Set(rehideTime);
            }
            rehide.Update(SystemConfig.DeltaTime);
            //Log.Debug($"{Unit.UnitData.Id}隐身了");
            if (rehide.Finished())
            {
                LastingEffect?.gameObject.SetActive(true);
                Unit.IfHide = true;
            }
        }
    }
}
