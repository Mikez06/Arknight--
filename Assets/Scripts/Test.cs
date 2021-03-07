using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        System.Random r = new System.Random();
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(r.Next(0, 2));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
