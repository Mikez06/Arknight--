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

    public Battle Battle;

    public bool Pause;

    public float ExcuteTime;


    private void Update()
    {
        if (Battle != null)
        {
            if (Pause) return;
            ExcuteTime += Time.deltaTime;
            int frame = Mathf.FloorToInt(ExcuteTime / SystemConfig.DeltaTime);
            for (int i = 0; i < frame - Battle.Tick; i++)
            {
                Battle.Update();
            }
        }
    }

    TaskCompletionSource<bool> battleTcs;

    public async Task StartBattle(BattleInput battleConfig)
    {
        battleTcs = new TaskCompletionSource<bool>();
        var sceneName = Database.Instance.GetMap(battleConfig.MapName).Scene;
        var scene = await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        var battleUI = UIManager.Instance.ChangeView<BattleUI.UI_Battle>(BattleUI.UI_Battle.URL);
        await TimeHelper.Instance.WaitAsync(0.5f);
        Battle = new Battle();
        Battle.Init(battleConfig);
        battleUI.SetBattle(Battle);
        ExcuteTime = 0;
        await battleTcs.Task;
        Debug.Log("ExitBattleScene");
        //Battle = null;
        await SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        EffectManager.Instance.ReturnAll();
        BulletManager.Instance.ReturnAll();
        //var path = Battle.Map.FindPath(Battle.Map.Grids[1, 3], Battle.Map.Grids[8, 1]);
        //foreach (var grid in path)
        //{
        //    Debug.Log(grid.X + "," + grid.Y);
        //}
    }

    public void FinishBattle()
    {
        battleTcs?.TrySetResult(true);
    }
}

