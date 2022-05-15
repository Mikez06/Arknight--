using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 召唤 : Skill
    {
        WaveInfo WaveInfo;
        float range;
        int count;
        public override void Init()
        {
            base.Init();
            WaveInfo = JsonHelper.Clone((Unit as Units.敌人).WaveData);
            WaveInfo.sUnitId = SkillData.Data.GetStr("UnitId");
            range = SkillData.Data.GetFloat("Range");
            count = SkillData.Data.GetInt("Count", 1);
        }
        public override void Effect(Unit target)
        {
            base.Effect(target);
            var parent = Unit as Units.敌人;

            //这里确保新生成的怪不会跑到其他格子上
            float xMin = parent.GridPos.x - 0.5f;
            float xMax = parent.GridPos.x + 0.5f;
            float yMin = parent.GridPos.y - 0.5f;
            float yMax = parent.GridPos.y + 0.5f;
            if (Unit.Position2.x - range > xMin) xMin = parent.Position2.x - range;
            if (Unit.Position2.x + range < xMax) xMax = parent.Position2.x + range;
            if (Unit.Position2.y - range > yMin) yMin = parent.Position2.y - range;
            if (Unit.Position2.y + range < yMax) yMax = parent.Position2.y + range;
            for (int i = 0; i < count; i++)
            {
                var unit = Battle.CreateEnemy(WaveInfo);
                unit.Position = new UnityEngine.Vector3(Battle.NextFloat(xMin, xMax), parent.Position.y, Battle.NextFloat(yMin, yMax)); //parent.Position;
                unit.NowPathPoint = parent.NowPathPoint;
                unit.Parent = parent;
            }
        }
    }
}
