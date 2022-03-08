using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 修改地块通行性 : Skill
    {
        bool changeToWalk;

        public override void Init()
        {
            base.Init();
            changeToWalk = SkillData.Data.GetBool("Walkable");
        }

        public override void Start()
        {
            if (!Useable()) return;
            UseCount++;
            if (AttackPoints == null)
            {
                return;
            }
            foreach (var point in AttackPoints)
            {
                Battle.Map.Tiles[point.x, point.y].CanMove = changeToWalk;
            }
            //由于地块通行性发生变化，通知所有敌人
            foreach (var unit in Battle.Enemys)
            {
                (unit as Units.敌人).NeedResetPath = true;
            }
        }
    }
}
