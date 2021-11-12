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

    public void Awake()
    {
        Renderer = transform.GetChild(0).GetComponent<Renderer>();
        normal = Renderer.material;
        dark = ResHelper.GetAsset<Material>(PathHelper.OtherPath + "DarkMat");
    }

    public void SetDark(bool bo)
    {
        Renderer.material = bo ? dark : normal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DungeonManager.Instance.Move(DungeonTile);
    }
}

