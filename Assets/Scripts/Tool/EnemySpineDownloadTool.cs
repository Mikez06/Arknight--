using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemySpineDownloadTool : MonoBehaviour
{
    public TextAsset TextAsset;
    public string Dir;
    public class A
    {
        public Dictionary<string, object> spCharGroups;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAll());
    }

    IEnumerator DownloadAll()
    {
        var a = JsonHelper.FromJson<Dictionary<string,object>>(TextAsset.text);
        foreach (var kv in a)
        {
            float t = Time.time;
            Debug.Log($"开始爬取{kv.Key}");
            yield return StartCoroutine(dowloadOne(kv.Key));
            Debug.Log($"{kv.Key}完成!耗时{Time.time - t}");
        }
        Debug.Log($"全部爬取完成！");
    }

    IEnumerator dowloadOne(string name)
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        for (int i = 0; i < 3; i++)
        {
            coroutines.Add(StartCoroutine(Download(name, i)));
        }
        foreach (var co in coroutines)
        {
            yield return co;
        }
    }

    IEnumerator Download(string name, int type)
    {
        string end;
        switch (type)
        {
            case 0:
                end = ".png";
                break;
            case 1:
                end = ".skel";
                break;
            default:
                end = ".atlas";
                break;
        }
        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get("http://" + $"static.prts.wiki/spine/enemy/{name}/{name}/{name}{end}");
        yield return wr.SendWebRequest();
        if (!string.IsNullOrEmpty(wr.error))
        {
            Debug.Log("Download Error:" + wr.error);
        }
        else
        {
            string path = Dir + name + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream txt = new FileStream(path + $"{name}{end}", FileMode.Create);
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
