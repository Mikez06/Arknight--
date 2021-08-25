using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class StandPicDownLoadTool : MonoBehaviour
{
    public TextAsset PartText, CharText;
    public string Dir;
    public string StartName;
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

        bool needContinue = !string.IsNullOrEmpty(StartName);
        foreach (var kv in partInfos)
        {
            if (needContinue)
            {
                if (kv.cn != StartName)
                    continue;
                else
                    needContinue = false;
            }
            if (Targets.Length > 0 && !Targets.Contains(kv.cn)) continue;
            float t = Time.time;
            Debug.Log($"开始爬取{kv.cn}");
            //yield return StartCoroutine(Download(kv.icon, "icon_" + kv.en, IconDir));
            yield return StartCoroutine(Download(getIdByName(kv.cn), kv.cn, Dir));
            Debug.Log($"{kv.cn}完成!耗时{Time.time - t}");
        }
        Debug.Log($"全部爬取完成！");
    }

    IEnumerator Download(string fileName, string name,string dir)
    {
        string url = $"http://prts.wiki/w/" + UnityEngine.Networking.UnityWebRequest.EscapeURL($"文件:立绘_{name}_1").ToUpper() + ".png";
        Debug.Log(url);

        UnityEngine.Networking.UnityWebRequest wr = UnityEngine.Networking.UnityWebRequest.Get(url);
        yield return wr.SendWebRequest();
        if (!string.IsNullOrEmpty(wr.error))
        {
            Debug.Log("Download Error:" + wr.error);
        }
        else
        {
            var htmlText = (wr.downloadHandler.text);
            Debug.Log(htmlText);
            int startIndex = htmlText.IndexOf(UnityEngine.Networking.UnityWebRequest.EscapeURL($"立绘_{name}_1").ToUpper());
            Debug.Log(startIndex);
            //int index1 = htmlText.LastIndexOf('/', startIndex - 6);
            //int index2 = htmlText.LastIndexOf('/', index1 - 6);
            var next = htmlText.Substring(startIndex - 6, 6);
            Debug.Log(next);
            var nextUrl = $"http://prts.wiki/images" + next + UnityEngine.Networking.UnityWebRequest.EscapeURL($"立绘_{name}_1").ToUpper() + ".png";
            wr = UnityEngine.Networking.UnityWebRequest.Get(nextUrl);
            Debug.Log($"http://prts.wiki/images" + next + $"立绘_{name}_1" + ".png");
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
                FileStream txt = new FileStream(path + $"{fileName.ToLower()}.png", FileMode.Create);
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
