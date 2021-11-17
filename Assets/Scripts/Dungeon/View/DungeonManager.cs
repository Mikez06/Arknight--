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
        var card = Dungeon.AddUnit(0);
        Dungeon.StartCard = card;
    }

    public async Task StartDungeon()
    {
        await SceneManager.LoadSceneAsync("Dungeon");
        await TimeHelper.Instance.WaitAsync(0.1f);
        Dungeon.RebuildMap(8, 4);
        DungeonBuilder.Instance.ReBuild();
        var unitGo= ResHelper.Instantiate(PathHelper.UnitPath + Dungeon.StartCard.UnitData.Model);
        DungeonUnit = unitGo.AddComponent<DungeonUnit>();
        DungeonUnit.SetUnit(Dungeon.StartCard.UnitData);
        DungeonUnit.transform.position = Dungeon.NowTile.WorldPos + new Vector3(0, 0, -0.25f);
        DungeonUnit.transform.localScale *= 0.7f;
        DungeonUnit.transform.SetParent(DungeonRoot.Instance.transform);
        foreach (var tile in Dungeon.Tiles)
        {
            tile.TileController.SetDark(!tile.InSight);
            //if (tile.InSight && tile.TileType == DungeonTileTypeEnum.Battle) tile.TileController.BuildBuildingModel();
        }
    }

    public async Task Move(DungeonTile dungeonTile)
    {
        if (!Dungeon.CanMove(dungeonTile)) return;
        bool ifBattle = dungeonTile.TileType == DungeonTileTypeEnum.Battle;
        Dungeon.MoveTo(dungeonTile);
        await DungeonUnit.MoveTo(dungeonTile.WorldPos + (ifBattle ? new Vector3(-0.55f, 0, 0) : Vector3.zero));
        foreach (var tile in Dungeon.Tiles)
        {
            tile.TileController.SetDark(!tile.InSight);
        }
        await TimeHelper.Instance.WaitAsync(0.3f);
        if (ifBattle)
        {
            //进入战斗流程
            var dungeonBattle = UIManager.Instance.ChangeView<DungeonUI.UI_Battle>(DungeonUI.UI_Battle.URL);
            var contract = await dungeonBattle.BuildTeam(Dungeon.NowTile.MapId);

            DungeonRoot.Instance.gameObject.SetActive(false);
            await BattleManager.Instance.StartBattle(new BattleInput()
            {
                Contracts = new List<int>(contract),
                Dungeon = Dungeon,
                MapName = Dungeon.NowTile.MapId,
            });
            DungeonRoot.Instance.gameObject.SetActive(true);
            UIManager.Instance.ChangeView<DungeonUI.UI_Map>(DungeonUI.UI_Map.URL);

            //战斗结束
            await DungeonUnit.ShowAttack();
            await TimeHelper.Instance.WaitAsync(0.2f);
            await dungeonTile.TileController.ShowDie();
            await DungeonUnit.MoveTo(dungeonTile.WorldPos);
        }
    }
}