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

        public List<Vector2Int> AttackPoints = new List<Vector2Int>();

        public List<敌人> StopUnits = new List<敌人>();

        public CountDown Start = new CountDown();//入场

        public CountDown Reseting = new CountDown();

        /// <summary>
        /// 干员是第几个进入场地的
        /// </summary>
        public int MapIndex = -1;

        public float ResetTime;

        public override void Init()
        {
            base.Init();
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
            UnitModel.gameObject.SetActive(true);
            Position = Battle.Map.Grids[x, y].transform.position;
            Direction_E = directionEnum;
            ResetAttackPoint();
        }

        public void ResetAttackPoint()
        {
            var baseAttackPoints = Skills[0].Config.AttackPoints;
            switch (Direction_E)
            {
                case DirectionEnum.Right:
                    Direction = new Vector2(0, 0);
                    break;
                case DirectionEnum.Left:
                    Direction = new Vector2(-1, 0);
                    break;
                case DirectionEnum.Up:
                    Direction = new Vector2(0, 1);
                    break;
                case DirectionEnum.Down:
                    Direction = new Vector2(0, -1);
                    break;
            }
            AttackPoints.Clear();
            for (int i = 0; i < baseAttackPoints.Length; i++)
            {
                AttackPoints.Add(PointWithDirection(baseAttackPoints[i]));
            }
            foreach (var point in AttackPoints.ToArray())
            {
                if (point.x < 0 || point.x >= Battle.Map.Grids.GetLength(0) || point.y < 0 || point.y >= Battle.Map.Grids.GetLength(1))
                {
                    AttackPoints.Remove(point);
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
            MapIndex = Battle.NowUnitIndex;
            Battle.NowUnitIndex++;
            Battle.Map.Grids[GridPos.x, GridPos.y].Unit = this;
        }

        public void LeaveMap()
        {
            State = StateEnum.Default;
            Direction = new Vector2(1, 0);
            MapIndex = -1;
            Battle.Map.Grids[GridPos.x, GridPos.y].Unit = null;
            Reseting.Set(ResetTime);
        }

        public Vector2Int PointWithDirection(Vector2Int point)
        {
            switch (Direction_E)
            {
                case DirectionEnum.Right:
                    return(GridPos + point);
                case DirectionEnum.Left:
                    return (GridPos - point);
                case DirectionEnum.Up:
                    return (GridPos + new Vector2Int(point.y, -point.x));
                case DirectionEnum.Down:
                    return (GridPos + new Vector2Int(-point.y, point.x));
            }
            return GridPos;
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
