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
        public Card Card;
        public int MainSkillId;

        public DirectionEnum Direction_E;

        public List<敌人> StopUnits = new List<敌人>();

        public CountDown Start = new CountDown();//入场

        public CountDown Reseting = new CountDown();

        /// <summary>
        /// 干员是第几帧进入场地的
        /// </summary>
        public int InputTime = -1;

        public float ResetTime;

        /// <summary>
        /// 建造次数
        /// </summary>
        public int BuildTime;

        public int Cost;

        public override void Init()
        {
            base.Init();
            MainSkill = LearnSkill(Config.MainSkill[MainSkillId]);
        }

        public override void Refresh()
        {
            base.Refresh();
            Cost = Config.Cost;
            ResetTime = Config.ResetTime;
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
                    SetStatus(StateEnum.Idle);
                }
                return;
            }
            if (this.State == StateEnum.Start)
            {
                return;
            }
            if (ScaleX != TargetScaleX)
            {
                var delta = Math.Sign(TargetScaleX - ScaleX) / SystemConfig.TurningTime * SystemConfig.DeltaTime;
                if ((TargetScaleX - ScaleX) < delta)
                {
                    TargetScaleX = ScaleX;
                }
                else
                    ScaleX += delta;
            }

            if (MainSkill != null && MainSkill.Config.PowerType == PowerRecoverTypeEnum.自动)
            {
                RecoverPower(PowerSpeed * SystemConfig.DeltaTime);
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
            switch (Direction_E)
            {
                case DirectionEnum.Right:
                    ScaleX = TargetScaleX = 1;
                    Direction = new Vector2(1, 0);
                    break;
                case DirectionEnum.Left:
                    ScaleX = TargetScaleX = -1;
                    Direction = new Vector2(-1, 0);
                    break;
                case DirectionEnum.Up:
                    Direction = new Vector2(0, -1);
                    break;
                case DirectionEnum.Down:
                    Direction = new Vector2(0, 1);
                    break;
            }
            foreach (var skill in Skills)
            {
                skill.UpdateAttackPoints();
            }
        }

        public bool CanBuild()
        {
            return GetCost() <= Battle.Cost;
        }

        public void JoinMap()
        {
            Debug.Log("StartStart" + Time.time);
            Battle.Cost -= GetCost();
            Start.Set(UnitModel.GetAnimationDuration("Start"));
            CheckBlock();
            //Debug.Log("start:" + Time.time + "," + Start.value);
            SetStatus(StateEnum.Start);
            InputTime = Battle.Tick;
            Battle.Map.Grids[GridPos.x, GridPos.y].Unit = this;
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);
        }

        public void LeaveMap()
        {
            UnitModel.gameObject.SetActive(false);
            BattleUI.UI_Battle.Instance.ReturnUIUnit(this);
            State = StateEnum.Default;
            Direction = new Vector2(1, 0);
            InputTime = -1;
            Battle.Map.Grids[GridPos.x, GridPos.y].Unit = null;
            Reseting.Set(ResetTime);
            BuildTime++;
            Battle.Cost += Cost / 2;
            foreach (var unit in StopUnits)
            {
                unit.StopUnit = null;
            }
            StopUnits.Clear();
        }

        public override void Finish()
        {
            LeaveMap();
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

        public int GetCost()
        {
            return (int)(Cost * (BuildTime == 0 ? 1 : BuildTime == 1 ? 1.5f : 2));
        }

        public bool Useable()
        {
            return GetCost() <= Battle.Cost;
        }

        public override void DoDie()
        {
            base.DoDie();
            foreach (var unit in StopUnits)
            {
                unit.StopUnit = null;
            }
            StopUnits.Clear();
        }

        public bool CanStop(Units.敌人 target)
        {
            if (target.StopUnit != null) return false;
            if (StopUnits.Contains(target)) return true;
            return StopUnits.Count < Config.StopCount;
        }

        public override float Hatred()
        {
            return base.Hatred() + InputTime;
        }

        public override bool IfStoped()
        {
            return StopUnits.Count > 0;
        }

        void CheckBlock()
        {
            var blockUnits = Battle.FindAll(Position2, Config.Radius, 1);
            foreach (Units.敌人 u in blockUnits)
            {
                if (CanStop(u))
                {
                    u.StopUnit = this;
                    StopUnits.Add(u);
                    var pos = u.Position2 + (u.Position2 - Position2).normalized * (u.Config.Radius + Config.Radius);
                    u.Position = new Vector3(pos.x, Position.y, pos.y);
                }
            }
        }
    }
}
