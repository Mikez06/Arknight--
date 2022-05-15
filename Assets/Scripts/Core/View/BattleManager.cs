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
        var loadingUI = UIManager.Instance.ChangeView<MainUI.UI_Loading>(MainUI.UI_Loading.URL);
        loadingUI.m_name.text = battleConfig.MapName;
        SaveHelper.SaveData();
        Pause = true;
        battleTcs = new TaskCompletionSource<bool>();
        var mapInfo = Database.Instance.GetMap(battleConfig.MapName);
        var sceneName = mapInfo.Scene;
        if (string.IsNullOrEmpty(sceneName))
        {
            await SceneManager.LoadSceneAsync("MapBuilder", LoadSceneMode.Additive);
            MapManager.Instance.Build(mapInfo.GridInfos);
            await TimeHelper.Instance.WaitAsync(0.1f);
            Camera.main.transform.position = mapInfo.CameraPos;
            Camera.main.transform.GetComponent<BattleCamera>().startPosition = mapInfo.CameraPos;
           sceneName = "MapBuilder";
            //AstarPath.active.Scan();
        }
        else
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        AudioManager.Instance.PlayBackgroundAudio("battle");
        await TimeHelper.Instance.WaitAsync(0.5f);
        Battle = new Battle();
        Battle.Init(battleConfig);
        await TimeHelper.Instance.WaitAsync(0.1f);
        AstarPath.active.Scan();

        for (int i = 0; i < battleConfig.Team.Cards.Count; i++)
        {
            Card card = battleConfig.Team.Cards[i];
            await ResHelper.Prepare(Database.Instance.GetIndex<UnitData>(card.UnitId), battleConfig.Team.UnitSkill[i]);
        }
        foreach (var wave in mapInfo.WaveInfos)
        {
            if (!string.IsNullOrEmpty(wave.sUnitId))
                await ResHelper.Prepare(Database.Instance.GetIndex<UnitData>(wave.sUnitId));
        }
        foreach (var wave in mapInfo.UnitInfos)
        {
            await ResHelper.Prepare(Database.Instance.GetIndex<UnitData>(wave.UnitId));
        }

        Pause = false;
        var battleUI = UIManager.Instance.ChangeView<BattleUI.UI_Battle>(BattleUI.UI_Battle.URL);
        battleUI.SetBattle(Battle);
        ExcuteTime = 0;
        await battleTcs.Task;
        Debug.Log("ExitBattleScene");
        //Battle = null;
        if (string.IsNullOrEmpty(sceneName))
        {
            await SceneManager.UnloadSceneAsync("MapBuilder", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
        else
        {
            await SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
        EffectManager.Instance.ReturnAll();
        BulletManager.Instance.ReturnAll();
        AudioManager.Instance.PlayBackgroundAudio("main");
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

