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

    private void Start()
    {
        Destroy(GetComponent<UnitModel>());
        var sks = GetComponentsInChildren<SkeletonAnimation>();
        SkeletonAnimation = sks[0];
        SkeletonAnimation.gameObject.SetActive(true);
        SkeletonAnimation.state.SetAnimation(0, "Idle", true);
        for (int i = 1; i < sks.Length; i++)
        {
            sks[i].gameObject.SetActive(false);
        }
    }

    public void SetUnit(UnitData unitData)
    {

    }

    public async Task MoveTo(Vector3 pos)
    {
        var tween = DOTween.To(() => -30f, setAngleZ, 30f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        tween.fullPosition = 1;
        await transform.DOMove(pos, (pos - transform.position).magnitude / 1f).SetEase(Ease.Linear).Wait();
        tween.Kill();
        setAngleZ(0);
    }

    void setAngleZ(float x)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, x);
    }
}
