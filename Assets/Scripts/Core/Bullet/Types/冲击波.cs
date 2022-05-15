using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bullets
{
    public class 冲击波 : Bullet
    {
        public override void Init()
        {
            base.Init();
            Direction = TargetPos - this.Position;
        }
        public override void Update()
        {
            Vector3 delta = TargetPos - Position;
            if (delta.magnitude < BulletData.Speed * SystemConfig.DeltaTime)
            {
                Finish();
            }
            else
            {
                Position += delta.normalized * BulletData.Speed * SystemConfig.DeltaTime;
            }
            var target = Battle.FindAll(Position.ToV2(), 0.25f, Skill.SkillData.TargetTeam); //Battle.FindAll(new Vector2Int(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z)), Skill.SkillData.TargetTeam);
            if (target.Count > 0)
            {
                var t = target.FirstOrDefault();
                if (t.Alive())
                {
                    Skill.Hit(t, this);
                    Finish();
                }
            }
        }
    }
}
