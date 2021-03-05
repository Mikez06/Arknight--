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
        public DirectionEnum Direction_E;

        public Vector2Int[] AttackPoints;

        public List<敌人> StopUnits = new List<敌人>();

        public CountDown Start = new CountDown();//入场

        public CountDown Reseting = new CountDown();

        public bool InMap = false;

        public float ResetTime;

        public override void Init()
        {
            base.Init();
            var baseAttackPoints = Skills[0].Config.AttackPoints;
            AttackPoints = new Vector2Int[baseAttackPoints.Length];

        }

        public override void Refresh()
        {
            base.Refresh();
        }

        public override void UpdateAction()
        {
            //不在战场也能转放置CD
            Reseting.Update(SystemConfig.DeltaTime);
            if (State == StateEnum.Default) return;

            if (!Start.Finished())
            {
                if (Start.Update(SystemConfig.DeltaTime))
                {
                    //Debug.Log("StartEnd" + Time.time);
                    State = StateEnum.Idle;
                    AnimationName = "Idle";
                    AnimationSpeed = 1;
                }
                return;
            }
            if (this.State == StateEnum.Start)
            {
                return;
            }
            if (this.State == StateEnum.Die)
            {
                UpdateDie();
            }
            else
            {
                UpdateSkills();
            }
            Recover.Update(SystemConfig.DeltaTime);
        }

        public void ChangePos(int x,int y, DirectionEnum directionEnum)
        {
            Position = Battle.Map.Grids[x, y].transform.position;
            Direction_E = directionEnum;
            var baseAttackPoints = Skills[0].Config.AttackPoints;
            for (int i = 0; i < baseAttackPoints.Length; i++)
            {
                switch (Direction_E)
                {
                    case DirectionEnum.Right:
                        Direction = new Vector2(0, 0);
                        AttackPoints[i] = GridPos + baseAttackPoints[i];
                        break;
                    case DirectionEnum.Left:
                        Direction = new Vector2(-1, 0);
                        AttackPoints[i] = GridPos - baseAttackPoints[i];
                        break;
                    case DirectionEnum.Up:
                        Direction = new Vector2(0, 1);
                        AttackPoints[i] = GridPos + new Vector2Int(baseAttackPoints[i].y, -baseAttackPoints[i].x);
                        break;
                    case DirectionEnum.Down:
                        Direction = new Vector2(0, -1);
                        AttackPoints[i] = GridPos + new Vector2Int(-baseAttackPoints[i].y, baseAttackPoints[i].x);
                        break;
                }
            }
        }

        public void JoinMap()
        {
            Start.Set(UnitModel.GetAnimationDuration("Start"));
            //Debug.Log("start:" + Time.time + "," + Start.value);
            State = StateEnum.Start;
            AnimationName = "Start";
            AnimationSpeed = 1;
            InMap = true;
            Battle.PlayerUnitsMap[GridPos.x, GridPos.y] = this;
        }

        public void LeaveMap()
        {
            State = StateEnum.Default;
            Direction = new Vector2(1, 0);
            InMap = false;
            Battle.PlayerUnitsMap[GridPos.x, GridPos.y] = null;
            Reseting.Set(ResetTime);
        }

        public int Cost()
        {
            return (int)(Config.Cost * (ResetTime == 0 ? 1 : ResetTime == 1 ? 1.5f : 2));
        }

        public bool Useable()
        {
            return Cost() <= Battle.Cost;
        }

        public override void DoDie()
        {
            base.DoDie();
            foreach (var unit in StopUnits)
            {
                unit.StopUnit = null;
            }
        }

        public bool CanStop(Units.敌人 target)
        {
            return StopUnits.Count < Config.StopCount;
        }
    }
}
