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

        public WaveConfig WaveConfig => Database.Instance.Get<WaveConfig>(WaveId);
        public int WaveId;
        /// <summary>
        /// 当前走到第几个目标点
        /// </summary>
        public int NowPathPoint;
        public CountDown PathWaiting;
        public List<MapGrid> TempPath;

        public override void Init()
        {
            base.Init();
            Position = Battle.Map.Grids[WaveConfig.Path[0].x, WaveConfig.Path[0].y].transform.position + new Vector3(0, Config.Height, 0);

            TempPath = Battle.Map.FindPath(NowGrid, Battle.Map.Grids[WaveConfig.Path[NowPathPoint + 1].x, WaveConfig.Path[NowPathPoint + 1].y]);
            ScaleX = TargetScaleX = (TempPath[1].X - TempPath[0].X) > 0 ? 1 : -1;
            State = StateEnum.Idle;
            AnimationName = "Idle";
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);
            PosOffset = ((float)Battle.Random.NextDouble() - 0.5f);
        }

        public override void Finish()
        {
            Debug.Log("Finish");
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
            if (TempPath == null)
            {
                TempPath = Battle.Map.FindPath(NowGrid, Battle.Map.Grids[WaveConfig.Path[NowPathPoint + 1].x, WaveConfig.Path[NowPathPoint + 1].y]);
            }
            int index = TempPath.IndexOf(NowGrid);
            if (index < 0)//因为外力走出了预定路线，重寻路
            {
                TempPath = Battle.Map.FindPath(NowGrid, Battle.Map.Grids[WaveConfig.Path[NowPathPoint + 1].x, WaveConfig.Path[NowPathPoint + 1].y]);
                index = 0;
            }
            Vector3 targetPoint = index == TempPath.Count - 1 ? TempPath[index].transform.position : TempPath[index + 1].transform.position;
            var delta = targetPoint - Position;
            if (delta != Vector3.zero) Direction = new Vector2(delta.x, delta.z);
            var scaleX = delta.x > 0 ? 1 : -1;
            if (scaleX != ScaleX)
            {
                TargetScaleX = scaleX;
                Turning.Set(SystemConfig.TurningTime);
                return;
            }
            if ((targetPoint - Position).magnitude < Speed * SystemConfig.DeltaTime)
            {
                //Debug.Log("Arrive");
                Position = targetPoint;
                //抵达临时目标
                NowPathPoint++;
                if (NowPathPoint == WaveConfig.Path.Length - 1)
                {
                    //破门了
                    Battle.DoDamage(Config.Damage);
                    Finish();
                }
                else
                {
                    //往下个点走
                    TempPath = null;
                    PathWaiting = new CountDown(WaveConfig.PathWait[NowPathPoint]);
                }
            }
            else
            {
                Position += (targetPoint - Position).normalized * Speed * SystemConfig.DeltaTime;
            }
        }

        public override void DoDie()
        {
            if (StopUnit != null)
            {
                StopUnit.StopUnits.Remove(this);
            }
            base.DoDie();
        }

        public float distanceToFinal()
        {
            var point = WaveConfig.Path.Last();
            var v = Battle.Map.Grids[point.x, point.y].transform.position - Position;
            v.y = 0;
            return v.magnitude;
        }
    }
}
