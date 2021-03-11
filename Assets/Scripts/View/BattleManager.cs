using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("BattleManager");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<BattleManager>();
            }
            return instance;
        }
    }
    private static BattleManager instance;

    Battle Battle;

    public float startTime;
    private void Update()
    {
        if (Battle != null)
        {
            int frame = Mathf.FloorToInt((Time.time - startTime - Battle.Tick * SystemConfig.DeltaTime) / SystemConfig.DeltaTime);
            for (int i = 0; i < frame; i++)
            {
                Battle.Update();
            }
        }
    }
    public async void StartBattle(BattleInput battleConfig)
    {
        SceneManager.LoadScene(battleConfig.MapName);
        var battleUI = UIManager.Instance.ChangeView<BattleUI.UI_Battle>(BattleUI.UI_Battle.URL);
        await TimeHelper.Instance.WaitAsync(0.5f);
        Battle = new Battle();
        Battle.Init(battleConfig);
        battleUI.SetBattle(Battle);
        startTime = Time.time;
        //var path = Battle.Map.FindPath(Battle.Map.Grids[1, 3], Battle.Map.Grids[8, 1]);
        //foreach (var grid in path)
        //{
        //    Debug.Log(grid.X + "," + grid.Y);
        //}
    }
}

