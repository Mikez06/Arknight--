using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public ParticleSystem PS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (PS != null && PS.isStopped)
        {
            EffectManager.Instance.ReturnEffect(this);
        }
    }

    public void Play()
    {
        if (PS != null) PS.Play();
    }
}
