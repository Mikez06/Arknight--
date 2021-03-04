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
    public virtual void Init()
    {
        var go = ResHelper.GetUnit(Unit.Config.Model);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        SkeletonAnimation = go.GetComponent<SkeletonAnimation>();
        UpdateState();
    }

    public void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        transform.position = Unit.Position;
        if (Unit.AnimationName != SkeletonAnimation.AnimationName)
        {
            SkeletonAnimation.AnimationState.SetAnimation(0, Unit.AnimationName, true);
        }
        if (Unit.AnimationSpeed != SkeletonAnimation.timeScale)
        {
            SkeletonAnimation.timeScale = Unit.AnimationSpeed;
        }
    }

    void changeAnimation(string animationName)
    {
        SkeletonAnimation.state.ClearTrack(0);
        if (animationName == "Idle") //从其他状态返回Idle时，如果有退出动画，就播放
        {
            var _endAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(SkeletonAnimation.AnimationName + "_End");
            if (_endAnimation != null)
            {
                SkeletonAnimation.state.SetAnimation(0, SkeletonAnimation.AnimationName + "_End", false);
            }
        }

        //切入其他状态时，若有进入动画，播放
        var _beginAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(SkeletonAnimation.AnimationName + "_Begin");
        if (_beginAnimation != null)
        {
            SkeletonAnimation.state.AddAnimation(0, SkeletonAnimation.AnimationName + "_Begin", false, 0);
        }
        SkeletonAnimation.state.AddAnimation(0, SkeletonAnimation.AnimationName, true, 0);
    }

    public float GetSkillDelay(string animationName)
    {
        float result = 0;
        var _beginAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(animationName + "_Begin");
        if (_beginAnimation != null)
        {
            result += _beginAnimation.duration;
        }
        var animation = SkeletonAnimation.Skeleton.data.FindAnimation(animationName);
        foreach (var timeline in animation.timelines)
        {
            if (timeline is Spine.EventTimeline eventTimeline)
            {
                var attackEvent = eventTimeline.Events.FirstOrDefault(x => x.data.name == "OnAttack");
                result += attackEvent.Time;
                break;
            }
        }
        return result;
    }
}

