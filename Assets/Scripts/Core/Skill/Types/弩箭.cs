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
        public int StartPos;
        public int Line;

        public override void Init()
        {
            base.Init();
            StartPos = SkillData.Data.GetInt("StartPos");
            Line = SkillData.Data.GetInt("Line");
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Cast()
        {
            base.Cast();
        }

        public override void Effect(Unit target)
        {
            if (SkillData.Bullet != null)
            {
                //创建一个子弹
                Vector3 startPoint = Vector3.zero; 
                switch (StartPos)
                {
                    case 0:
                        startPoint = Unit.UnitModel.GetPoint(SkillData.ShootPoint);
                        Battle.CreateBullet(SkillData.Bullet.Value, startPoint, startPoint + new Vector3(Unit.Direction.x, 0, Unit.Direction.y).normalized * 20f, target, this);
                        break;
                    case 1:
                        startPoint = Unit.NowGrid.Pos;
                        startPoint.x = -2;
                        for (int i = -Line; i <= Line; i++)
                        {
                            var j = startPoint.z + i;
                            if (j >= 0 && j < Battle.Map.Tiles.GetLength(1))
                                Battle.CreateBullet(SkillData.Bullet.Value, startPoint + i * new Vector3(0, 0, 1), startPoint + new Vector3(1, 0, 0) * 20f + i * new Vector3(0, 0, 1), target, this);
                        }
                        break;
                    case 2:
                        startPoint = Unit.NowGrid.Pos;
                        startPoint.z = -2;
                        for (int i = -Line; i <= Line; i++)
                        {
                            var j = startPoint.x + i;
                            if (j >= 0 && j < Battle.Map.Tiles.GetLength(0))
                                Battle.CreateBullet(SkillData.Bullet.Value, startPoint + i * new Vector3(0, 1, 0), startPoint + new Vector3(0, 0, 1) * 20f + i * new Vector3(0, 1, 0), target, this);
                        }
                        break;
                    case 3:
                        startPoint = Unit.NowGrid.Pos;
                        startPoint.x = Battle.Map.Tiles.GetLength(0) + 2;
                        for (int i = -Line; i <= Line; i++)
                        {
                            var j = startPoint.z + i;
                            if (j >= 0 && j < Battle.Map.Tiles.GetLength(1))
                                Battle.CreateBullet(SkillData.Bullet.Value, startPoint + i * new Vector3(0, 0, 1), startPoint + new Vector3(-1, 0, 0) * 20f + i * new Vector3(0, 0, 1), target, this);
                        }
                        break;
                    case 4:
                        startPoint = Unit.NowGrid.Pos;
                        startPoint.x = Battle.Map.Tiles.GetLength(1) + 2;
                        for (int i = -Line; i <= Line; i++)
                        {
                            var j = startPoint.x + i;
                            if (j >= 0 && j < Battle.Map.Tiles.GetLength(0))
                                Battle.CreateBullet(SkillData.Bullet.Value, startPoint + i * new Vector3(0, 1, 0), startPoint + new Vector3(0, 0, -1) * 20f + i * new Vector3(0, 1, 0), target, this);
                        }
                        break;
                    default:
                        break;
                }
                //Debug.Log($"攻击{target.Config.Name}:{target.Hp} 起点：{startPoint}");
            }
        }

        public override void BreakCast()
        {
            base.BreakCast();
        }
    }
}
