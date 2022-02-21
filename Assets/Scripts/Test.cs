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
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;

public class Test : MonoBehaviour
{
    public Vector3 d;
    public Vector3 c;
    public SkeletonRenderer SkeletonRenderer;
    public string BoneName;
    // Start is called before the first frame update
    async void Start()
    {
        GetComponent<BoneFollower>().boneName = BoneName;
        GetComponent<BoneFollower>().SkeletonRenderer = SkeletonRenderer;
        //GetComponent<BoneFollower>().Initialize();
        //transform.Rotate(new Vector3(0, 1, 0), 90);
        //var animation = GetComponent<SkeletonAnimation>().Skeleton.Data.FindAnimation(s);
        //foreach (var timeline in animation.Timelines)
        //{
        //    if (timeline is Spine.EventTimeline eventTimeline)
        //    {
        //        foreach (var e in eventTimeline.Events)
        //        {
        //            Debug.Log(e.Data.Name + "," + e.Time);
        //        }
        //        //var attackEvent = eventTimeline.Events.FirstOrDefault(x => x.Data.Name == "OnAttack");
        //        //Debug.Log("Onattack:" + attackEvent.Time);
        //        break;
        //    }
        //}
        //await Addressables.InitializeAsync().Task;
        //var a = await Addressables.LoadAssetAsync<UnityEngine.Object>(PathHelper.SpritePath + "spot").Task;
        //Debug.Log(a.GetType());
        //Debug.Log(UnityEngine.Networking.UnityWebRequest.EscapeURL(s).ToUpper());
        //StartCoroutine(Download());

        //UnityEngine.
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
    private void OnGUI()
    {
        //GUI.Button(new Vector3(0,0,150,50),transform.position)
    }
    private void Update()
    {
        //transform.rotation = Quaternion.LookRotation(d,c);
    }
}
