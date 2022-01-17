using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;


public class TrailManager : Singleton<TrailManager>
{
    public Pool<TrailRenderer> TrailPool = new Pool<TrailRenderer>();

    const float MoveSpeed = 8f;

    public async void ShowPath(List<Vector3> points)
    {
        if (points == null || points.Count == 0) return;
        var go = TrailPool.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.OtherPath + "Trail").GetComponent<TrailRenderer>(), points.FirstOrDefault());
        for (int i = 1; i < points.Count() - 1; i++)
        {
            await go.transform.DOMove(points[i] + new Vector3(0, 0.001f, 0), MoveSpeed).SetEase(Ease.Linear).SetSpeedBased().Wait();
        }
        await TimeHelper.Instance.WaitAsync(go.time);
        TrailPool.Despawn(go);
    }
}
