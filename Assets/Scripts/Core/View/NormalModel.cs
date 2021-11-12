using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NormalModel : UnitModel
{
    Animator Animator;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }
    public override void Init(Unit unit)
    {
        this.Unit = unit;
        Animator.Play(Unit.AnimationName[0]);
    }


    private void LateUpdate()
    {
        transform.position = Unit.Position;
        Animator.Play(Unit.AnimationName[0]);
        Animator.speed = Unit.AnimationSpeed;
    }

    public override void BreakAnimation()
    {
        base.BreakAnimation();
        Animator.Play(Unit.AnimationName[0], 0, 0);
    }

    public override float GetAnimationDuration(string animationName)
    {
        var result = Animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == animationName);
        return result.length;
    }

    public override float GetSkillDelay(string[] animationName, string[] lastState, out float fullDuration, out float beginDuration)
    {
        var ani = Animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == animationName[0]);
        fullDuration = ani.length;
        beginDuration = 0;
        return fullDuration / 2;
    }
}