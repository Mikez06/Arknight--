using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    Battle Battle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await Database.Instance.InitAsync();
        StartBattle(new BattleInput()
        {
            MapName = "TestMap",
            UnitInputs = new UnitInput[]
            {
                new UnitInput(){ Id=2 },
                new UnitInput(){ Id=2 },
                new UnitInput(){ Id=2 },
                new UnitInput(){ Id=2 },
            }
        });
    }

    private void FixedUpdate()
    {
        Battle?.Update();
    }

    async void StartBattle(BattleInput battleConfig)
    {
        SceneManager.LoadScene(battleConfig.MapName);
        var battleUI = UIManager.Instance.ChangeView<BattleUI.UI_Battle>(BattleUI.UI_Battle.URL);
        await TimeHelper.Instance.WaitAsync(0.001f);
        Battle = new Battle();
        Battle.Init(battleConfig);
        battleUI.SetBattle(Battle);
        //var path = Battle.Map.FindPath(Battle.Map.Grids[1, 3], Battle.Map.Grids[8, 1]);
        //foreach (var grid in path)
        //{
        //    Debug.Log(grid.X + "," + grid.Y);
        //}
    }
}
