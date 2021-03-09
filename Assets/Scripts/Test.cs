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
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GetComponent<SkeletonAnimation>().state.AddAnimation(0, "Idle", false, 0);
            GetComponent<SkeletonAnimation>().state.AddAnimation(0, "Attack", false, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
