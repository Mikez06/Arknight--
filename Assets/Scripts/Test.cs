using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Test : MonoBehaviour, IPointerClickHandler
{
    public GameObject cube;
    public bool Foucs;
    private bool f;

    Vector3 startPosion;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(123);
    }

    // Start is called before the first frame update
    void Start()
    {
        f = Foucs;
        startPosion = transform.position;
        Vector3 forword = transform.up;
        var v3 = transform.rotation;
        DOTween.To(() => 0, (x) => transform.rotation = Quaternion.AngleAxis(x, forword) * v3, 10f, 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Foucs != f)
        //{
        //    f = Foucs;
        //}
        //if (Foucs)
        //{
        //    Vector3 targetPosition = cube.transform.position + new Vector3(0, 5, -5);
        //    DOTween.To(() => transform.position, (x) => transform.position = x, targetPosition, 0.2f);
        //}
        //else
        //{
        //    DOTween.To(() => transform.position, (x) => transform.position = x, startPosion, 0.2f);
        //}
    }
}
