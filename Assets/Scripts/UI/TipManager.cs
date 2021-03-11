using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using MainUI;

public class TipManager : MonoBehaviour
{
    public static TipManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("TipManager");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<TipManager>();
            }
            return instance;
        }
    }
    private static TipManager instance;

    List<TipItem> Tips = new List<TipItem>();

    public class TipItem
    {
        public GComponent com;
        public float startTime;
    }

    public void ShowTip(string text)
    {
        var tip = UIPackage.CreateObject("MainUI", "ToopTip").asCom;
        tip.text = text;
        GRoot.inst.AddChild(tip);
        TipItem tipItem = new TipItem()
        {
            com = tip,
            startTime = Time.time,
        };
        Tips.Add(tipItem);
        tip.position = new Vector3(GRoot.inst.width, 300);
        if (Tips.Count > 1 && tip.y - Tips[Tips.Count - 2].com.y < tip.height + 10 )
        {
            tip.y = Tips[Tips.Count - 2].com.y + tip.height + 10;
        }
    }

    private void Update()
    {
        foreach (var item in Tips.ToArray())
        {
            if (item.startTime + 5 < Time.time)
            {
                item.com.Dispose();
                Tips.Remove(item);
            }
            else break;
        }

        TipItem[] array = Tips.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            TipItem item = (TipItem)array[i];
            if (i == 0)
            {
                item.com.y -= 400 * Time.deltaTime;
                if (item.com.y < 100) item.com.y = 100;
            }
            else
            {
                item.com.y -= 400 * Time.deltaTime;
                if (item.com.y - array[i - 1].com.y < item.com.height + 10) item.com.y = array[i - 1].com.y + item.com.height + 10;
            }
        }
    }
}

