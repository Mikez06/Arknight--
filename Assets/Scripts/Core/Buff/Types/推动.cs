using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Buffs
{
    public class 推动 : Buff,IPushBuff
    {
        public float Power;
        public Vector2 Direction;

        public override void Update()
        {
            Power -= 490 * SystemConfig.DeltaTime;
            //Debug.Log($"当前推力:{ GetPushPower()},{Time.time}");
            if (Power < 0) Unit.RemovePush(this);
            //base.Update();
        }

        public Vector2 GetPushPower()
        {
            return Direction.normalized * Power / 100;
        }
    }
}
