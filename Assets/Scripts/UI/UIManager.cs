using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using System.IO;
using UnityEngine.AddressableAssets;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    const string PackagePath = "Assets/Bundles/UI/";
    List<string> LoadedPackages = new List<string>();

    Dictionary<string, GComponent> scenes = new Dictionary<string, GComponent>();
    GComponent nowScene;

    private void Awake()
    {
        Debug.Log("UIManagerInit");
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BattleUI.BattleUIBinder.BindAll();
        MainUI.MainUIBinder.BindAll();
        DungeonUI.DungeonUIBinder.BindAll();
        LoadPackge("BattleUI");
        LoadPackge("MainUI");
        LoadPackge("SkillIcon");
        LoadPackge("Res");
        LoadPackge("DungeonUI");
        //LoadPackge("UnitFace");
        //LoadPackge("UnitPic");
    }

    private void Start()
    {

    }

    public T GetView<T>(string url) where T : GComponent
    {
        if (scenes.TryGetValue(url, out GComponent r))
        {
            return r as T;
        }
        return null;
    }

    public T ChangeView<T>(string url) where T : GComponent
    {
        var view = GetView<T>(url);
        if (view == null)
        {
            //LoadPackge(packageName);
            view = UIPackage.CreateObjectFromURL(url) as T;
            scenes.Add((url), view);
            view.SetSize(GRoot.inst.size.x, GRoot.inst.size.y);
            view.AddRelation(GRoot.inst, RelationType.Size);
            GRoot.inst.AddChild(view);
        }
        if (nowScene != null) nowScene.visible = false;
        nowScene = view;
        view.visible = true;
        if (view is IGameUIView gameView)
        {
            gameView.Enter();
        }
        return view;
    }

    public void LoadPackge(string PackageName)
    {
        if (LoadedPackages.Contains(PackageName))
            return;
        LoadedPackages.Add(PackageName);
        var operation = Addressables.LoadAssetAsync<TextAsset>(PathHelper.UIPath + PackageName + "_fui");
        var bytes = operation.WaitForCompletion().bytes;
        UIPackage.AddPackage(bytes, PackageName, load);
    }

    object load(string name, string extension, System.Type type, out DestroyMethod destroyMethod)
    {
        destroyMethod = DestroyMethod.Unload;
        try
        {
            var op = Addressables.LoadAssetAsync<UnityEngine.Object>(PathHelper.UIPath + name);
            op.WaitForCompletion();
            return op.Result;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    AssetBundle getBundle(string name)
    {
        string p = Path.Combine(PathHelper.AppResPath, PathHelper.UIPath, name.ToLower());
        if (File.Exists(p))
        {
            Debug.Log(p);
            return AssetBundle.LoadFromFile(p);
        }
        else
        {
            p = Path.Combine(PathHelper.AppHotfixResPath, PathHelper.UIPath, name.ToLower());
            Debug.Log(p);
            if (File.Exists(p))
            {
                return AssetBundle.LoadFromFile(p);
            }
            else
            {
                throw new Exception("cant find" + name);
                return null;
            }
        }
    }
}