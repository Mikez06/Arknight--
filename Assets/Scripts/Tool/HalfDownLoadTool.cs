using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class HalfDownLoadTool : MonoBehaviour
{
    public TextAsset PartText, CharText;
    public string IconDir, HalfDir;
    public string[] Targets;

    public class PrtsInfo
    {
        public string cn;
        public string en;
        public string icon;
        public string half;
    }


    public class CharInfo
    {
        public string name;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAll());
    }

    IEnumerator DownloadAll()
    {

        PrtsInfo[] partInfos = JsonHelper.FromJson<PrtsInfo[]>(PartText.text);
        Dictionary<string, CharInfo> charInfos = JsonHelper.FromJson<Dictionary<string, CharInfo>>(CharText.text);
        string getIdByName(string name)
        {
            var s = charInfos.FirstOrDefault(x => x.Value.name == name).Key;
            if (string.IsNullOrEmpty(s)) return "";
            var ss = s.Split('_');
            return ss.Last();
        }
        foreach (var kv in partInfos)
        {
            if (Targets.Length > 0 && !Targets.Contains(kv.cn)) continue;
            float t = Time.time;
            Debug.Log($"开始爬取{kv.cn}");
            yield return StartCoroutine(Download(kv.icon, "icon_" + getIdByName(kv.cn), IconDir));
            yield return StartCoroutine(Download(kv.half, "half_" + getIdByName(kv.cn), HalfDir));
            Debug.Log($"{kv.cn}完成!耗时{Time.time - t}");
        }
        Debug.Log($"全部爬取完成！");
    }

    IEnumerator Download(string url, string name,string dir)
    {
        int index = url.IndexOf("110px-");
        if (index > 0)
        {
            url = url.Substring(0, index - 1);
            url = url.Replace("thumb/", "");
        }
        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get(url);
        wr.timeout = 2;
        yield return wr.SendWebRequest();
        if (!string.IsNullOrEmpty(wr.error))
        {
            Debug.Log("Download Error:" + wr.error);
        }
        else
        {
            string path = dir;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream txt = new FileStream(path + $"{name.ToLower()}.png", FileMode.Create);
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
