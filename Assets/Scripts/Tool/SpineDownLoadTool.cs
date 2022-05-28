using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpineDownLoadTool : MonoBehaviour
{
    public TextAsset TextAsset;
    public string Dir;
    public class A
    {
        public Dictionary<string, string[]> spCharGroups;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAll());
    }

    IEnumerator DownloadAll()
    {
        A a = JsonHelper.FromJson<A>(TextAsset.text);
        foreach (var kv in a.spCharGroups)
        {
            float t = Time.time;
            Debug.Log($"��ʼ��ȡ{kv.Key}");
            yield return StartCoroutine(dowloadOne(kv.Key));
            Debug.Log($"{kv.Key}���!��ʱ{Time.time - t}");
        }
        Debug.Log($"ȫ����ȡ��ɣ�");
    }

    IEnumerator dowloadOne(string name)
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        for (int i = 0; i < 3; i++)
        {
            coroutines.Add(StartCoroutine(Download(name, true, i)));
            coroutines.Add(StartCoroutine(Download(name, false, i)));
        }
        foreach (var co in coroutines)
        {
            yield return co;
        }
    }

    IEnumerator Download(string name,bool back,int type)
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
        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get("http://" + $"static.prts.wiki/spine38/char/{name}/{(back ? "back_" : "")}{name}/{name}{end}");
        yield return wr.SendWebRequest();
        if (!string.IsNullOrEmpty(wr.error))
        {
            Debug.Log("Download Error:" + wr.error);
        }
        else
        {
            string path = Dir + name + "/" + (back ? "back" : "front") + "/";
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
