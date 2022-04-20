using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitModel : UnitModel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init(Unit unit)
    {
        base.Init(unit);
        if (string.IsNullOrEmpty(BattleManager.Instance.Battle.MapData.Scene))
        {
            var t = BattleManager.Instance.Battle.Map.Tiles[unit.GridPos.x, unit.GridPos.y].MapGrid.transform;
            t.GetComponentInChildren<MeshRenderer>().enabled = false;
            t.GetComponentInChildren<BoxCollider>().center = new Vector3(0, -0.3f, 0);
        }
    }
}
