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

        public float PowerAll;

        public Unit Source;

        public float FullDuration;

        public float StartDistance;

        public override void Init()
        {
            Duration.Set(FullDuration);
            StartDistance = (Source.Position2 - Unit.Position2).magnitude;
            var effect = Skill.SkillData.Data.GetStr("PullEffect");
            if (!string.IsNullOrEmpty(effect))
            {
                LastingEffect = EffectManager.Instance.GetEffect(Database.Instance.GetIndex<EffectData>(effect));
                LastingEffect.Init(Unit, Unit, Unit.Position, Unit.Direction);
                LastingEffect.SetLifeTime(float.PositiveInfinity);
            }
        }

        public override void Update()
        {
            base.Update();

            float dis = (Source.Position2 - Unit.Position2).magnitude;
            if (dis < StartDistance)
            {
                dis = Mathf.Pow(dis / StartDistance, 4);
            }
            else
                dis = 1;
            if (dis > 1) dis = 1;
            PowerAll += dis * Power;
            PowerAll -= 490 * SystemConfig.DeltaTime;
        }

        public Vector2 GetPushPower()
        {
            if (Unit.IfStoped() && (Unit as Units.敌人).StopUnit == Source)
            {
                Finish();
                return Vector2.zero;
            }
            //float dis= (Source.Position2 - Unit.Position2).magnitude;
            //if (dis < StartDistance)
            //{
            //    dis = Mathf.Pow(dis / StartDistance, 4);
            //}
            //else
            //    dis = 1;
            //if (dis > 1) dis = 1;
            //var result= (Source.Position2 - Unit.Position2).normalized * Power / 100f * dis;
            //Debug.Log("拉力:" + result.x + "," + result.y);
            return (Source.Position2 - Unit.Position2).normalized * PowerAll / 100f;
        }
        public override void Finish()
        {
            Unit.PushBuffs.Remove(this);
            base.Finish();
        }
    }
}
