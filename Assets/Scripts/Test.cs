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
    void Start()
    {
        //Debug.Log(UnityEngine.Networking.UnityWebRequest.EscapeURL(s).ToUpper());
        StartCoroutine(Download());

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
