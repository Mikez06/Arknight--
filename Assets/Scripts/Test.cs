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
    public GameObject target;
    // Start is called before the first frame update
    async void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
        transform.position += (target.transform.position - transform.position).normalized * 0.1f * Time.deltaTime;
    }
}
