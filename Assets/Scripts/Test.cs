using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using Pathfinding;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        List<Vector3Int> l = new List<Vector3Int>()
        {
            new Vector3Int(3,2,1),
            new Vector3Int(1,2,3),
            new Vector3Int(3,1,2),
            new Vector3Int(2,2,2),
        };
        l = l.OrderBy(x => x.x).ThenBy(x => x.y).ToList();
        foreach (var a in l)
        {
            Debug.Log(a);
        }
        //Debug.Log((item["t"] as Newtonsoft.Json.Linq.JArray).GetEnumerator();
        //var p = ABPath.Construct(t0.transform.position, t1.transform.position, null);
        //StartEndModifier s = new StartEndModifier()
        //{
        //    exactStartPoint = StartEndModifier.Exactness.ClosestOnNode,
        //    exactEndPoint = StartEndModifier.Exactness.ClosestOnNode,
        //};
        //AstarPath.StartPath(p);
        //p.BlockUntilCalculated();
        //s.Apply(p);
        //foreach (var point in p.vectorPath)
        //{
        //    Debug.Log(point);
        //}       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
