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
        然后判断死亡
        接着判断技能，如果技能可以释放，会阻止单位移动。
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

        public override void UpdateAction()
        {
            base.UpdateAction();
        }

        protected override void UpdateMove()
        {
            if (PathWaiting != null && !PathWaiting.Finished())
            {
                PathWaiting.Update(SystemConfig.DeltaTime);
                return;
            }
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
            if ((targetPoint - Position).magnitude < Speed * SystemConfig.DeltaTime)
            {
                Position = targetPoint;
                //抵达临时目标
                NowPathPoint++;
                if (NowPathPoint == WaveConfig.Path.Length - 1)
                {
                    //破门了
                    Battle.DoDamage(Config.Damage);
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
            base.DoDie();
            if (StopUnit != null)
            {
                StopUnit.StopUnits.Remove(this);
            }
        }
    }
}
