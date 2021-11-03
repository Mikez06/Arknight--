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

        public float StartDistance;

        public override void Init()
        {
            Duration.Set(FullDuration);
            StartDistance = (source.Position2 - Unit.Position2).magnitude;
        }

        public Vector2 GetPushPower()
        {
            if (Unit.IfStoped()) return Vector2.zero;
            float dis= (source.Position2 - Unit.Position2).magnitude;
            if (dis < StartDistance)
            {
                dis = Mathf.Pow(dis / StartDistance, 4);
            }
            else
                dis = 1;
            return (source.Position2 - Unit.Position2).normalized * Power / 100f * dis;
        }
        public override void Finish()
        {
            Unit.PushBuffs.Remove(this);
            base.Finish();
        }
    }
}
