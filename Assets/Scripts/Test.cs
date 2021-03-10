using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        List<string> a = new List<string>()
        {
           "za", "az","aa","nt","t",
        };
        foreach (var s in a.OrderBy(x=>x))
        {
            Debug.Log(s);
        }
        Debug.Log(NPinyin.Pinyin.GetPinyin("阿米娅"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
