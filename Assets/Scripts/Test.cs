using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 36; i++)
        {
            Debug.Log(Vector2.SignedAngle(Vector2.right, new Vector2(Mathf.Cos(i * 10 * Mathf.Deg2Rad), Mathf.Sin(i * 10 * Mathf.Deg2Rad))));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
