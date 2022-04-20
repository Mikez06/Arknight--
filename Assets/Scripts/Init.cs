using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Init : MonoBehaviour
{
    public static Init Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            if (args.Exception.InnerException != null)
            {
                Debug.LogError(args.Exception.InnerException);
            }
            else
                Debug.LogError(args.Exception);
        };
    }

    private async void Start()
    {
        await UnityEngine.AddressableAssets.Addressables.InitializeAsync().Task;
        await Database.Instance.Init();
        //ModifyManager.Instance.Init();
        GameData.Instance.TestInit();
        //Debug.Log(Database.Instance.Get<UnitData>(0).Id);
        AudioManager.Instance.PlayBackgroundAudio("main");
        var battleUI = UIManager.Instance.ChangeView<MainUI.UI_Main>(MainUI.UI_Main.URL);
        //StartBattle(new BattleInput()
        //{
        //    MapName = "TestMap",
        //    UnitInputs = new UnitInput[]
        //    {
        //        new UnitInput(){ Id=2 },
        //        new UnitInput(){ Id=3 },
        //        new UnitInput(){ Id=3 },
        //        new UnitInput(){ Id=4 },
        //    },
        //    StartCost=50,
        //});
    }
}
