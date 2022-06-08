using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ABExportTool
{
    public static AssetBundleManifest AssetBundleManifestObject;
    static string AssetsPath;

    public static void Init()
    {
        AssetsPath = Application.streamingAssetsPath + "/Android/";
        AssetBundleManifestObject = AssetBundle.LoadFromFile(AssetsPath + "torappu.ab").LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        foreach (var s in AssetBundleManifestObject.GetAllAssetBundles())
        {
            //Debug.Log(s);
        }
    }

    public static void LoadAssets(string assetPath)
    {
        var dependence = AssetBundleManifestObject.GetAllDependencies(assetPath);
        foreach (var path in dependence)
        {
            AssetBundle.LoadFromFile(AssetsPath + path);
        }

        var ab = AssetBundle.LoadFromFile(AssetsPath + assetPath);

        var assets = ab.LoadAllAssets();
        foreach (var ob in assets)
        {
            if (ob.GetType() == typeof(GameObject))
            {
                GameObject.Instantiate(ob);
            }
            //Debug.Log(ob.name + "," + ob.GetType());
        }
    }
    public static void SaveAssets(string assetPath)
    {
        var dependence = AssetBundleManifestObject.GetAllDependencies(assetPath);
        List<AssetBundle> abs = new List<AssetBundle>();
        foreach (var path in dependence)
        {
            var a = AssetBundle.LoadFromFile(AssetsPath + path);
            abs.Add(a);
        }

        var ab = AssetBundle.LoadFromFile(AssetsPath + assetPath);

        var assets = ab.LoadAllAssets();
        foreach (var ob in assets)
        {
            if (ob.GetType() == typeof(GameObject))
            {
                //GameObject.Instantiate(ob as GameObject);
                EffectRestoreHelper.Restore(GameObject.Instantiate(ob as GameObject), System.IO.Path.GetFileNameWithoutExtension(assetPath));
            }
            //Debug.Log(ob.name + "," + ob.GetType());
        }
        foreach (var a in abs)
        {
            a.Unload(true);
        }
        ab.Unload(true);
    }

    public static async void LoadScene(string scenePath, string sceneName)
    {
        var dependence = AssetBundleManifestObject.GetAllDependencies(scenePath);
        foreach (var path in dependence)
        {
            Debug.Log(path);
            AssetBundle.LoadFromFile(AssetsPath + path);
        }
        AssetBundle.LoadFromFile(AssetsPath + scenePath);
        SceneManager.LoadSceneAsync(sceneName).completed += (x) =>
        {
            //foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            //{
            //    UnityEditor.GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            //}
        };
    }

    public static void CopyFile(string assetsPath)
    {
        var dependence = AssetBundleManifestObject.GetAllDependencies(assetsPath);
        foreach (var path in dependence)
        {
            CopyAssets(path);
            //Debug.Log(AssetsPath + path + "," + Path.GetFileName(AssetsPath + path));
        }
        CopyAssets(assetsPath);
    }

    static string Dir = "E:/111/1/";
    private static void CopyAssets(string path)
    {
        var targetDir = Path.GetDirectoryName(Dir + path);
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }
        File.Copy(AssetsPath + path, Dir + path, true);
    }

    private static void CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
    {
        if (File.Exists(SourcePath))
        {
            var dir = Path.GetDirectoryName(DestinationPath);
            Debug.Log(dir);
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            File.Copy(SourcePath, DestinationPath, overwriteexisting);
        }
    }
}
