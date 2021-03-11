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
        GetComponent<ImageEffect_GaussianBlur>().AnimSetBlurSize(0, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
