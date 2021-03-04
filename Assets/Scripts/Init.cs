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
        StartBattle(new BattleConfig()
        {
            MapName = "TestMap",
        });
    }

    private void FixedUpdate()
    {
        //Battle.Update();
    }

    async void StartBattle(BattleConfig battleConfig)
    {
        SceneManager.LoadScene(battleConfig.MapName);
        await TimeHelper.Instance.WaitAsync(0.001f);
        Battle = new Battle();
        Battle.Init(battleConfig);
        var path = Battle.Map.FindPath(Battle.Map.Grids[1, 3], Battle.Map.Grids[8, 1]);
        foreach (var grid in path)
        {
            Debug.Log(grid.X + "," + grid.Y);
        }
    }
}
