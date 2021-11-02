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
        public override void Update()
        {
            Vector3 delta = TargetPos - Postion;
            if (delta.magnitude < Config.Speed * SystemConfig.DeltaTime)
            {
                Finish();
            }
            else
            {
                Postion += delta.normalized * Config.Speed * SystemConfig.DeltaTime;
            }
            var target = Battle.FindAll(new Vector2Int(Mathf.RoundToInt(Postion.x), Mathf.RoundToInt(Postion.z)), 1);
            if (target.Count > 0)
            {
                var t = target.FirstOrDefault();
                if (t.Alive())
                {
                    Skill.Hit(t);
                    Finish();
                }
            }
        }
    }
}
