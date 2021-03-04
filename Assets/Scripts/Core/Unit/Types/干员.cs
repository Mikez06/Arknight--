using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units
{
    public class 干员 : Unit
    {
        public DirectionEnum Direction;

        public Vector2Int[] AttackPoints;

        public List<敌人> StopUnits = new List<敌人>();

        public override void Init()
        {
            base.Init();
            var baseAttackPoints = Skills[0].Config.AttackPoints;
            AttackPoints = new Vector2Int[baseAttackPoints.Length];

            for (int i = 0; i < baseAttackPoints.Length; i++)
            {
                switch (Direction)
                {
                    case DirectionEnum.Right:
                        AttackPoints[i] = GridPos + baseAttackPoints[i];
                        break;
                    case DirectionEnum.Left:
                        AttackPoints[i] = GridPos - baseAttackPoints[i];
                        break;
                    case DirectionEnum.Up:
                        AttackPoints[i] = GridPos + new Vector2Int(baseAttackPoints[i].y, -baseAttackPoints[i].x);
                        break;
                    case DirectionEnum.Down:
                        AttackPoints[i] = GridPos + new Vector2Int(-baseAttackPoints[i].y, baseAttackPoints[i].x);
                        break;
                }
            }
        }

        public override void DoDie()
        {
            base.DoDie();
            foreach (var unit in StopUnits)
            {
                unit.StopUnit = null;
            }
        }
    }
}
