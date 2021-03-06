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
        SkeletonAnimation.AnimationName = "Default";
        updateState();
    }

    public void Update()
    {
        updateState();
    }

    protected virtual void updateState()
    {
        if (Unit.State == StateEnum.Default)
        {
            if (Unit.Direction.x > 0) SkeletonAnimation.transform.localScale = new Vector3(1, 1, 1);
            if (Unit.Direction.x < 0) SkeletonAnimation.transform.localScale = new Vector3(-1, 1, 1);
            return;
        }
        transform.position = Unit.Position;
        //默认转身需要0.2秒
        if (Unit.Direction.x > 0 && transform.localScale.x != 1)
        {
            SkeletonAnimation.transform.localScale = new Vector3(Mathf.Clamp(SkeletonAnimation.transform.localScale.x + 5f * Time.deltaTime, -1, 1), 1, 1);
        }
        else if (Unit.Direction.x < 0 && transform.localScale.x != -1)
        {
            SkeletonAnimation.transform.localScale = new Vector3(Mathf.Clamp(SkeletonAnimation.transform.localScale.x - 5f * Time.deltaTime, -1, 1), 1, 1);
        }

        if (Unit.AnimationName != SkeletonAnimation.AnimationName)
        {
            changeAnimation(Unit.AnimationName);
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
        var _beginAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(animationName + "_Begin");
        if (_beginAnimation != null)
        {
            SkeletonAnimation.state.AddAnimation(0, animationName + "_Begin", false, 0);
        }
        //SkeletonAnimation.state.AddAnimation(0, animationName, true, 0);
        SkeletonAnimation.state.AddAnimation(0, animationName, animationName == "Run_Loop" || animationName == "Move_Loop" || animationName == "Idle" || animationName == "Attack", 0);
    }

    public float GetSkillDelay(string animationName,string lastState,out float fullDuration)
    {
        float result = 0;
        fullDuration = 0;
        //如果有状态切换，那么去判断前摇和后摇动画
        if (animationName != lastState)
        {
            //观察发现，进入idle以外并不会播后摇动画
            //var _endAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(lastState + "_End");
            //if (_endAnimation != null)
            //{
            //    result += _endAnimation.duration;
            //}
            var _beginAnimation = SkeletonAnimation.Skeleton.data.FindAnimation(animationName + "_Begin");
            if (_beginAnimation != null)
            {
                result += _beginAnimation.duration;
                fullDuration += _beginAnimation.duration;
            }
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
        fullDuration += animation.duration;
        return result;
    }

    public float GetAnimationDuration(string animationName)
    {
        var result= SkeletonAnimation.Skeleton.data.FindAnimation(animationName).Duration;
        return result;
    }
}

