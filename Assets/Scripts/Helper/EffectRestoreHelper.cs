using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EffectRestoreHelper
{
    static Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    static string assetsPath = "Assets/2/";
    static string assetsPath2 = "/2/";

    public static void Restore(GameObject go,string path0)
    {
#if UNITY_EDITOR
        //var go = Selection.activeGameObject;
        //狸猫换太子，把引用丢失的脚本去除
        var go1 = new GameObject(go.name.Substring(0, go.name.Length - 7));
        var path1 = go1.name + "/";
        if (!System.IO.Directory.Exists(Application.dataPath + assetsPath2 + path0 + "/" + path1))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + assetsPath2 + path0 + "/" + path1);
        }
        go1.transform.position = go.transform.position;
        go1.transform.rotation = go.transform.rotation;
        go1.transform.localScale = go.transform.localScale;
        if (go.transform.childCount > 0)
        {
            go.transform.GetChild(0).parent = go1.transform;
            GameObject.Destroy(go);
        }
        else
        {
            return;
        }
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{assetsPath}/{path0}/{go1.name}.prefab");
        bool success = false;
        if (prefab == null)
        {
            foreach (Transform t in go1.GetComponentsInChildren<Transform>())
            {
                var m = t.GetComponent<MeshFilter>();
                if (m != null)
                {
                    var newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{assetsPath}{path0}/{path1}{m.name}.asset");
                    if (newMesh == null)
                    {
                        Debug.Log(m.name);
                        AssetDatabase.CreateAsset(m.mesh, $"{assetsPath}{path0}/{path1}{m.name}.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{assetsPath}{path0}/{path1}{m.name}.asset");
                    }
                    m.mesh = newMesh;
                }
                var m1 = t.GetComponent<ParticleSystemRenderer>();
                if (m1 != null && m1.mesh != null)
                {
                    var name = m1.mesh.name.Replace(":", "");
                    Debug.Log(name);
                    var newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{assetsPath}{path0}/{path1}{name}.asset");
                    if (newMesh == null)
                    {
                        var tempMesh = (Mesh)UnityEngine.Object.Instantiate(m1.mesh);
                        AssetDatabase.CreateAsset(tempMesh, $"{assetsPath}{path0}/{path1}{name}.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{assetsPath}{path0}/{path1}{name}.asset");
                    }
                    m1.mesh = newMesh;
                }
                var m2 = t.GetComponent<SkinnedMeshRenderer>();
                if (m2 != null && m2.sharedMesh != null)
                {
                    Debug.Log(m2.sharedMesh.name);
                    var newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{assetsPath}{path0}/{path1}{m2.sharedMesh.name}.asset");
                    if (newMesh == null)
                    {
                        var tempMesh = (Mesh)UnityEngine.Object.Instantiate(m2.sharedMesh);
                        AssetDatabase.CreateAsset(tempMesh, $"{assetsPath}{path0}/{path1}{m2.sharedMesh.name}.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{assetsPath}{path0}/{path1}{m2.sharedMesh.name}.asset");
                    }
                    m2.sharedMesh = newMesh;
                }

                var r = t.GetComponent<Renderer>();
                if (r != null)
                {
                    var ms = r.materials;
                    for (int i = 0; i < r.sharedMaterials.Length; i++)
                    {
                        Material mat = r.sharedMaterials[i];
                        Debug.Log("Mat:" + mat.name);
                        if (mat.name.Contains("Default-Particle") || mat.name.Contains("Default-Material")) continue;
                        Material newMat = AssetDatabase.LoadAssetAtPath<Material>($"{assetsPath}{path0}/{path1}{mat.name}.mat");
                        if (newMat == null)
                        {
                            string matName = "Legacy Shaders/Particles/Additive";
                            if (mat.shader.name.Contains("Additive")) matName = "Legacy Shaders/Particles/Additive";
                            else if (mat.shader.name.Contains("Alpha Blended")|| mat.shader.name.Contains("AlphaBlend")) matName = "Legacy Shaders/Particles/Alpha Blended";
                            else if (mat.shader.name.Contains("Multiply")) matName = "Legacy Shaders/Particles/Multiply";
                            else if (mat.shader.name.Contains("VertexLit Blended")) matName = "Legacy Shaders/Particles//VertexLit Blended";
                            else Debug.LogWarning($"未找到{mat.shader.name}对应的shader");
                            newMat = new Material(Shader.Find(matName));
                            newMat.CopyPropertiesFromMaterial(mat);

                            foreach (var keyword in mat.GetTexturePropertyNames())
                            {
                                //Debug.Log(mat.GetTexture(keyword).name);
                                var texture = mat.GetTexture(keyword) as Texture2D;
                                if (texture == null || texture.name == "Default-Particle") continue;
                                Texture2D newTexture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{assetsPath}{path0}/{path1}{texture.name}.png");
                                if (newTexture == null)
                                {
                                    newTexture = duplicateTexture(texture);
                                    byte[] bytes = newTexture.EncodeToPNG();
                                    System.IO.File.WriteAllBytes(Application.dataPath + $"{assetsPath2}{path0}/{path1}{texture.name}.png", bytes);
                                    AssetDatabase.Refresh();
                                    newTexture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{assetsPath}{path0}/{path1}{texture.name}.png");
                                }
                                newMat.SetTexture(keyword, newTexture);
                                //texture.ReadPixels(new Rect(0,0,))
                            }

                            AssetDatabase.CreateAsset(newMat, $"{assetsPath}{path0}/{path1}{mat.name}.mat");
                            AssetDatabase.Refresh();
                            newMat = AssetDatabase.LoadAssetAtPath<Material>($"{assetsPath}{path0}/{path1}{mat.name}.mat");
                            //Debug.Log(AssetDatabase.GetAssetPath(newMat));
                        }
                        ms[i] = newMat;
                    }
                    r.materials = ms;
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            var result = PrefabUtility.SaveAsPrefabAsset(go1, $"{assetsPath}/{path0}/{go1.name}.prefab");
            if (result != null) success = true;
        }
        else
        {
            GameObject.Destroy(go1);
        }
        if (success)
            GameObject.Destroy(go1);
#endif
    }
}
