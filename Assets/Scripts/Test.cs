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

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
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

        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get("http://static.prts.wiki/spine/char/char_485_pallas/char_485_pallas/char_485_pallas.png");
        yield return wr.SendWebRequest();
        if (!string.IsNullOrEmpty( wr.error))
        {
            Debug.Log("Download Error:" + wr.error);
        }
        else
        {
            FileStream txt = new FileStream("E:/1.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(txt);
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

    // Update is called once per frame
    void Update()
    {

    }
}
