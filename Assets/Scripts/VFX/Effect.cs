using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public int Id;
    public EffectData EffectData => Database.Instance.Get<EffectData>(Id);
    public Unit Parent;
    ParticleSystem[] PS;
    float LifeTime = 5f;

    private void Awake()
    {
        PS = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EffectData.ParentFollow == 1)
        {
            transform.localScale = new Vector3(Parent.ScaleX, 1, 1);
        }

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
    public void Init(Unit user,Unit target, Vector3 basePos, Vector3 direction)
    {
        Play();
        if (EffectData.ParentFollow == 2)
            this.Parent = user;
        else
            this.Parent = target;

        var bonePos = Parent.UnitModel.transform.position;
        if (!string.IsNullOrEmpty(EffectData.BindPoint))
        {
            basePos = Parent.UnitModel.GetPoint(EffectData.BindPoint);
        }
        if (EffectData.StartPos == 0)
        {
            transform.position = basePos + EffectData.Offset;
        }
        else
        {
            transform.position = bonePos + EffectData.Offset;
        }

        if (EffectData.ScaleXFollow == 1)
        {
            transform.localScale = new Vector3(target.ScaleX, 1, 1);
        }
        else if (EffectData.ScaleXFollow == 2)
        {
            transform.localScale = new Vector3(user.ScaleX, 1, 1);
        }

        float angleX = EffectData.FaceCamera ? 60 : 0;
        float angleY = 0;
        float angleZ = 0;
        if (EffectData.ForwordDirection==1)
        {
            if (!(Parent.UnitModel as PlayerUnitModel).Forward)
            {
                angleZ = Parent.ScaleX * 90;
            }
        }
        if (EffectData.ForwordDirection == 2)
        {
            angleY = Vector2.SignedAngle(user.Direction, Vector2.right);
        }
        transform.eulerAngles = new Vector3(angleX, angleY, angleZ);
    }
}
