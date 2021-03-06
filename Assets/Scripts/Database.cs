using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.AddressableAssets;

public class Database
{
    public static Database Instance => instance == null ?
#if UNITY_EDITOR
        instance = new Database().Init1()
#else
            instance = new Database()
#endif
            : instance;
    private static Database instance;

    Dictionary<Type, IConfig[]> dic = new Dictionary<Type, IConfig[]>();

    public void Clear()
    {
        dic.Clear();
    }

    public async Task Init()
    {
        try
        {
            if (dic.Count > 0) return;
            await Task.WhenAll(
            AddAsync<MapTileData>("MapTileData"),
            AddAsync<CardData>("CardData"),
            AddAsync<UnitData>("UnitData"),
            //AddAsync<MapData>("MapData"),
            AddAsync<SkillData>("SkillData"),
            AddAsync<BuffData>("BuffData"),
            AddAsync<BulletData>("BulletData"),
            //AddAsync<WaveData>("WaveData"),
            AddAsync<ModifyData>("ModifyData"),
            AddAsync<EffectData>("EffectData"),
            AddAsync<RelicData>("RelicData"),
            AddAsync<ContractData>("ContractData"),
            AddAsync<EventData>("EventData"),
            AddAsync<RewardData>("RewardData"),
            AddAsync<DungeonLevelData>("DungeonLevelData"),
            AddAsync<SystemData>("SystemData")
            );
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public Database Init1()
    {
        try
        {
            if (dic.Count > 0) return this;
            Add<MapTileData>("MapTileData");
            Add<CardData>("CardData");
            Add<UnitData>("UnitData");
            //Add<MapData>("MapData");
            Add<SkillData>("SkillData");
            Add<BuffData>("BuffData");
            Add<BulletData>("BulletData");
            //Add<WaveData>("WaveData");
            Add<ModifyData>("ModifyData");
            Add<EffectData>("EffectData");
            Add<RelicData>("RelicData");
            Add<ContractData>("ContractData");
            Add<EventData>("EventData");
            Add<RewardData>("RewardData");
            Add<DungeonLevelData>("DungeonLevelData");
            Add<SystemData>("SystemData");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        return this;
    }

    public T Get<T>(int id) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        if (r == null || id < 0 || id >= r.Length)
        {
            Debug.LogWarning($"cant find {typeof(T).Name} ,id {id}");
            return null;
        }
        return r[id] as T;
    }

    public T Get<T>(string id) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        return r.FirstOrDefault(x => x.Id == id) as T;
    }

    public T Get<T>(Func<T, bool> match) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        return r.FirstOrDefault(x => match(x as T)) as T;
    }

    public T[] GetAll<T>() where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        return r.Select(x => x as T).ToArray();
    }

    public int GetIndex<T>(T t) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        var result = Array.IndexOf(r, t);
        if (result == -1) throw new Exception($"cant find {typeof(T).Name} ,id {t.Id}");
        return result;
    }

    public int GetIndex<T>(string id) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        var result = Array.FindIndex(r, x => x.Id == id);
        if (result == -1) throw new Exception($"cant find {typeof(T).Name} ,id {id}");
        return result;
    }

    private void Add<T>(string name) where T : IConfig
    {
#if UNITY_EDITOR
        var text = SaveHelper.LoadFile("/Data/" + name + ".txt"); 
        if (string.IsNullOrEmpty(text))
        {
            //Debug.Log(name + "load from address");
            text = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(PathHelper.DataPath + name + ".txt").text;
        }
        var arr = text.Split('\n');
        IConfig[] values = new IConfig[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            try
            {
                values[i] = JsonHelper.FromJson<T>(arr[i]);
            }
            catch (Exception e)
            {
                Debug.LogError(typeof(T).Name + "," + (i+1) + "\n" + e.ToString());
            }
        }
        dic.Add(typeof(T), values);
#endif
    }

    private async Task AddAsync<T>(string name) where T : IConfig
    {
        string text;
        text = SaveHelper.LoadFile("/Data/" + name + ".txt");
        if (string.IsNullOrEmpty(text))
        {
            Debug.Log(name + "load from address");
            var operation = Addressables.LoadAssetAsync<TextAsset>(PathHelper.DataPath + name);
            //var text= operation.WaitForCompletion().text;
            await operation.Task;
            text = operation.Result.text;
            Addressables.ReleaseInstance(operation);
        }
        if (string.IsNullOrEmpty(text)) return;
        var arr = text.Split('\n');
        IConfig[] values = new IConfig[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            try
            {
                values[i] = JsonHelper.FromJson<T>(arr[i]);
                if (values[i] == null) Debug.LogError($"{name}\n {arr[i]}");
            }
            catch (Exception e)
            {
                Debug.LogError(arr[i] + "\n" + e.ToString());
            }
        }
        //Debug.Log(Time.time);
        dic.Add(typeof(T), values);
    }
    public Dictionary<string, MapInfo> Maps = new Dictionary<string, MapInfo>();
    public MapInfo GetMap(string mapName)
    {
        if (Maps.TryGetValue(mapName, out MapInfo result))
        {
            return result;
        }
        else
        {
            var str = SaveHelper.LoadBaseFile(mapName);
            if (string.IsNullOrEmpty(str))
                str = SaveHelper.LoadFile("/Map/" + mapName + ".map");
            result = JsonHelper.FromJson<MapInfo>(str);
            Maps.Add(mapName, result);
            return result;
        }
    }

    public List<string> GetMaps()
    {
        var path = PathHelper.AppHotfixResPath + "/Map/";
        if (!Directory.Exists(path)) return new List<string>();
        List<string> files = new List<string>();
        FileHelper.GetAllFiles(files, path);
        files.RemoveAll(x => x.EndsWith(".meta"));
        return files.Select(x => System.IO.Path.GetFileNameWithoutExtension(x)).ToList();
    }
}
