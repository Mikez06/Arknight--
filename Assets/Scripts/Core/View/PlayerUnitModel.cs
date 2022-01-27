using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class PlayerUnitModel : SpineModel
{
    public SkeletonAnimation SkeletonAnimation2;
    bool forward = true;

    public override void Init(Unit unit)
    {
        SkeletonAnimation2.AnimationName = "Default";
        SkeletonAnimation2.gameObject.SetActive(false);
        gameObject.SetActive(false);
        base.Init(unit);
    }

    protected override void updateState()
    {
        var angle = Vector2.SignedAngle(Vector2.right, Unit.Direction);
        if (angle < 0) angle += 360;
        bool backward = angle > 45 && angle < 135;//在这个角度下，显示干员背面
        if (Unit.State == StateEnum.Die || Unit.AnimationName == Unit.UnitData.StunAnimation) backward = false;//死亡和眩晕时，只有正面有动画
        if (backward == forward)
        {
            var ani = Unit.AnimationName.Length > 1 ? Unit.AnimationName[1] : Unit.AnimationName[0];
            if (Unit.UnitData.ForwardAnimation != null && Unit.UnitData.ForwardAnimation.Contains(ani) && backward) //有些动画会将单位强制设置为正面
            {

            }
            else
            {
                changeForward();
            }
        }
        base.updateState();
    }

    void changeForward()
    {
        forward = !forward;
        var s = SkeletonAnimation;
        SkeletonAnimation = SkeletonAnimation2;
        SkeletonAnimation2 = s;
        SkeletonAnimation2.gameObject.SetActive(false);
        SkeletonAnimation.gameObject.SetActive(true);
    }

    protected override void changeAnimation(string[] animations)
    {
        var ani = animations.Length > 1 ? animations[1] : animations[0];
        if (Unit.UnitData.ForwardAnimation != null && Unit.UnitData.ForwardAnimation.Contains(ani) && !forward) //有些动画会将单位强制设置为正面
        {
            changeForward();
        }
        base.changeAnimation(animations);
    }

    public override float GetAnimationDuration(string animationName)
    {
        var a = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName);
        if (a==null)
            a=SkeletonAnimation2.Skeleton.Data.FindAnimation(animationName);
        return a.Duration;
    }

    public override Vector3 GetPoint(string name)
    {
        if (name != null && name.StartsWith("F_")) return base.GetPoint(name);
        return base.GetPoint((forward ? "F_" : "B_") + name);
    }

    public override float GetSkillDelay(string[] animationName, string[] lastState, out float fullDuration, out float beginDuration)
    {
        Spine.Unity.SkeletonAnimation SkeletonAnimation = forward ? this.SkeletonAnimation : this.SkeletonAnimation2;
        float result = 0;
        fullDuration = 0;
        beginDuration = 0;
        //如果有状态切换，那么去判断前摇和后摇动画
        if (!animationName.StringsEqual(lastState) && animationName.Length > 1)
        {
            var _beginAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName[0]);
            if (_beginAnimation != null)
            {
                float duration = _beginAnimation.Duration; //- SkeletonAnimation.skeletonDataAsset.defaultMix;
                if (duration < 0) duration = 0;
                //Debug.Log("beginAnimation:" + _beginAnimation.duration);
                beginDuration = duration;
            }
        }
        var nextAnimation = animationName.Length > 1 ? animationName[1] : animationName[0];
        var animation = SkeletonAnimation.Skeleton.Data.FindAnimation(nextAnimation);
        bool ifAttackEvent = false;
        foreach (var timeline in animation.Timelines)
        {
            if (timeline is Spine.EventTimeline eventTimeline)
            {
                var attackEvent = eventTimeline.Events.FirstOrDefault(x => x.Data.Name == "OnAttack");
                //if (attackEvent == null) attackEvent = eventTimeline.Events.FirstOrDefault();
                if (attackEvent != null)
                {
                    result += attackEvent.Time;
                    ifAttackEvent = true;
                }
                //Debug.Log("Onattack:" + attackEvent.Time);
                break;
            }
        }
        //Debug.Log("A:" + animation.duration);
        fullDuration += animation.Duration;
        if (!ifAttackEvent) result = fullDuration;
        return result;
    }
}

