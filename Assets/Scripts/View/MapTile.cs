using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IfHeal(bool bo)
    {
        transform.GetChild(0).gameObject.SetActive(!bo);
        transform.GetChild(1).gameObject.SetActive(bo);
    }
}
