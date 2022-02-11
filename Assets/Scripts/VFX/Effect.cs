using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    ParticleSystem[] PS;
    float LifeTime = 5f;

    private void Awake()
    {
        PS = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            EffectManager.Instance.ReturnEffect(this);
        }
    }

    public void SetLifeTime(float time)
    {
        LifeTime = time;
    }

    public void Play()
    {
        foreach (var p in PS)
        {
            p.Play();
        }
    }
    public void Init(Unit parent, Vector3 basePos, Vector3 direction)
    {
        Play();
    }
}
