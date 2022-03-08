using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Spine.Unity;

public class SpineModel : UnitModel
{
    public SkeletonAnimation SkeletonAnimation;
    protected Renderer Renderer;
    protected MaterialPropertyBlock mpb;
    string[] nowAnimations;

    public Transform Shadow;
    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        Renderer = SkeletonAnimation.GetComponent<Renderer>();
        Renderer.GetPropertyBlock(mpb);
        Shadow = transform.GetChild(0);
    }

    public override void Init(Unit unit)
    {
        this.Unit = unit;
        SkeletonAnimation.AnimationName = "Default";
        nowAnimations = Unit.DefaultAnimation;
        SkeletonAnimation.SkeletonDataAsset.GetAnimationStateData().DefaultMix = 0f;
        updateState();
        Shadow.localScale = Vector3.one * unit.UnitData.ModelScale;
    }

    public void LateUpdate()
    {
        if (Unit != null)
            updateState();
    }

    public override Vector3 GetModelPositon()
    {
        return SkeletonAnimation.transform.position;
    }

    public override Vector3 GetPoint(string name)
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
        if (Unit is Units.敌人 u)
        {
            gameObject.SetActive(u.Visiable);
            if (!u.Visiable) return;
        }
        SkeletonAnimation.transform.localScale = new Vector3(Unit.ScaleX, 1, 1) * Unit.UnitData.ModelScale;
        SkeletonAnimation.transform.localPosition = new Vector3(0, Unit.Height, Unit.Height*0.45f);//没什么道理的z轴偏移
        if (Unit.State == StateEnum.Default)
        {
            return;
        }
        transform.position = Unit.Position;

        if (nowAnimations!=Unit.GetAnimation())
        {
            changeAnimation(Unit.GetAnimation());
        }
        if (Unit.AnimationSpeed != SkeletonAnimation.timeScale)
        {
            SkeletonAnimation.timeScale = Unit.AnimationSpeed;
        }
    }

    protected virtual void changeAnimation(string[] animations)
    {
        var nowAnimation = SkeletonAnimation.AnimationName;
        if (animations == null || animations.Length == 0)//如果模型上不存在目标动画 那么就把当前动作暂停住 一般用于怪物被击晕
        {
            nowAnimations = animations;
            SkeletonAnimation.timeScale = 0;
            return;
        }
        //SkeletonAnimation.state.SetEmptyAnimation(0, 0);
        SkeletonAnimation.Skeleton.SetToSetupPose();
        SkeletonAnimation.state.ClearTracks();
        if (animations[0].Contains("Idle"))//从其他状态返回Idle时，如果有退出动画，就播放
        {
            if (nowAnimations.Length >= 3 && nowAnimation != nowAnimations[2] && SkeletonAnimation.Skeleton.Data.FindAnimation(nowAnimations[2]) != null)
            {
                Debug.Log($"{Unit.UnitData.Id}播放切出动画{nowAnimations[2]} ,至{animations[0]}");
                SkeletonAnimation.state.AddAnimation(0, nowAnimations[2], false, 0);
            }
        }
        float delay = 0;
        //切入其他状态时，若有进入动画，播放
        if (!nowAnimations.StringsEqual(animations) && animations.Length>1)
        {
            Debug.Log($"{Unit.UnitData.Id}播放切入动画{animations[0]}");
            var _beginAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(animations[0]);
            SkeletonAnimation.state.AddAnimation(0, animations[0], false, 0);
            delay += _beginAnimation.Duration;
        }
        var nextAnimation = animations.Length > 1 ? animations[1] : animations[0];
        var loop = (animations[0] == "Start" || animations[0] == "Appear" || animations[0] == "Die") ? false : true;
        //SkeletonAnimation.state.AddAnimation(0, nextAnimation, nextAnimation == "Move"|| nextAnimation == "Idle" || nextAnimation.EndsWith("Loop"), delay);
        SkeletonAnimation.state.AddAnimation(0, nextAnimation, loop, delay);
        nowAnimations = animations;
    }

    public override void ChangeToEnd()
    {
        if (nowAnimations.Length >= 3)
        {
            SkeletonAnimation.state.ClearTracks();
            SkeletonAnimation.state.AddAnimation(0, nowAnimations[2], false, 0);//.Complete += (x) => { Unit.OverWriteAnimation = null; };
        }
    }

    public override void BreakAnimation()
    {
        var next = Unit.GetAnimation();
        //Debug.Log($"break from {} to {next}");
        changeAnimation(next);
    }

    public override float GetSkillDelay(string[] animationName,string[] lastState,out float fullDuration,out float beginDuration)
    {
        float result = 0;
        fullDuration = 0;
        beginDuration = 0;
        //如果有状态切换，那么去判断前摇和后摇动画
        if (!animationName.StringsEqual(lastState) && animationName.Length>1)
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

    public override float GetAnimationDuration(string animationName)
    {
        var result = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName);;
        return result == null ? 0 : result.Duration;
    }

    public override void SetColor(Color color)
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

