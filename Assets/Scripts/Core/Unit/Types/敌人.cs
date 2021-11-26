using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public const float StopExCheck = 0.2f, TempArriveDistance = 0.1f;
        public 干员 StopUnit;

        public WaveData WaveData => Database.Instance.Get<WaveData>(WaveId);
        public int WaveId;
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
        public override void Init()
        {
            base.Init();
            Team = 1;
            PathPoints = PathManager.Instance.GetPath(WaveData.Path);
            Position = GetPoint(0);

            findNewPath();
            ScaleX = TargetScaleX = (GetPoint(NowPathPoint + 1).x - Position.x) > 0 ? 1 : -1;
            SetStatus(StateEnum.Idle);
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);
        }

        public override void Finish()
        {
            base.Finish();
            Debug.Log($"{UnitData.Id}Finish");
            Hp = 0;
            if (!UnitData.WithoutCheckCount) Battle.EnemyCount--;
            BattleUI.UI_Battle.Instance.ReturnUIUnit(this);
            Battle.AllUnits.Remove(this);
            Battle.Enemys.Remove(this);
            GameObject.Destroy(UnitModel.gameObject);
            UnitModel = null;
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!Visiable) IfSelectable = false;
        }

        public override void UpdateAction()
        {
            if (!Visiable) updateHide();
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
            var blockUnits = Battle.FindAll(Position2, UnitData.Radius + StopExCheck, 1).Select(x => x as Units.干员).Where(x => (x as Units.干员).CanStop(this)).ToList();
            blockUnits.OrderBy(x => (x.Position2 - Position2).magnitude);
            if (blockUnits.Count > 0)
            {
                blockUnits[0].AddStop(this);
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
                    if (NowPathPoint != PathPoints.Count-1)//抵达临时目标点，如果该目标点不是终点,重新找去下一个点的路
                    {
                        PathWaiting.Set(PathPoints[NowPathPoint].Delay);
                        if (PathPoints[NowPathPoint].HideMove)
                        {
                            Visiable = false;
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

        void updateHide()
        {
            if (PathWaiting.Update(SystemConfig.DeltaTime))
            {
                NowPathPoint++;
                Position = PathPoints[NowPathPoint].Pos;
                Visiable = true;
                UnitModel?.gameObject.SetActive(true);
                Refresh();
            }
        }

        protected override void UpdateMove()
        {
            CheckBlock();
            if (!PathWaiting.Finished())
            {
                if (AnimationName == UnitData.MoveAnimation) SetStatus(StateEnum.Idle);
                PathWaiting.Update(SystemConfig.DeltaTime);
                return;
            }
            CheckArrive();
            if (Unbalance || !Visiable) return;//失衡状态下不许主动移动
            if (StopUnit != null)
            {
                if (AnimationName == UnitData.MoveAnimation)
                {
                    SetStatus(StateEnum.Idle);
                }
                return;//有人阻挡，停止移动
            }
            AnimationName = UnitData.MoveAnimation;
            AnimationSpeed = 1;
            if (TempPath == null || NeedResetPath)//无路径或因为外力走出了预定路线，重寻路
            {
                findNewPath();
            }

            var delta = TempTarget - Position;
            if (delta != Vector3.zero) Direction = new Vector2(delta.x, delta.z);
            float scaleX;
            if (delta.x > 0) scaleX = 1;
            else if (delta.x <0) scaleX = -1;
            else scaleX= TargetScaleX;
            if (scaleX != ScaleX)
            {
                TargetScaleX = scaleX;
                return;
            }
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
                        Finish();
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
            TempPath = Battle.Map.FindPath(Position - offset, NextPoint - offset, PathPoints[NowPathPoint].DirectMove);
            for (int i = 0; i < TempPath.Count; i++)
            {
                TempPath[i] += offset;
            }
            //var log = "";
            //foreach (var p in TempPath) log += p.ToString() + ",";
            //Debug.Log($"Path:{log}");
            TempIndex = 0;
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
            for (int i = TempIndex + 1; i < TempPath.Count - 1; i++)
            {
                result += (TempPath[i] - TempPath[i + 1]).magnitude;
            }
            result += (Position - TempTarget).magnitude;
            return result;
        }

        public override float Hatred()
        {
            return base.Hatred() - distanceToFinal();
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
