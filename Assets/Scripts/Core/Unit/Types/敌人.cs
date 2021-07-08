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
        public 干员 StopUnit;

        public WaveData WaveConfig => Database.Instance.Get<WaveData>(WaveId);
        public int WaveId;
        /// <summary>
        /// 当前走到第几个目标点
        /// </summary>
        public int NowPathPoint;
        protected Vector3 NextPoint => PathPoints[NowPathPoint + 1].Pos;
        public CountDown PathWaiting;
        public List<PathPoint> PathPoints;
        public bool NeedResetPath;


        public List<Vector3> TempPath;
        public int TempIndex;
        protected Vector3 TempTarget => TempPath[TempIndex + 1];


        public override void Init()
        {
            base.Init();
            PathPoints = PathManager.Instance.GetPath(WaveConfig.Path);
            Position = PathPoints[0].Pos;

            findNewPath();
            ScaleX = TargetScaleX = (PathPoints[NowPathPoint + 1].Pos.x - Position.x) > 0 ? 1 : -1;
            State = StateEnum.Idle;
            AnimationName = "Idle";
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);
        }

        public override void Finish()
        {
            //Debug.Log("Finish");
            Hp = 0;
            BattleUI.UI_Battle.Instance.ReturnUIUnit(this);
            Battle.Enemys.Remove(this);
            GameObject.Destroy(UnitModel.gameObject);
            UnitModel = null;
        }

        public override void UpdateAction()
        {
            if (State != StateEnum.Die)
            {
                CheckBlock();
            }

            if (!Turning.Finished())
            {
                Turning.Update(SystemConfig.DeltaTime);
                ScaleX = -TargetScaleX * ((Turning.value / SystemConfig.TurningTime) - 0.5f) * 2;
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

            if (Turning.Finished() && (State == StateEnum.Move || State == StateEnum.Idle))
            {
                UpdateMove();
            }
        }

        /// <summary>
        /// 判断是否有人在阻挡自己
        /// </summary>
        public virtual void CheckBlock()
        {
            if (Config.Height > 0) return;//飞行单位无法被阻挡
            if (StopUnit != null) return;
            var blockUnit = Battle.FindFirst(Position2, Config.Radius, 0, x => (x as Units.干员).CanStop(this), x => (x.Position2 - Position2).magnitude) as 干员;
            if (blockUnit != null)
            {
                StopUnit = blockUnit;
                blockUnit.StopUnits.Add(this);
            }
        }

        protected override void UpdateMove()
        {
            if (PathWaiting != null && !PathWaiting.Finished())
            {
                AnimationName = "Idle";
                AnimationSpeed = 1;
                PathWaiting.Update(SystemConfig.DeltaTime);
                return;
            }
            if (StopUnit != null) return;//有人阻挡，停止移动,理论上这一句不要
            AnimationName = Speed >= 1 ? "Run_Loop" : "Move_Loop";
            AnimationSpeed = 1;
            if (TempPath == null || NeedResetPath)//无路径或因为外力走出了预定路线，重寻路
            {
                findNewPath();
            }

            var delta = TempTarget - Position;
            if (delta != Vector3.zero) Direction = new Vector2(delta.x, delta.z);
            var scaleX = delta.x > 0 ? 1 : -1;
            if (scaleX != ScaleX)
            {
                TargetScaleX = scaleX;
                Turning.Set(SystemConfig.TurningTime);
                return;
            }
            if ((TempTarget - Position).magnitude < Speed * SystemConfig.DeltaTime)
            {
                //Debug.Log("Arrive");
                Position = TempTarget;
                //抵达临时目标
                TempIndex++;
                if (TempIndex == TempPath.Count - 1)
                {
                    NowPathPoint++;

                    if (NowPathPoint == WaveConfig.Path.Length)
                    {
                        //破门了
                        Battle.DoDamage(Config.Damage);
                        Battle.EnemyCount--;
                        Finish();
                    }
                    else
                    {
                        //往下个点走
                        TempPath = null;
                    }
                }
            }
            else
            {
                Position += (TempTarget - Position).normalized * Speed * SystemConfig.DeltaTime;
            }
        }

        public override void DoDie()
        {
            if (StopUnit != null)
            {
                StopUnit.StopUnits.Remove(this);
            }
            Battle.EnemyCount--;
            base.DoDie();
        }

        void findNewPath()
        {
            TempPath = Battle.Map.FindPath(Position, NextPoint);
            var log = "";
            foreach (var p in TempPath) log += p.ToString() + ",";
            Debug.Log($"Path:{log}");
            TempIndex = 0;
        }

        public float distanceToFinal()
        {
            float result = 0;
            for (int i = NowPathPoint + 1; i < PathPoints.Count-1; i++)
            {
                result += (PathPoints[i].Pos - PathPoints[i + 1].Pos).magnitude;
            }
            for (int i = TempIndex + 1; i < TempPath.Count - 1; i++)
            {
                result += (TempPath[i] - TempPath[i + 1]).magnitude;
            }
            result += (Position - TempTarget).magnitude;
            return result;
        }
    }
}
