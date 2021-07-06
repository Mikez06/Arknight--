using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await Addressables.InitializeAsync().Task;
        var pic = ResHelper.GetAsset<Sprite>("Assets/Bundles/Image/StandPic/12fce");
        GetComponent<SpriteRenderer>().sprite = pic;
        await TimeHelper.Instance.WaitAsync(1f);
        GetComponent<SpriteRenderer>().sprite = null;
        Resources.UnloadAsset(pic.texture);
        Debug.Log(pic.name);
        //ResHelper.Return(pic);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
