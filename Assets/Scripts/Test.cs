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
    public Transform t0, t1;
    // Start is called before the first frame update
    async void Start()
    {
        var item = JsonHelper.FromJson<Dictionary<string,object>>("{\"t\":[\"abc\",\"gft\"]}");
        var s= (item["t"] as Newtonsoft.Json.Linq.JArray).ToObject<string[]>();
        foreach (var a in s)
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
