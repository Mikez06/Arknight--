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

    

    IEnumerator Download(string name)
    {

        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get("http://" + $"static.prts.wiki/spine/char/{name}/{name}/{name}.png");
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

    Vector2 tile;
    // Update is called once per frame
    void Update()
    {

        tile = Input.mousePosition;
        tile = new Vector2(tile.x / Screen.width, tile.y / Screen.height);
        float x = tile.x;
        float y = tile.y;
        bool b1 = y - x > 0;
        bool b2 = x + y < 1;
        if (b1 && b2)
        {
            tile.x = Mathf.FloorToInt(x);
        }
        if (b1 && !b2)
        {
            tile.y = Mathf.CeilToInt(y);
        }
        if (!b1 && b2)
        {
            tile.y = Mathf.FloorToInt(y);
        }
        if (!b1 && !b2)
        {
            tile.x = Mathf.CeilToInt(x);
        }
        Debug.Log(tile);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0,0,150,150), tile.ToString());
        GUI.Label(new Rect(0, 200, 150, 150), Input.mousePosition.ToString());
    }
}
