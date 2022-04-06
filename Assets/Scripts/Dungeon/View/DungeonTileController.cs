using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonTileController : MonoBehaviour,IPointerClickHandler
{
    public DungeonTile DungeonTile;

    //GameObject normal, dark;

    Renderer Renderer;
    Material normal, dark;
    SpriteRenderer sr;

    GameObject enemyModel;
    UnitData UnitData;

    public void Awake()
    {
        Renderer = transform.GetChild(0).GetComponent<Renderer>();
        if (transform.childCount > 1)
            sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        normal = Renderer.material;
        dark = ResHelper.GetAsset<Material>(PathHelper.OtherPath + "DarkMat");
    }

    public void BuildBuildingModel()
    {
        //enemyModel = ResHelper.Instantiate(PathHelper.UnitPath + DungeonTile.MapData.MapModel);
        //UnitData= Database.Instance.GetAll<UnitData>().FirstOrDefault(x => x.Model == DungeonTile.MapData.MapModel);
        enemyModel.transform.parent = transform;
        enemyModel.transform.localPosition = Vector3.zero;
        enemyModel.transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
        enemyModel.transform.localPosition = new Vector3(0, 0, -0.25f);
        enemyModel.GetComponentInChildren<Spine.Unity.SkeletonAnimation>().state.SetAnimation(0, UnitData.IdleAnimation[0], true);
    }

    public void DestoryBuildingModel()
    {
        ResHelper.Return(enemyModel);
        enemyModel = null;
    }

    public void SetDark(bool bo)
    {
        Renderer.material = bo ? dark : normal;
        if (sr != null)
        {
            sr.color = bo ? Color.black : Color.white;
        }
        if (DungeonTile.TileType == DungeonTileTypeEnum.Battle && DungeonTile.MapId != null)
        {
            if (!bo && enemyModel == null)
                BuildBuildingModel();
            else if (bo && enemyModel != null)
            {
                DestoryBuildingModel();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FairyGUI.Stage.isTouchOnUI) return;
        if ((eventData.position - eventData.pressPosition).magnitude > 10f) return;//移动超过十个像素，算你在拖拽
        UIManager.Instance.GetView<DungeonUI.UI_Map>(DungeonUI.UI_Map.URL).SelectTile(DungeonTile);
        //DungeonManager.Instance.Move(DungeonTile);
    }

    public async Task ShowDie()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        var SkeletonAnimation = enemyModel.GetComponentInChildren<Spine.Unity.SkeletonAnimation>();
        SkeletonAnimation.state.SetAnimation(0, Unit.DieAnimation[0], false).Complete += ((x) =>
        {
            tcs.SetResult(true);
        });
        await tcs.Task;
        Destroy(SkeletonAnimation.gameObject);
        SkeletonAnimation = null;
    }
}

