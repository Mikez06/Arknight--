﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class PlayerUnitModel : UnitModel
{
    public SkeletonAnimation SkeletonAnimation2;
    bool forward = false;

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
        bool backward = angle > 45 && angle < 135;//在这个角度下，显示干员背面
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
}

