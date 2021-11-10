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
        public int MainSkillId = -1;

        public DirectionEnum Direction_E;

        public List<敌人> StopUnits = new List<敌人>();

        public CountDown Start = new CountDown();//入场

        public CountDown Reseting = new CountDown();

        /// <summary>
        /// 干员是第几帧进入场地的,-1表示未放置
        /// </summary>
        public int InputTime = -1;

        /// <summary>
        /// 再部署时间
        /// </summary>
        public float ResetTime;
        public float ResetTimeBase, ResetTimeAdd, ResetTimeRate;

        /// <summary>
        /// 建造次数
        /// </summary>
        public int BuildTime;
        public int StopCount;

        public float Cost;
        public float CostBase, CostAdd;

        public 干员 Parent;
        public List<干员> Children = new List<干员>();

        public override void Init()
        {
            base.Init();
            if (MainSkillId >= 0)
                MainSkill = LearnSkill(UnitData.MainSkill[MainSkillId]);
        }

        protected override void baseAttributeInit()
        {
            base.baseAttributeInit();
            CostBase = UnitData.Cost + UnitData.CostEx;
            ResetTimeBase = UnitData.ResetTime + UnitData.ResetTimeEx;
        }

        public override void Refresh()
        {
            CostAdd = 0;
            ResetTimeAdd = ResetTimeRate = 0;
            StopCount = UnitData.StopCount;
            base.Refresh();
            Cost = CostBase + CostAdd;
            ResetTime = (ResetTimeBase + ResetTimeAdd) * (1 + ResetTimeRate);
        }

        public override void UpdateAction()
        {
            base.UpdateAction();
            //不管怎么样 都要检测阻挡是否已经失效
            foreach (var target in StopUnits.ToList())
            {
                if ((target.Position2 - Position2).magnitude > UnitData.Radius + target.UnitData.Radius + 敌人.StopExCheck || !CanStop(target))
                {
                    Debug.Log("移除阻挡");
                    RemoveStop(target);
                }
            }

            //不在战场也能转放置CD
            Reseting.Update(SystemConfig.DeltaTime);
            if (State == StateEnum.Default) return;

            if (!Start.Finished() && State != StateEnum.Die)
            {
                if (Start.Update(SystemConfig.DeltaTime))
                {
                    Battle.TriggerDatas.Push(new TriggerData()
                    {
                        Target = this,
                    });
                    Trigger(TriggerEnum.落地);
                    Battle.TriggerDatas.Pop();
                    hideBase = false;
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
                if (Mathf.Abs(TargetScaleX - ScaleX) < Mathf.Abs(delta))
                {
                    ScaleX = TargetScaleX;
                }
                else
                    ScaleX += delta;
            }

            if (this.State == StateEnum.Die)
            {
                UpdateDie();
            }
            else
            {
                UpdateSkills();
            }
            //Recover.Update(SystemConfig.DeltaTime);
        }

        public void ChangePos(int x,int y, DirectionEnum directionEnum)
        {
            UnitModel.gameObject.SetActive(true);
            Position = Battle.Map.Tiles[x, y].Pos;
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
            hideBase = true;
            Battle.Cost -= GetCost();
            Battle.BuildCount -= UnitData.BuildCountCost;
            Hp = MaxHp;
            Start.Set(UnitModel.GetAnimationDuration("Start"));
            CheckBlock();
            //Debug.Log("start:" + Time.time + "," + Start.value);
            SetStatus(StateEnum.Start);
            InputTime = Battle.Tick;
            Battle.Map.Tiles[GridPos.x, GridPos.y].Unit = this;
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);
            Battle.TriggerDatas.Push(new TriggerData()
            {
                Target = this,
            });
            Battle.Trigger(TriggerEnum.入场);
            Battle.TriggerDatas.Pop();
        }

        public void LeaveMap(bool recoverPower = false)
        {
            UnitModel.gameObject.SetActive(false);
            BattleUI.UI_Battle.Instance.ReturnUIUnit(this);
            State = StateEnum.Default;
            Direction = new Vector2(1, 0);
            InputTime = -1;
            Battle.Map.Tiles[GridPos.x, GridPos.y].Unit = null;
            Reseting.Set(ResetTime);
            BuildTime++;
            if (recoverPower)
                Battle.Cost += Mathf.FloorToInt(UnitData.Cost * UnitData.LeaveReturn);
            Battle.BuildCount += UnitData.BuildCountCost;
            foreach (var unit in StopUnits)
            {
                unit.StopUnit = null;
            }
            StopUnits.Clear();
            if (UnitData.NotReturn)//消耗品
            {
                Battle.PlayerUnits.Remove(this);
                Battle.AllUnits.Remove(this);
                if (Parent != null) Parent.Children.Remove(this);
            }

            BattleUI.UI_Battle.Instance.UpdateUnitsLayout();
            foreach (var skill in Skills)
            {
                skill.Reset();
            }
        }

        public override void Finish()
        {
            LeaveMap();
            base.Finish();
        }

        public override Vector2Int PointWithDirection(Vector2Int point)
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
            return (int)(Cost * (BuildTime == 0 ? 1 : BuildTime == 1 ? 1 + CostAdd : 1 + CostAdd * 2));
        }

        public bool Useable()
        {
            return GetCost() <= Battle.Cost && Battle.BuildCount >= UnitData.BuildCountCost;
        }

        public override void DoDie(object source)
        {
            base.DoDie(source);

            foreach (var unit in Children)
            {
                (unit as 干员).DoDie(null);
            }
            Children.Clear();

            foreach (var unit in StopUnits)
            {
                unit.StopUnit = null;
            }
            StopUnits.Clear();
        }

        public void AddStop(Units.敌人 target)
        {
            StopUnits.Add(target);
            target.StopUnit = this;
        }

        public void RemoveStop(Units.敌人 target)
        {
            StopUnits.Remove(target);
            target.StopUnit = null;
        }

        public bool CanStop(Units.敌人 target)
        {
            if (!CanStopOther) return false;
            if (StopUnits.Contains(target)) return true;
            if (target.StopUnit != null) return false;
            return StopUnits.Count < StopCount;
        }

        public override float Hatred()
        {
            return base.Hatred() + InputTime;
        }

        public override bool IfStoped()
        {
            return StopUnits.Count > 0;
        }

        public void GainChild(int id)
        {
            var unit= Battle.CreatePlayerUnit(id);
            Children.Add(unit);
            unit.Parent = this;
            unit.UnitModel?.gameObject.SetActive(false);
            BattleUI.UI_Battle.Instance.UpdateUnitsLayout();
        }

        void CheckBlock()
        {
            var blockUnits = Battle.FindAll(Position2, UnitData.Radius, 1);
            foreach (Units.敌人 u in blockUnits)
            {
                if (CanStop(u))
                {
                    u.StopUnit = this;
                    StopUnits.Add(u);
                    var pos = Position2 + (u.Position2 - Position2).normalized * (u.UnitData.Radius + UnitData.Radius);
                    u.Position = new Vector3(pos.x, Position.y, pos.y);
                }
            }
        }
    }
}
