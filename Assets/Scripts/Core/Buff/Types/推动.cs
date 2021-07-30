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
        public int Power;
        public Vector2 Direction;

        public override void Update()
        {
            //base.Update();
        }

        public Vector2 GetPushPower()
        {
            return Direction;
        }
    }
}
