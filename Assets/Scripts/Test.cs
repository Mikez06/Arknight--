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
    public string s;
    // Start is called before the first frame update
    async void Start()
    {
        var animation = GetComponent<SkeletonAnimation>().Skeleton.Data.FindAnimation("Skill_1");
        foreach (var timeline in animation.Timelines)
        {
            if (timeline is Spine.EventTimeline eventTimeline)
            {
                foreach (var e in eventTimeline.Events)
                {
                    Debug.Log(e.Data.Name + "," + e.Time);
                }
                //var attackEvent = eventTimeline.Events.FirstOrDefault(x => x.Data.Name == "OnAttack");
                //Debug.Log("Onattack:" + attackEvent.Time);
                break;
            }
        }
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
        GUI.Label(new Rect(0, 0, 150, 100), Time.time.ToString());
    }
    IEnumerator Download()
    {
        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get(s);
        yield return wr.SendWebRequest();
        if (!string.IsNullOrEmpty(wr.error))
        {
            Debug.Log("Download Error:" + wr.error);
        }
        else
        {
            var s = (wr.downloadHandler.text);
            Debug.Log(s);
            string a = "<tr><td></td><td style='white-space: nowrap;'>";
            int startIndex = s.IndexOf(a);
            Debug.Log(startIndex);
            int index1 = s.IndexOf('\"', startIndex + a.Length);
            int index2 = s.IndexOf('\"', index1 + 1);
            Debug.Log(index1 + "," + index2);
            var next = s.Substring(index1 + 1, index2 - index1 - 1);
            var nextUrl = $"http://prts.wiki" + next;
            Debug.Log(nextUrl);
            wr = UnityEngine.Networking.UnityWebRequest.Get(nextUrl);
            yield return wr.SendWebRequest();
            if (!string.IsNullOrEmpty(wr.error))
            {
                Debug.Log("Download Error:" + wr.error);
            }
            else
            {
                string path = "E://1.png";
                FileStream txt = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(txt);
                //Debug.Log(txt.Name);
                try
                {
                    sw.BaseStream.Write(wr.downloadHandler.data, 0, wr.downloadHandler.data.Length);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                sw.Close();
                txt.Close();
            }
        }
    }
}
