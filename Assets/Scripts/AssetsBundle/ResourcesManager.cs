using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

    public class ABInfo : Component
    {
        private int refCount;
        public string Name { get; }

        public int RefCount
        {
            get
            {
                return this.refCount;
            }
            set
            {
                //Log.Debug($"{this.Name} refcount: {value}");
                this.refCount = value;
            }
        }

        public AssetBundle AssetBundle { get; }

        public ABInfo(string name, AssetBundle ab)
        {
            this.Name = name;
            this.AssetBundle = ab;
            this.RefCount = 1;
            //Log.Debug($"load assetbundle: {this.Name}");
        }

        public void Dispose()
        {
            AssetBundle?.Unload(true);
        }
    }

    // 用于字符串转换，减少GC
    public static class AssetBundleHelper
    {
        public static readonly Dictionary<int, string> IntToStringDict = new Dictionary<int, string>();

        public static readonly Dictionary<string, string> StringToABDict = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> BundleNameToLowerDict = new Dictionary<string, string>()
        {
            { "StreamingAssets", "StreamingAssets" }
        };

        // 缓存包依赖，不用每次计算
        public static Dictionary<string, string[]> DependenciesCache = new Dictionary<string, string[]>();

        public static string IntToString(this int value)
        {
            string result;
            if (IntToStringDict.TryGetValue(value, out result))
            {
                return result;
            }

            result = value.ToString();
            IntToStringDict[value] = result;
            return result;
        }

        public static string StringToAB(this string value)
        {
            string result;
            if (StringToABDict.TryGetValue(value, out result))
            {
                return result;
            }

            result = value + ".unity3d";
            StringToABDict[value] = result;
            return result;
        }

        public static string IntToAB(this int value)
        {
            return value.IntToString().StringToAB();
        }

        public static string BundleNameToLower(this string value)
        {
            string result;
            if (BundleNameToLowerDict.TryGetValue(value, out result))
            {
                return result;
            }

            result = value.ToLower();
            BundleNameToLowerDict[value] = result;
            return result;
        }

        public static string[] GetDependencies(string assetBundleName)
        {
            string[] dependencies = new string[0];
            if (DependenciesCache.TryGetValue(assetBundleName, out dependencies))
            {
                return dependencies;
            }
            //if (!Define.IsAsync)
#if UNITY_EDITOR && !Async
            dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
#else
            dependencies = ResourcesManager.AssetBundleManifestObject.GetAllDependencies(assetBundleName);
#endif

            return dependencies;
        }

        public static string[] GetSortedDependencies(string assetBundleName)
        {
            Dictionary<string, int> info = new Dictionary<string, int>();
            List<string> parents = new List<string>();
            CollectDependencies(parents, assetBundleName, info);
            string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            return ss;
        }

        public static void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
        {
            parents.Add(assetBundleName);
            string[] deps = GetDependencies(assetBundleName);
            foreach (string parent in parents)
            {
                if (!info.ContainsKey(parent))
                {
                    info[parent] = 0;
                }
                info[parent] += deps.Length;
            }


            foreach (string dep in deps)
            {
                if (parents.Contains(dep))
                {
                    throw new Exception($"包有循环依赖，请重新标记: {assetBundleName} {dep}");
                }
                CollectDependencies(parents, dep, info);
            }
            parents.RemoveAt(parents.Count - 1);
        }
    }


