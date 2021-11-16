using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class DungeonUnit : MonoBehaviour
{
    public SkeletonAnimation SkeletonAnimation;
    UnitData UnitData;

    private void Start()
    {
        Destroy(GetComponent<UnitModel>());
        var sks = GetComponentsInChildren<SkeletonAnimation>();
        SkeletonAnimation = sks[0];
        SkeletonAnimation.gameObject.SetActive(true);
        SkeletonAnimation.state.SetAnimation(0, "Idle", true);
        SkeletonAnimation.GetComponent<Renderer>().sortingOrder = 10;
        for (int i = 1; i < sks.Length; i++)
        {
            sks[i].gameObject.SetActive(false);
        }
    }

    public void SetUnit(UnitData unitData)
    {
        this.UnitData = unitData;
    }

    public async Task MoveTo(Vector3 pos)
    {
        var tween = DOTween.To(() => -30f, setAngleZ, 30f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        tween.fullPosition = 1;
        await transform.DOMove(pos + new Vector3(0, 0, -0.25f), (pos - transform.position).magnitude / 1f).SetEase(Ease.Linear).Wait();
        tween.Kill();
        setAngleZ(0);
    }

    public async Task ShowAttack()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        var skInfo = Database.Instance.Get<SkillData>(UnitData.Skills[0]).ModelAnimation;
        SkeletonAnimation.state.SetAnimation(0, skInfo.Length == 1 ? skInfo[0] : skInfo[1], false).Complete += ((x) =>
        {
            tcs.SetResult(true);
        });
        await tcs.Task;
        SkeletonAnimation.state.SetAnimation(0, UnitData.IdleAnimation[0], true);
    }

    void setAngleZ(float x)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, x);
    }
}
