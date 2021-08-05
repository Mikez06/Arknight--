using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Spine.Unity;

public class UnitModel : MonoBehaviour
{
    public Unit Unit;
    public SkeletonAnimation SkeletonAnimation;
    protected Renderer Renderer;
    protected MaterialPropertyBlock mpb;
    string nowAnimation;
    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        Renderer = SkeletonAnimation.GetComponent<Renderer>();
        Renderer.GetPropertyBlock(mpb);
    }

    public virtual void Init()
    {
        SkeletonAnimation.AnimationName = "Default";
        nowAnimation = "Default";
        SkeletonAnimation.SkeletonDataAsset.GetAnimationStateData().DefaultMix = 0f;
        updateState();
    }

    public void LateUpdate()
    {
        updateState();
    }

    public virtual Vector3 GetPoint(string name)
    {
        if (string.IsNullOrEmpty(name) || name == "F_" || name == "B_") return transform.position;
        var bone = SkeletonAnimation.Skeleton.FindBone(name);
        //var bones = SkeletonAnimation.skeletonDataAsset.GetSkeletonData(false).Bones;
        //var bone = SkeletonAnimation.skeletonDataAsset.GetSkeletonData(false).Bones.Find(x => x.Name == name);
        if (bone == null)
        {
            Debug.LogWarning($"{Unit.UnitData.Name} 无法找到骨骼 {name}");
            return transform.position;
        }
        //Vector3 point = new Vector3(bone.GetWorldPosition * transform.lossyScale.x, bone.Y * transform.lossyScale.y, 0);
        //Debug.Log($"bone {name} 的位置偏移 {point}");
        return bone.GetWorldPosition(SkeletonAnimation.transform);
    }

    protected virtual void updateState()
    {
        SkeletonAnimation.transform.localScale = new Vector3(Unit.ScaleX, 1, 1);
        if (Unit.State == StateEnum.Default)
        {
            return;
        }
        transform.position = Unit.Position;

        if (nowAnimation!=Unit.GetAnimation())
        {
            changeAnimation(Unit.GetAnimation());
        }
        if (Unit.AnimationSpeed != SkeletonAnimation.timeScale && !nullAnimation)
        {
            SkeletonAnimation.timeScale = Unit.AnimationSpeed;
        }
    }

    bool nullAnimation;
    void changeAnimation(string animationName)
    {
        var next = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName);
        if (next == null) //如果模型上不存在目标动画 那么就把当前动作暂停住 一般用于怪物被击晕
        {
            nowAnimation = animationName;
            SkeletonAnimation.timeScale = 0;
            nullAnimation = true;
            return;
        }
        nullAnimation = false;
        SkeletonAnimation.state.ClearTracks();
        //SkeletonAnimation.state.AddEmptyAnimation(0, 0, 0);
        if (animationName == "Idle") //从其他状态返回Idle时，如果有退出动画，就播放
        {
            var _endAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(nowAnimation + "_End");
            if (_endAnimation != null)
            {
                SkeletonAnimation.state.AddAnimation(0, nowAnimation + "_End", false, 0);
            }
        }

        float delay = 0;
        //切入其他状态时，若有进入动画，播放
        if (nowAnimation != animationName)
        {
            var _beginAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName + "_Begin");
            if (_beginAnimation != null)
            {
                Debug.Log(Unit.UnitData.Id + "Add" + animationName + "_Begin" + Time.time);
                SkeletonAnimation.state.AddAnimation(0, animationName + "_Begin", false, 0);
                delay += _beginAnimation.Duration;
            }
        }
        //Debug.Log(Unit.Config._Id + "Add" + animationName);
        //SkeletonAnimation.state.AddAnimation(0, animationName, true, 0);
        SkeletonAnimation.state.AddAnimation(0, animationName, animationName == "Idle" || animationName.EndsWith("Loop"), delay);
        nowAnimation = animationName;
    }

    public void BreakAnimation()
    {
        var next = Unit.GetAnimation();
        Debug.Log($"break from {nowAnimation} to {next}");
        changeAnimation(next);
    }

    public float GetSkillDelay(string animationName,string lastState,out float fullDuration,out float beginDuration)
    {
        float result = 0;
        fullDuration = 0;
        beginDuration = 0;
        //如果有状态切换，那么去判断前摇和后摇动画
        if (animationName != lastState)
        {
            //进入idle以外并不会播后摇动画
            //var _endAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(lastState + "_End");
            //if (_endAnimation != null)
            //{
            //    result += _endAnimation.duration;
            //}
            var _beginAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName + "_Begin");
            if (_beginAnimation != null)
            {
                float duration = _beginAnimation.Duration; //- SkeletonAnimation.skeletonDataAsset.defaultMix;
                if (duration < 0) duration = 0;
                //Debug.Log("beginAnimation:" + _beginAnimation.duration);
                beginDuration = duration;
            }
        }
        var animation = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName);
        foreach (var timeline in animation.Timelines)
        {
            if (timeline is Spine.EventTimeline eventTimeline)
            {
                var attackEvent = eventTimeline.Events.FirstOrDefault(x => x.Data.Name == "OnAttack");
                result += attackEvent.Time;
                //Debug.Log("Onattack:" + attackEvent.Time);
                break;
            }
        }
        //Debug.Log("A:" + animation.duration);
        fullDuration += animation.Duration;
        return result;
    }

    public virtual float GetAnimationDuration(string animationName)
    {
        var result= SkeletonAnimation.Skeleton.Data.FindAnimation(animationName).Duration;
        return result;
    }

    public void SetColor(Color color)
    {
        mpb.SetColor("_Color", color);
        Renderer.SetPropertyBlock(mpb);
    }

    public Color GetColor(Color color)
    {
        Renderer.GetPropertyBlock(mpb);
        return mpb.GetColor("_Color");
    }
}

