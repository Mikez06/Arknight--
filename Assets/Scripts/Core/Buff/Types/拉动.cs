using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Buffs
{
    public class 拉动 : Buff,IPushBuff
    {
        public int Power;

        public Unit source;

        public float FullDuration;

        public override void Init()
        {
            Duration.Set(FullDuration);
        }
        public Vector2 GetPushPower()
        {
            return (source.Position2 - Unit.Position2).normalized;
        }
    }
}
