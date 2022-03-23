using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Effect : MonoBehaviour
{
    public int Id;
    public EffectData EffectData => Database.Instance.Get<EffectData>(Id);
    public Unit Parent;
    ParticleSystem[] PS;
    float LifeTime = 5f;

    PlayerUnitModel PlayerUnitModel;
    BoneFollower BoneFollower;
    bool forward;

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
            if (BoneFollower != null) Destroy(BoneFollower);
            EffectManager.Instance.ReturnEffect(this);
        }
        if (BoneFollower != null)
        {
            if (forward != PlayerUnitModel.Forward) updateBoneFollow();
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
    void updateBoneFollow()
    {
        this.forward = PlayerUnitModel.Forward;
        if (!forward && EffectData.ForwardOnly)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        BoneFollower.boneName = (forward ? "F_" : "B_") + EffectData.BindPoint;
        var sr = (PlayerUnitModel.SkeletonAnimation).GetComponent<SkeletonRenderer>();
        BoneFollower.SkeletonRenderer = sr;
        transform.SetParent(null);
        transform.localScale = new Vector3(1, 1, 1);
        transform.SetParent(sr.transform);
    }

    public virtual void Init(Unit user, Unit target, Vector3 basePos, Vector3 direction, float speed = 1)
    {
        foreach (var p in PS)
        {
            var m = p.main;
            m.simulationSpeed = speed;
        }
        Play();
        if (EffectData.ParentFollow == 2)
            this.Parent = user;
        else
            this.Parent = target;
        Vector3 bonePos = Vector3.zero;
        if (Parent != null)
        {
            bonePos = Parent.UnitModel.transform.position;
            if (!string.IsNullOrEmpty(EffectData.BindPoint))
            {
                basePos = Parent.UnitModel.GetPoint(EffectData.BindPoint);
            }
        }

        if (EffectData.ParentFollow != 0)
        {
            transform.parent = Parent.UnitModel.transform;
        }
        if (EffectData.BoneFollow)
        {
            PlayerUnitModel = Parent.UnitModel as PlayerUnitModel;
            BoneFollower = gameObject.AddComponent<BoneFollower>();
            updateBoneFollow();
            //BoneFollower.boneName = EffectData.BindPoint;
            //var sr= Parent.UnitModel.GetComponentInChildren<SkeletonRenderer>();
            //BoneFollower.SkeletonRenderer = sr;
            //transform.SetParent(sr.transform);
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

        float angleX = EffectData.FaceCamera;
        float angleY = 0;
        float angleZ = 0;
        if (EffectData.ForwordDirection == 1)
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
