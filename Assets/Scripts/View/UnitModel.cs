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
    string nowAnimation;
    public virtual void Init()
    {
        SkeletonAnimation.AnimationName = "Default";
        nowAnimation = "Default";
        SkeletonAnimation.SkeletonDataAsset.GetAnimationStateData().DefaultMix = 0.1f;
        updateState();
    }

    public void LateUpdate()
    {
        updateState();
    }

    protected virtual void updateState()
    {
        SkeletonAnimation.transform.localScale = new Vector3(Unit.ScaleX, 1, 1);
        if (Unit.State == StateEnum.Default)
        {
            return;
        }
        transform.position = Unit.Position;

        if (nowAnimation!=Unit.AnimationName)
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
        var _beginAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName + "_Begin");
        if (_beginAnimation != null)
        {
            Debug.Log(Unit.Config.Id + "Add" + animationName + "_Begin" + Time.time);
            SkeletonAnimation.state.AddAnimation(0, animationName + "_Begin", false, 0);
            delay += _beginAnimation.Duration;
        }
        //Debug.Log(Unit.Config._Id + "Add" + animationName);
        //SkeletonAnimation.state.AddAnimation(0, animationName, true, 0);
        SkeletonAnimation.state.AddAnimation(0, animationName, animationName == "Run_Loop" || animationName == "Move_Loop" || animationName == "Idle", delay);
        nowAnimation = animationName;
    }

    public float GetSkillDelay(string animationName,string lastState,out float fullDuration)
    {
        float result = 0;
        fullDuration = 0;
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
                float duration = _beginAnimation.Duration - SkeletonAnimation.skeletonDataAsset.defaultMix;
                if (duration < 0) duration = 0;
                //Debug.Log("beginAnimation:" + _beginAnimation.duration);
                result += duration;
                fullDuration += duration;
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
}

