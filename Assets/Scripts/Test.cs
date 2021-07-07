using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Linq;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        Core.Dungeon dungeon = new Core.Dungeon();
        dungeon.CreateDungeon(5, 10, 3);
        foreach (var tile in dungeon.Tiles)
        {
            Debug.Log(tile.X + "," + tile.Y);
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.transform.position = new Vector3(tile.X, tile.Y);
        }
        //ResHelper.Return(pic);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
