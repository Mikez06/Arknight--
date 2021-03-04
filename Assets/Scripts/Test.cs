using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;

public class Test : MonoBehaviour
{
    public SkeletonAnimation SkeletonAnimation;

    // Start is called before the first frame update
    void Start()
    {
        SkeletonAnimation = GetComponent<SkeletonAnimation>();
        var skeletondata= GetComponent<SkeletonAnimation>().Skeleton.Data;
        Debug.Log(GetSkillDelay("Attack"));
        //foreach (var animation in skeletondata.Animations)
        //{
        //    foreach (var e in animation.Timelines)
        //    {
        //        if (e is Spine.EventTimeline eventTimeline)
        //        {
        //            foreach (var ev in eventTimeline.Events)
        //            {
        //                Debug.Log(animation.Name + "," + animation.Duration + "," + ev.Time + "," + ev.Data);
        //            }
        //        }
        //    }
        //}
        //sa.state.AddAnimation(0, "Default", false, 0);
        //for (int i = 0; i < 100; i++)
        //{
        //    sa.state.AddAnimation(0, "Attack_Begin", false, 0);
        //    sa.state.AddAnimation(0, "Attack", false, 0);
        //    sa.state.AddAnimation(0, "Attack_End", false, 0);
        //}
    }

    // Update is called once per frame
    void Update()
    {

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
