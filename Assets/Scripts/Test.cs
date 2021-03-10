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
        for (int i = 0; i < 10; i++)
        {
            var ps= EffectManager.Instance.GetEffect("击中1");
            ps.PS.Play();
            await TimeHelper.Instance.WaitAsync(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
