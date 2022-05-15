using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
    /**
     普通敌人逻辑:
        出生确定行动路线，按照设置好的路径点挨个走最近路线。
        每帧先跑一遍buff，刷新buff数值
        判断死亡,
        判断阻挡,
        判断技能，
        判断移动。
        [对于多数敌人来说，技能可以释放的条件会加上“被阻挡”]
        最后判断移动，如果当前格子有阻挡者，也会阻止移动。
         */
    public class 敌人 : Unit
    {
        public const float StopExCheck = 0.29f, TempArriveDistance = 0.1f;
        public Unit StopUnit;
        public 敌人 Parent;

        public WaveInfo WaveData;//=> Database.Instance.Get<WaveData>(WaveId);
        //public int WaveId;
        /// <summary>
        /// 当前走到第几个目标点
        /// </summary>
        public int NowPathPoint;
        protected Vector3 NextPoint => GetPoint(NowPathPoint + 1);
        public CountDown PathWaiting = new CountDown();
        public List<PathPoint> PathPoints;
        public bool NeedResetPath;


        public List<Vector3> TempPath;
        public int TempIndex;
        protected Vector3 TempTarget => TempIndex >= TempPath.Count - 1 ? TempPath[TempPath.Count - 1] : TempPath[TempIndex + 1];

        public bool Visiable = true;
        public bool UnStopped;
        public int StopCost;
        public float WaitTimeEx;

        public override void Init()
        {
            base.Init();
            StopCost = 1;
            if (UnitData.StopCount != 0) StopCost = UnitData.StopCount;
            PathPoints = Battle.MapData.PathInfos.Find(x => x.Name == WaveData.Path).Path; //PathManager.Instance.GetPath(WaveData.Path);
            Position = GetPoint(0);
            PathWaiting.Set(PathPoints[0].Delay);

            //findNewPath();
            ScaleX = TargetScaleX = (GetPoint(NowPathPoint + 1).x - Position.x) > 0 ? 1 : -1;
            SetStatus(StateEnum.Idle);
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);

            hideBase = true;
            Start.Set(UnitModel.GetAnimationDuration("Start"));
            if (Start.Finished()) StartEnd();
            else
                SetStatus(StateEnum.Start);
        }

        public void StartEnd()
        {
            hideBase = false;
            SetStatus(StateEnum.Idle);
            Battle.TriggerDatas.Push(new TriggerData()
            {
                Target = this,
            });
            Trigger(TriggerEnum.落地);
            Battle.TriggerDatas.Pop();
        }

        public override void Finish(bool leaveEvent = true)
        {
            Hp = 0;
            base.Finish(leaveEvent);
            //Debug.Log($"{UnitData.Id}Finish");
            Hp = 0;
            if (!UnitData.WithoutCheckCount && Parent == null)
            {
                Battle.EnemyCount--;
                Battle.CheckPoints.Add(Battle.Tick);
            }
            BattleUI.UI_Battle.Instance.ReturnUIUnit(this);
            Battle.AllUnits.Remove(this);
            Battle.Enemys.Remove(this);
            Battle.PlayerUnits2.Remove(this);
            GameObject.Destroy(UnitModel.gameObject);
            UnitModel = null;
        }

        public override void Refresh()
        {
            UnStopped = false;
            WaitTimeEx = 0;
            base.Refresh();
            if (!Visiable) IfSelectable = false;
        }

        public override void UpdateAction()
        {
            if (!Start.Finished() && State != StateEnum.Die)
            {
                if (Start.Update(SystemConfig.DeltaTime))
                {
                    StartEnd();
                }
                return;
            }
            if (State == StateEnum.Default) return;
            if (!Visiable) if (PathWaiting.Update(SystemConfig.DeltaTime + WaitTimeEx)) finishHide();
            if (!Visiable) return;
            base.UpdateAction();
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

            CheckBlock();
            if (
                //ScaleX==TargetScaleX &&
                (State == StateEnum.Move || State == StateEnum.Idle))
            {
                UpdateMove();
            }
        }

        /// <summary>
        /// 判断是否有人在阻挡自己
        /// </summary>
        public virtual void CheckBlock()
        {
            if (UnitData.Height > 0) return;//飞行单位无法被阻挡
            if (StopUnit != null) return;
            //虽然不知道为啥，但是判断阻挡时和目标不是相切的,加上一个默认的缓冲值
            var blockUnits = Battle.FindAll(Position2, UnitData.Radius + StopExCheck, 1).Where(x => x.CanStop(this)).ToList();
            blockUnits.OrderBy(x => (x.Position2 - Position2).magnitude);
            if (blockUnits.Count > 0)
            {
                blockUnits[0].AddStop(this);

                Battle.TriggerDatas.Push(new TriggerData()
                {
                    User = blockUnits[0],
                    Target = this,
                });
                Trigger(TriggerEnum.阻挡);
                Battle.TriggerDatas.Pop();
            }
        }

        public virtual void CheckArrive()
        {
            if (TempPath == null) findNewPath();

            if ((TempTarget - Position).magnitude < TempArriveDistance)
            {
                TempIndex++;
                if (TempIndex == TempPath.Count - 1)
                {
                    NowPathPoint++;
                    if (NowPathPoint != PathPoints.Count - 1)//抵达临时目标点，如果该目标点不是终点,重新找去下一个点的路
                    {
                        PathWaiting.Set(PathPoints[NowPathPoint].Delay);
                        if (PathPoints[NowPathPoint].HideMove)
                        {
                            if (PathWaiting.value > 0)
                                Visiable = false;
                            else
                                finishHide();
                        }
                        TempPath = null;
                    }
                    else
                    {
                        NowPathPoint--;
                    }
                }
            }
        }

        void finishHide()
        {
            NowPathPoint++;
            Position = PathPoints[NowPathPoint].Pos;
            Visiable = true;
            UnitModel?.gameObject.SetActive(true);
            //Refresh();
            findNewPath();
        }

        public void Jump(float distance)
        {
            if (StopUnit != null) StopUnit.RemoveStop(this);
            if (TempPath == null) findNewPath();
            List<Vector3> points = new List<Vector3>();
            points.Add(Position);
            for (int i = TempIndex + 1; i < TempPath.Count; i++)
            {
                points.Add(TempPath[i]);
            }
            int pathIndex = NowPathPoint;
            int index = 1;
            while (distance > 0)
            {
                if (index >= points.Count)
                {
                    if (pathIndex >= PathPoints.Count - 1)//跳跃后已经可以进门了
                    {
                        Position = PathPoints[PathPoints.Count - 1].Pos;
                        break;
                    }
                    else
                    {
                        //否则把下个寻路节点找到
                        var offset = new Vector3(WaveData.OffsetX, 0, WaveData.OffetsetY);
                        List<Vector3> tempPath;
                        if (Height <= 0)
                            tempPath = Battle.Map.FindPath(Position - offset, GetPoint(pathIndex + 1) - offset, PathPoints[NowPathPoint].DirectMove);
                        else
                            tempPath = new List<Vector3>() { Position - offset, GetPoint(pathIndex + 1) - offset };
                        for (int i = 1; i < tempPath.Count; i++) //注意不要把起点加进去了
                        {
                            Vector3 p = tempPath[i];
                            points.Add(p + offset);
                        }
                        pathIndex++;
                    }
                }
                float pathDist = (points[index] - points[index - 1]).magnitude;
                if (pathDist > distance)
                {
                    Position = points[index - 1] + (points[index] - points[index - 1]).normalized * distance;
                    distance = 0;
                }
                else
                {
                    distance -= pathDist;
                    index++;
                }
            }

            if (NowPathPoint != pathIndex)
            {
                NowPathPoint = pathIndex;
                PathWaiting.Finish();
                NeedResetPath = true;
            }
            else
            {
                TempIndex = TempPath.IndexOf(points[index - 1]);
            }
        }

        protected override void UpdateMove()
        {
            if (!PathWaiting.Finished())
            {
                if (AnimationName == GetMoveAnimation()) SetStatus(StateEnum.Idle);
                PathWaiting.Update(SystemConfig.DeltaTime + WaitTimeEx);
                return;
            }
            CheckArrive();
            if (TempPath == null || NeedResetPath)//无路径或因为外力走出了预定路线，重寻路
            {
                findNewPath();
            }
            if (Unbalance || !Visiable) return;//失衡状态下不许主动移动
            if (StopUnit != null)
            {
                if (AnimationName == GetMoveAnimation())
                {
                    SetStatus(StateEnum.Idle);
                }
                return;//有人阻挡，停止移动
            }
            AnimationName = GetMoveAnimation();
            AnimationSpeed = 1;

            var delta = TempTarget - Position;
            if (delta != Vector3.zero) Direction = new Vector2(delta.x, delta.z);
            float scaleX = TargetScaleX;
            if (delta.x > 0) scaleX = 1;
            else if (delta.x < 0) scaleX = -1;
            else
            {
                bool success = false;
                for (int i = NowPathPoint + 1; i < PathPoints.Count; i++)
                {
                    var x = GetPoint(i).x;
                    if (x != Position.x)
                    {
                        scaleX = Math.Sign(x - Position.x);
                        success = true;
                    }
                }
                if (!success)
                    scaleX = TargetScaleX;
            }
            TargetScaleX = scaleX;
            if ((TempTarget - Position).magnitude < Speed * SystemConfig.DeltaTime)
            {
                //Debug.Log("Arrive");
                Position = TempTarget;
                //抵达临时目标
                TempIndex++;
                if (TempIndex >= TempPath.Count - 1)
                {
                    NowPathPoint++;

                    if (NowPathPoint == PathPoints.Count - 1)
                    {
                        //破门了
                        Battle.DoDamage(UnitData.Damage);
                        Finish(false);
                    }
                    else
                    {
                        PathWaiting.Set(PathPoints[NowPathPoint].Delay);
                        //往下个点走
                        TempPath = null;
                    }
                }
            }
            else
            {
                var target = Position + (TempTarget - Position).normalized * Speed * SystemConfig.DeltaTime;
                Position = target;
                //Debug.Log(Position.x);
            }
        }

        public override void DoDie(object source)
        {
            if (StopUnit != null)
            {
                StopUnit.StopUnits.Remove(this);
            }
            UnitModel?.SetColor(Color.black);
            //Battle.EnemyCount--;
            base.DoDie(source);
        }

        void findNewPath()
        {
            var offset = new Vector3(WaveData.OffsetX, 0, WaveData.OffetsetY);
            if (Height <= 0)
                TempPath = Battle.Map.FindPath(Position - offset, NextPoint - offset, PathPoints[NowPathPoint].DirectMove);
            else
                TempPath = new List<Vector3>() { Position - offset, GetPoint(NowPathPoint + 1) - offset };
            if (TempPath.Count == 0) 
                TempPath.Add(Position - offset);
            for (int i = 0; i < TempPath.Count; i++)
            {
                TempPath[i] += offset;
            }
            //var log = "";
            //foreach (var p in TempPath) log += p.ToString() + ",";
            //Debug.Log($"Path:{log}");
            TempIndex = 0;
            NeedResetPath = false;
        }

        public void DisplayPath()
        {
            List<Vector3> p = new List<Vector3>();
            for (int i = NowPathPoint; i < PathPoints.Count - 1; i++)
            {
                var p1 = Battle.Map.FindPath(PathPoints[i].Pos, PathPoints[i + 1].Pos, PathPoints[i].DirectMove);
                p.AddRange(p1);
            }
            TrailManager.Instance.ShowPath(p);
        }

        Vector3 GetPoint(int index)
        {
            return PathPoints[index].Pos + new Vector3(WaveData.OffsetX, 0, WaveData.OffetsetY);
        }

        public float distanceToFinal()
        {
            float result = 0;
            for (int i = NowPathPoint + 1; i < PathPoints.Count-1; i++)
            {
                result += (GetPoint(i) - GetPoint(i + 1)).magnitude;
            }
            if (TempPath != null)
            {
                for (int i = TempIndex + 1; i < TempPath.Count - 1; i++)
                {
                    result += Mathf.Abs(TempPath[i].x - TempPath[i + 1].x) + Mathf.Abs(TempPath[i].y - TempPath[i + 1].y);
                }
                result += (Position - TempTarget).magnitude;
            }
            return result;
        }

        public override float Hatred()
        {
            return base.Hatred();
        }

        public override bool IfStoped()
        {
            return StopUnit != null;
        }

        protected override void RecoverBalance()
        {
            base.RecoverBalance();
            //因推拉等外力导致偏移路线时，需要结束等待并,重新寻路
            NeedResetPath = true;
        }
    }
}
