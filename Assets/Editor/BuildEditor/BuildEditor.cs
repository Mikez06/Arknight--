using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
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

    [MenuItem("Tools/重新标记")]
    public static void ShowWindow()
    {
        SetPackingTagAndAssetBundle();
    }
    private static void SetPackingTagAndAssetBundle()
    {
        //ClearPackingTagAndAssetBundle();

        //SetIndependentBundleAndAtlas("Assets/Bundles/Independent");

        //把data文件全打进一个包里
        SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.DataPath + "/", PathHelper.DataPath + "/", false, "Bundles/Data");

        //SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.MapPath, PathHelper.MapPath, true);

        //SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.SpritePath + "/", PathHelper.SpritePath + "/", true);

        SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.UnitPath, PathHelper.UnitPath);
        SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.BulletPath, PathHelper.BulletPath);
        SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.EffectPath, PathHelper.EffectPath);


        SetBundleAndAtlasWithoutShare("Assets/Bundles/Other/", "Bundles/Other/");
        //SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.TimelinePath, PathHelper.TimelinePath, true);

        SetFairyGUI();

        //SetRootBundleOnly("Assets/Bundles/Unit");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
    }
    private static void SetFairyGUI()
    {
        string end = "_fui.bytes";
        List<string> paths = EditorResHelper.GetAllResourcePath("Assets/" + PathHelper.UIPath, true);
        foreach (var path in paths)
        {
            if (path.EndsWith(end))
            {
                string start = path.Substring(0, path.Length - end.Length);
                //Debug.Log(start);
                foreach (var path0 in paths)
                {
                    if (path0.StartsWith(start))
                    {
                        string path1 = path0.Replace('\\', '/');
                        UnityEngine.Object go = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path1);

                        SetBundle(path1, Path.Combine(PathHelper.UIPath, Path.GetFileNameWithoutExtension(start) + (path0 == path ? "desc" : "res")));
                    }
                }
            }
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
