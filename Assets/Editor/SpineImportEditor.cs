using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Spine.Unity.Editor;

public class SpineImportEditor
{
    const string unitAnimationResourcePath = "Assets/Res/Spine";
    const string unitAnimationPrefab_AssetPath = "Assets/Bundles/Units/";
    [MenuItem("Tools/Spine转Prefab")]
    public static async void SpinePrefab()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(unitAnimationResourcePath);
        FileInfo[] files = dirInfo.GetFiles("*_SkeletonData.asset", SearchOption.AllDirectories);
        int index = 0;
        foreach (var item in files)
        {
            string assetPath = "Assets" + item.FullName.Substring(Application.dataPath.Length);
            string name = item.Name.Substring(0, item.Name.IndexOf("_SkeletonData.asset"));
            bool front = item.FullName.Contains("\\front\\");
            bool enemy = item.FullName.Contains("\\Enemy\\");
            //EditorUtility.DisplayProgressBar("Create unit spine prefab", name, (float)index++ / files.Length);

            string materialPath = assetPath.Replace("SkeletonData.asset", "Material.mat");
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            material.shader = Shader.Find("Spine/Skeleton Tint");
            var dataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(Path.Combine(assetPath));
            dataAsset.scale = 0.003f;
            //dataAsset.
            if (dataAsset.atlasAssets == null || dataAsset.atlasAssets.Length == 0)
            {
                SpineEditorUtilities.ImportSpineContent(new string[] { AssetDatabase.GetAssetPath(dataAsset.skeletonJSON) }, true);
                EditorUtility.SetDirty(dataAsset);
            }
            var root = AssetDatabase.LoadAssetAtPath<GameObject>(unitAnimationPrefab_AssetPath + name + ".prefab");
            if (root == null)
            {
                root = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Res/Spine/{(enemy ? "Enemy" : "Unit")}.prefab"));
                PrefabUtility.SaveAsPrefabAsset(root, string.Format("{0}/{1}.prefab", unitAnimationPrefab_AssetPath, name));
                GameObject.DestroyImmediate(root);
                AssetDatabase.ImportAsset(Path.Combine(assetPath));
                root = AssetDatabase.LoadAssetAtPath<GameObject>(unitAnimationPrefab_AssetPath + name + ".prefab");
            }
            SkeletonAnimation sa;
            if (enemy || front) sa = root.transform.GetChild(1).GetComponent<SkeletonAnimation>();
            else sa = root.transform.GetChild(2).GetComponent<SkeletonAnimation>();
            sa.skeletonDataAsset = dataAsset;
            sa.Initialize(true);
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
        //AssetDatabase.Instance.Refresh();
    }
}