public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ResourcesManager>();
                if (instance == null)
                {
                    GameObject g = new GameObject("ResourcesManager");
                    instance = g.AddComponent<ResourcesManager>();
                }
            }
            return instance;
        }
    }
    private static ResourcesManager instance;

    public static AssetBundleManifest AssetBundleManifestObject { get; set; }

    private readonly Dictionary<string, Dictionary<string, UnityEngine.Object>> resourceCache = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

    private readonly Dictionary<string, ABInfo> bundles = new Dictionary<string, ABInfo>();

    //List<(AssetBundleCreateRequest, TaskCompletionSource<AssetBundle>)> createRequest = new List<(AssetBundleCreateRequest, TaskCompletionSource<AssetBundle>)>();
    Dictionary<string, (AssetBundleCreateRequest, TaskCompletionSource<AssetBundle>)> createRequest = new Dictionary<string, (AssetBundleCreateRequest, TaskCompletionSource<AssetBundle>)>();

    List<(AssetBundleRequest, TaskCompletionSource<bool>)> requests = new List<(AssetBundleRequest, TaskCompletionSource<bool>)>();

    List<ABInfo> aBInfos = new List<ABInfo>();

    private void Awake()
    {
        instance = this;
#if !UNITY_EDITOR || Async
            LoadOneBundle("StreamingAssets");
            AssetBundleManifestObject = (AssetBundleManifest)GetAsset<AssetBundleManifest>("StreamingAssets", "AssetBundleManifest");
#endif
    }

    private void Update()
    {
        foreach (var kv in createRequest.ToArray())
        {
            if (kv.Value.Item1.isDone)
            {
                kv.Value.Item2.SetResult(kv.Value.Item1.assetBundle);
                createRequest.Remove(kv.Key);
            }
        }
        for (int i = requests.Count - 1; i >= 0; i--)
        {
            (AssetBundleRequest, TaskCompletionSource<bool>) kv = requests[i];
            if (kv.Item1.isDone)
            {
                kv.Item2.SetResult(true);
                requests.RemoveAt(i);
            }
        }
    }

    public void OnDestroy()
    {
        foreach (var abInfo in this.bundles)
        {
            abInfo.Value?.AssetBundle?.Unload(true);
        }

        this.bundles.Clear();
        this.resourceCache.Clear();

        //foreach (var ab in aBInfos)
        //{
        //    ab.Dispose();
        //}
    }

    Task<AssetBundle> LoadAsync(string path)
    {
        if (createRequest.TryGetValue(path, out (AssetBundleCreateRequest, TaskCompletionSource<AssetBundle>) value))
        {
            return value.Item2.Task;
        }
        else
        {
            TaskCompletionSource<AssetBundle> tcs = new TaskCompletionSource<AssetBundle>();
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            createRequest.Add(path, (request, tcs));
            return tcs.Task;
        }
    }

    async Task<UnityEngine.Object[]> LoadAllAssetsAsync(AssetBundle assetBundle)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        AssetBundleRequest request = assetBundle.LoadAllAssetsAsync();
        requests.Add((request, tcs));
        await tcs.Task;
        return request.allAssets;
    }

    public T GetAsset<T>(string bundleName, string prefab) where T : UnityEngine.Object
    {
        //Debug.Log("------------------------------------------"+bundleName + "," + prefab);
        Dictionary<string, UnityEngine.Object> dict;
        //if (!this.resourceCache.ContainsKey(bundleName.BundleNameToLower()))
        //{
        //    LoadOneBundle(bundleName.ToLower());
        //}
        if (!this.resourceCache.TryGetValue(bundleName.BundleNameToLower(), out dict))
        {

            throw new Exception($"not found asset: {bundleName} {prefab}");
        }

        UnityEngine.Object resource = null;
        if (!dict.TryGetValue(prefab, out resource))
        {
            throw new Exception($"not found asset: {bundleName} {prefab}");
        }

        return resource as T;
    }

    public void UnloadBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();

        string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);

        //Log.Debug($"-----------dep unload {assetBundleName} dep: {dependencies.ToList().ListToString()}");
        foreach (string dependency in dependencies)
        {
            this.UnloadOneBundle(dependency);
        }
    }

    private void UnloadOneBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();

        ABInfo abInfo;
        if (!this.bundles.TryGetValue(assetBundleName, out abInfo))
        {
            throw new Exception($"not found assetBundle: {assetBundleName}");
        }

        //Log.Debug($"---------- unload one bundle {assetBundleName} refcount: {abInfo.RefCount - 1}");

        --abInfo.RefCount;

        if (abInfo.RefCount > 0)
        {
            return;
        }


        this.bundles.Remove(assetBundleName);
        abInfo.Dispose();
        //Log.Debug($"cache count: {this.cacheDictionary.Count}");
    }

    /// <summary>
    /// 同步加载assetbundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public void LoadBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
        //Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
        foreach (string dependency in dependencies)
        {
            if (string.IsNullOrEmpty(dependency))
            {
                continue;
            }
            this.LoadOneBundle(dependency);
        }
    }

    public void AddResource(string bundleName, string assetName, UnityEngine.Object resource)
    {
        //Debug.Log(bundleName +","+ assetName +","+ resource.name);
        Dictionary<string, UnityEngine.Object> dict;
        if (!this.resourceCache.TryGetValue(bundleName.BundleNameToLower(), out dict))
        {
            dict = new Dictionary<string, UnityEngine.Object>();
            this.resourceCache[bundleName] = dict;
        }

        dict[assetName] = resource;
    }

    public void LoadOneBundle(string assetBundleName)
    {
        //Debug.Log($"---------------load one bundle {assetBundleName}");
        ABInfo abInfo;
        if (this.bundles.TryGetValue(assetBundleName, out abInfo))
        {
            ++abInfo.RefCount;
            return;
        }

        //if (!Define.IsAsync)
        string[] realPath = null;
#if UNITY_EDITOR && !Async
        realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
        foreach (string s in realPath)
        {
            string assetName = Path.GetFileNameWithoutExtension(s);
            UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);//非异步加载太大的东西可能导致卡顿。
            AddResource(assetBundleName, assetName, resource);
            foreach (var r in AssetDatabase.LoadAllAssetRepresentationsAtPath(s))
            {
                AddResource(assetBundleName, assetName, r);
            }
        }

        abInfo = new ABInfo(assetBundleName, null);
        aBInfos.Add(abInfo);
        //abInfo.Parent = this;
        this.bundles[assetBundleName] = abInfo;
