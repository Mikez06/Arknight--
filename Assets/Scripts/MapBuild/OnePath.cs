using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 用来处理敌人罚站时间的
/// 暂时先这么搞吧
/// </summary>
public class OnePath : MonoBehaviour
{

    static StartEndModifier startEndModifier = new StartEndModifier()
    {
        exactStartPoint = StartEndModifier.Exactness.ClosestOnNode,
        exactEndPoint = StartEndModifier.Exactness.ClosestOnNode,
    };
    static RaycastModifier raycastModifier = new RaycastModifier()
    {
        useGraphRaycasting = true,
        useRaycasting = false,
    };

    public List<PathPoint> Path = new List<PathPoint>();

    public List<Vector3> PreviewPoints = new List<Vector3>();

    public void PreviewWayPoint()
    {
        Path.Clear();
        foreach (Transform tr in transform)
        {
            var buildPoint = tr.GetComponent<BuildPathPoint>();
            if (buildPoint == null) buildPoint = tr.gameObject.AddComponent<BuildPathPoint>();
            Path.Add(buildPoint.PathPoint);
        }

        PreviewPoints.Clear();
        AstarPath.FindAstarPath();
        AstarPath.active.Scan();
        for (int i = 0; i < transform.childCount-1; i++)
        {
            var start = transform.GetChild(i).position;
            var end = transform.GetChild(i + 1).position;

            if (Path[i].HideMove) continue;
            var p= ABPath.Construct(start, end);
            AstarPath.StartPath(p);
            p.BlockUntilCalculated();

            startEndModifier.Apply(p);
            if (Path[i].DirectMove) raycastModifier.Apply(p);
            for (int j = 0; j < p.vectorPath.Count; j++)
            {
                PreviewPoints.Add(p.vectorPath[j]);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (PreviewPoints != null)
            for (int i = 0; i < PreviewPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(PreviewPoints[i], PreviewPoints[i + 1]);
            }
    }
}