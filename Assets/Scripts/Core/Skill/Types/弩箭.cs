using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skills
{
    public class 弩箭 : 非指向技能
    {
        public override void Effect(Unit target)
        {
            if (SkillData.Bullet != null)
            {
                //创建一个子弹
                var startPoint = Unit.UnitModel.GetPoint(SkillData.ShootPoint);
                //Debug.Log($"攻击{target.Config.Name}:{target.Hp} 起点：{startPoint}");
                Battle.CreateBullet(SkillData.Bullet.Value, startPoint, startPoint + new Vector3(Unit.Direction.x, 0, Unit.Direction.y).normalized * 20f, target, this);
            }
        }
    }
}
