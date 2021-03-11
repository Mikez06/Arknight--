using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class PlayerUnitModel : UnitModel
{
    public SkeletonAnimation SkeletonAnimation2;
    bool forward = true;

    public override void Init()
    {
        SkeletonAnimation2.AnimationName = "Default";
        SkeletonAnimation2.gameObject.SetActive(false);
        gameObject.SetActive(false);
        base.Init();
    }

    protected override void updateState()
    {
        var angle = Vector2.SignedAngle(Vector2.right, Unit.Direction);
        if (angle < 0) angle += 360;
        bool backward = angle > 45 && angle < 135;//在这个角度下，显示干员背面
        if (Unit.State == StateEnum.Die) backward = false;//死亡时，只有正面有死亡动画
        if (backward == forward) //需要变换正背面
        {
            forward = !forward;
            var s = SkeletonAnimation;
            SkeletonAnimation = SkeletonAnimation2;
            SkeletonAnimation2 = s;
            SkeletonAnimation2.gameObject.SetActive(false);
            SkeletonAnimation.gameObject.SetActive(true);
        }
        base.updateState();
    }

    public override float GetAnimationDuration(string animationName)
    {
        var a = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName);
        if (a==null)
            a=SkeletonAnimation2.Skeleton.Data.FindAnimation(animationName);
        return a.Duration;
    }
}

