using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : Singleton<DungeonManager>
{
    public Dungeon Dungeon;
    public DungeonUnit DungeonUnit;

    public void PrepareDungeon()
    {
        if (Dungeon != null) return;
        Dungeon = new Dungeon();
        Dungeon.StartCard = new DungeonCard();
    }

    public async Task StartDungeon()
    {
        await SceneManager.LoadSceneAsync("Dungeon");
        await TimeHelper.Instance.WaitAsync(0.1f);
        Dungeon.RebuildMap(8, 4);
        DungeonBuilder.Instance.ReBuild();
        var unitGo= ResHelper.Instantiate(PathHelper.UnitPath + Dungeon.StartCard.UnitData.Model);
        DungeonUnit = unitGo.AddComponent<DungeonUnit>();
        DungeonUnit.transform.position = Dungeon.NowTile.WorldPos;
        foreach (var tile in Dungeon.Tiles)
        {
            tile.TileController.SetDark(!tile.InSight);
        }
    }

    public async Task Move(DungeonTile dungeonTile)
    {
        Dungeon.MoveTo(dungeonTile);
        await DungeonUnit.MoveTo(dungeonTile.WorldPos);
        foreach (var tile in Dungeon.Tiles)
        {
            tile.TileController.SetDark(!tile.InSight);
        }
    }
}