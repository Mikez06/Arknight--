using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineBoneFindTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var b in GetComponent<SkeletonAnimation>().Skeleton.Bones)
        {
            var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.transform.localScale = Vector3.one * 0.1f;
            g.name = b.Data.Name;
            g.transform.position = b.GetWorldPosition(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
