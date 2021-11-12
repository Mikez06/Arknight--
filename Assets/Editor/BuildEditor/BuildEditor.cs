using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

public class BundleInfo
{
    public List<string> ParentPaths = new List<string>();
}

public enum BuildType
{
    Development,
    Release,
}

public class BuildEditor
{
    private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();

    static AddressableAssetSettings setting;

    [MenuItem("Tools/重新标记")]
    public static void ShowWindow()
    {
        SetPackingTagAndAssetBundle();
    }
    private static void SetPackingTagAndAssetBundle()
    {
        setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
        //ClearPackingTagAndAssetBundle();

        //SetIndependentBundleAndAtlas("Assets/Bundles/Independent");

        SetAssetMark(PathHelper.DataPath, null);

        SetAssetMark(PathHelper.UnitPath, null);

        SetAssetMark(PathHelper.UIPath, null);

        SetAssetMark(PathHelper.BulletPath, null);

        SetAssetMark(PathHelper.EffectPath, null);
        SetAssetMark(PathHelper.OtherPath, null);
        SetAssetMark(PathHelper.DungeonGridPath, null);
        SetStandPic();
        Debug.Log("重新标记完成!");
    }

    static void SetStandPic()
    {
        List<string> paths = EditorResHelper.GetAllResourcePath(PathHelper.SpritePath, true);
        //var group = groupName == null ? setting.DefaultGroup : setting.FindGroup(groupName);
        var group = setting.FindGroup("StandPic");
        var unitDatas = Database.Instance.GetAll<UnitData>().Select(x => x.StandPic).ToList();
        foreach (string path in paths)
        {
            string path1 = path.Replace('\\', '/');
            var guid = AssetDatabase.AssetPathToGUID(path1);
            if (unitDatas.Contains(Path.GetFileNameWithoutExtension(path1)))
            {
                var refence = setting.CreateAssetReference(guid);
                var entry = setting.CreateOrMoveEntry(guid, group);
                if (entry.address.LastIndexOf(".") != -1)
                {
                    entry.SetAddress(entry.address.Substring(0, entry.address.LastIndexOf(".")));
                }
            }
            else
                setting.RemoveAssetEntry(guid);
        }
    }

    private static void SetAssetMark(string dir, string groupName)
    {
        List<string> paths = EditorResHelper.GetAllResourcePath(dir, true);
        var group = groupName == null ? setting.DefaultGroup : setting.FindGroup(groupName);
        foreach (string path in paths)
        {
            string path1 = path.Replace('\\', '/');
            var guid = AssetDatabase.AssetPathToGUID(path1);
            var refence = setting.CreateAssetReference(guid);
            var entry = setting.CreateOrMoveEntry(guid, group);
            if (entry.address.LastIndexOf(".") != -1)
            {
                entry.SetAddress(entry.address.Substring(0, entry.address.LastIndexOf(".")));
            }
            //setting.FindGroup("").entries.Add();
        }
    }

    private static void SetBundleAndAtlasWithoutShare(string dir, string dirPath, bool subDir = false, string bundleName = null)
    {
        List<string> paths = EditorResHelper.GetAllResourcePath(dir, subDir);
        foreach (string path in paths)
        {
            string path1 = path.Replace('\\', '/');
            UnityEngine.Object go = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path1);

            var path0 = path.Substring(7, path.Length - 7);
            path0 = Path.GetDirectoryName(path0) + "\\" + Path.GetFileNameWithoutExtension(path0);

            SetBundle(path1, bundleName == null ? path0 : bundleName);

            //List<string> pathes = CollectDependencies(path1);
            //foreach (string pt in pathes)
            //{
            //	if (pt == path1)
            //	{
            //		continue;
            //	}
            //
            //	SetBundleAndAtlas(pt, go.name);
            //}
        }
    }
    private static void SetBundle(string path, string name, bool overwrite = true)
    {
        string extension = Path.GetExtension(path);
        if (extension == ".cs" || extension == ".dll" || extension == ".js")
        {
            return;
        }
        if (path.Contains("Resources"))
        {
            return;
        }

        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer == null)
        {
            return;
        }

        if (importer.assetBundleName != "" && overwrite == false)
        {
            return;
        }

        if (importer.assetBundleName == name)
        {
            return;
        }

        //Log.Info(path);
        string bundleName = "";
        if (name != "")
        {
            bundleName = $"{name}";
        }

        importer.assetBundleName = bundleName;
    }
}