#else
            string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
            AssetBundle assetBundle = null;
            Debug.Log(p);
            if (File.Exists(p))
            {
                assetBundle = AssetBundle.LoadFromFile(p);
            }
            else
            {
                p = Path.Combine(PathHelper.AppResPath, assetBundleName);
                assetBundle = AssetBundle.LoadFromFile(p);
            }

            if (assetBundle == null)
            {
                throw new Exception($"assets bundle not found: {assetBundleName}");
            }

            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                // 异步load资源到内存cache住
                UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
                foreach (UnityEngine.Object asset in assets)
                {
                    AddResource(assetBundleName, asset.name, asset);
                }
            }

            abInfo = new ABInfo(assetBundleName, assetBundle);
            aBInfos.Add(abInfo);
            this.bundles[assetBundleName] = abInfo;
#endif
    }

    /// <summary>
    /// 异步加载assetbundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public async Task LoadBundleAsync(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
        // Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
        foreach (string dependency in dependencies)
        {
            if (string.IsNullOrEmpty(dependency))
            {
                continue;
            }
            await this.LoadOneBundleAsync(dependency);
        }
    }

    public async Task LoadOneBundleAsync(string assetBundleName)
    {
        ABInfo abInfo;
        if (this.bundles.TryGetValue(assetBundleName, out abInfo))
        {
            ++abInfo.RefCount;
            return;
        }

        //Log.Debug($"---------------load one bundle {assetBundleName}");
        string[] realPath = null;
#if UNITY_EDITOR && !Async
        realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
        foreach (string s in realPath)
        {
            string assetName = Path.GetFileNameWithoutExtension(s);
            UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
            AddResource(assetBundleName, assetName, resource);
        }

        abInfo = new ABInfo(assetBundleName, null);
        aBInfos.Add(abInfo);
        //abInfo.Parent = this;
        this.bundles[assetBundleName] = abInfo;
        await Task.CompletedTask;
#else
            string p = Path.Combine(PathHelper.AppHotfixResPath, assetBundleName);
            AssetBundle assetBundle = null;
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, assetBundleName);
            }


            assetBundle = await LoadAsync(p);


            if (assetBundle == null)
            {
                throw new Exception($"assets bundle not found: {assetBundleName}");
            }

            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                // 异步load资源到内存cache住
                UnityEngine.Object[] assets;
                assets = await LoadAllAssetsAsync(assetBundle);
                foreach (UnityEngine.Object asset in assets)
                {
                    AddResource(assetBundleName, asset.name, asset);
                }
            }

            abInfo = new ABInfo(assetBundleName, assetBundle);
            aBInfos.Add(abInfo);
            //abInfo.Parent = this;
            this.bundles[assetBundleName] = abInfo;
#endif
    }

    public string DebugString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (ABInfo abInfo in this.bundles.Values)
        {
            sb.Append($"{abInfo.Name}:{abInfo.RefCount}\n");
        }
        return sb.ToString();
    }
}
