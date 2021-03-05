using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using System.IO;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    const string PackagePath = "Assets/Bundles/UI/";
    List<string> LoadedPackages = new List<string>();

    Dictionary<string, GComponent> scenes = new Dictionary<string, GComponent>();
    GComponent nowScene;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        BattleUI.BattleUIBinder.BindAll();
        LoadPackge("BattleUI");
    }

    private void Start()
    {
        try
        {
            //ChangeView<GComponent>(StartPackage, StartView);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
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
#if UNITY_EDITOR
        UIPackage.AddPackage(PackagePath + PackageName);
#else
        string p = Path.Combine(PathHelper.AppHotfixResPath, PathHelper.UIPath, PackageName.ToLower());
            AssetBundle res = null;
            string resPath = p + "res";
            if (File.Exists(resPath))
                res = AssetBundle.LoadFromFile(resPath);
            var desc = AssetBundle.LoadFromFile(p + "desc");
            if (res != null)
                UIPackage.AddPackage(desc, res);
            else
            {
                UIPackage.AddPackage(desc);
            }
#endif
    }
}