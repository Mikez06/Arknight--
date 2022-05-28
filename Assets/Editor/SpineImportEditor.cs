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
using ExcelDataReader;

public class SpineImportEditor
{
    const string unitAnimationResourcePath = "Assets/Res/Spine";
    const string unitAnimationPrefab_AssetPath = "Assets/Bundles/Units/";

    [MenuItem("Tools/Spine移动信息")]
    public static async void SpineMoveAnimation()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        DirectoryInfo dirInfo = new DirectoryInfo("Assets/Res/Spine/Enemy");
        FileInfo[] files = dirInfo.GetFiles("*_SkeletonData.asset", SearchOption.AllDirectories);
        StringBuilder sb = new StringBuilder();
        foreach (var item in files)
        {
            string name = item.Name.Substring(0, item.Name.IndexOf("_SkeletonData.asset"));
            string assetPath = "Assets" + item.FullName.Substring(Application.dataPath.Length);
            var dataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(Path.Combine(assetPath));
            var data = dataAsset.GetSkeletonData(true);
            sb.Append(name + "\t");

            if (data.FindAnimation("Move_Begin") != null && data.FindAnimation("Move_Loop") != null && data.FindAnimation("Move_End") != null)
            {
                sb.Append("Move_Begin,Move_Loop,Move_End\r\n");
                dic.Add(name, "Move_Begin,Move_Loop,Move_End");
            }
            else if (data.FindAnimation("Run_Begin") != null && data.FindAnimation("Run_Loop") != null && data.FindAnimation("Run_End") != null)
            {
                sb.Append("Run_Begin,Run_Loop,Run_End\r\n");
                dic.Add(name, "Run_Begin,Run_Loop,Run_End");
            }
            else if (data.FindAnimation("Run_Begin") != null && data.FindAnimation("Run_Loop") != null && data.FindAnimation("Run_End") != null)
            {
                sb.Append("Run_Begin,Run_Loop,Run_End\r\n");
                dic.Add(name, "Run_Begin,Run_Loop,Run_End");
            }
            else if (data.FindAnimation("Run_Loop") != null)
            {
                sb.Append("Run_Loop\r\n");
                dic.Add(name, "Run_Loop");
            }
            else if (data.FindAnimation("Move") != null)
            {
                sb.Append("Move\r\n");
                dic.Add(name, "Move");
            }
            else if (data.FindAnimation("Move_A") != null)
            {
                sb.Append("Move_A\r\n");
                dic.Add(name, "Move_A");
            }
            else if (data.FindAnimation("Move_1") != null)
            {
                sb.Append("Move_1\r\n");
                dic.Add(name, "Move_1");
            }
            else if (data.FindAnimation("Move_Loop") != null)
            {
                sb.Append("Move_Loop\r\n");
                dic.Add(name, "Move_Loop");
            }
            else
            {
                Debug.Log($"{name} 没有任何符合的移动动作！");
                sb.Append("");
            }
        }

        sb.Clear();
        IExcelDataReader reader;
        using (FileStream file = new FileStream("Excel/battle.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            reader = ExcelReaderFactory.CreateReader(file);
            var sheet = reader.AsDataSet().Tables["UnitData"];
            Debug.LogWarning(sheet.Rows[1][62]);
            for (int i = 167; i < 638; i++)
            {
                if (dic.ContainsKey((string)sheet.Rows[i][0]))
                {
                    sb.Append(dic[(string)sheet.Rows[i][0]]+"\r\n");
                    sheet.Rows[i][62] = dic[(string)sheet.Rows[i][0]];
                }
                else
                {
                    sb.Append("\r\n");
                    Debug.Log($"未发现模型{ sheet.Rows[i][0]}");
                    if (!((string)sheet.Rows[i][0]).StartsWith("#"))
                    {
                        sheet.Rows[i][0] = "#" + (string)sheet.Rows[i][0];
                    }
                }
            }
        }
        UnityEngine.GUIUtility.systemCopyBuffer = sb.ToString();
    }

    [MenuItem("Tools/Spine转Prefab")]
    public static async void SpinePrefab()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(unitAnimationResourcePath);
        FileInfo[] files = dirInfo.GetFiles("*_SkeletonData.asset", SearchOption.AllDirectories);
        int index = 0;
        foreach (var item in files)
        {
            string assetPath = "Assets" + item.FullName.Substring(Application.dataPath.Length);
            Debug.Log(assetPath);
            string name = item.Name.Substring(0, item.Name.IndexOf("_SkeletonData.asset"));
            bool front = item.FullName.Contains("\\front\\");
            bool enemy = item.FullName.Contains("\\Enemy\\");
            //EditorUtility.DisplayProgressBar("Create unit spine prefab", name, (float)index++ / files.Length);

            string materialPath = assetPath.Replace("SkeletonData.asset", "Material.mat");
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                Debug.LogError("can't find mat at:" + materialPath);
                continue;
            }
            material.shader = Shader.Find("Spine/Skeleton Tint");
            material.SetFloat("_angle", 60);
            
            var dataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(Path.Combine(assetPath));
            //if (dataAsset.scale == 0.01f)
            //{
                dataAsset.scale = 0.003f * 0.9f; 
                EditorUtility.SetDirty(dataAsset);
            //}
            //dataAsset.
            //if (dataAsset.atlasAssets == null || dataAsset.atlasAssets.Length == 0)
            //{
            //    SpineEditorUtilities.ImportSpineContent(new string[] { AssetDatabase.GetAssetPath(dataAsset.skeletonJSON) }, true);
            //    EditorUtility.SetDirty(dataAsset);
            //}
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
            if (sa.skeletonDataAsset != dataAsset)
            {
                sa.skeletonDataAsset = dataAsset;
                sa.Initialize(true);
                EditorUtility.SetDirty(root);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        //AssetDatabase.Instance.Refresh();
    }
}
