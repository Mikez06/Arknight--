using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class Database
{
    public static Database Instance => instance == null ?
#if UNITY_EDITOR
            instance = new Database().Init()
#else
            instance = new Database()
#endif
            : instance;
    private static Database instance;

    Dictionary<Type, IConfig[]> dic = new Dictionary<Type, IConfig[]>();

    public async Task InitAsync()
    {
        try
        {
            await ResourcesManager.Instance.LoadBundleAsync(PathHelper.DataPath);
            if (dic.Count > 0) return;
            Add<UnitConfig>("UnitConfig");
            Add<SkillConfig>("SkillConfig");
            Add<BulletConfig>("BulletConfig");
            Add<MapConfig>("MapConfig");
            Add<WaveConfig>("WaveConfig");
            Add<BuffConfig>("BuffConfig");
            ResourcesManager.Instance.UnloadBundle(PathHelper.DataPath);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public Database Init()
    {
        if (dic.Count > 0) return this;
        ResourcesManager.Instance.LoadBundle(PathHelper.DataPath);
        Add<UnitConfig>("UnitConfig");
        Add<SkillConfig>("SkillConfig");
        Add<BulletConfig>("BulletConfig");
        Add<MapConfig>("MapConfig");
        Add<BuffConfig>("BuffConfig");
        Add<WaveConfig>("WaveConfig");
        ResourcesManager.Instance.UnloadBundle(PathHelper.DataPath);
        return this;
    }


    public T Get<T>(int id) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        if (r == null) throw new Exception();
        return r[id] as T;
    }

    public T Get<T>(string id) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        return r.FirstOrDefault(x => x._Id == id) as T;
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
        return Array.IndexOf(r, t);
    }

    public int GetIndex<T>(string id) where T : class, IConfig
    {
        dic.TryGetValue(typeof(T), out IConfig[] r);
        return Array.FindIndex(r, x => x._Id == id);
    }

    private void Add<T>(string name) where T : IConfig
    {
        var text = ResourcesManager.Instance.GetAsset<TextAsset>(PathHelper.DataPath, name).text;
        T[] values = MongoHelper.FromJson<T[]>(text);
        //foreach (var v in values)
        //{
        //    Debug.Log(MongoHelper.ToJson(v));
        //}
        dic.Add(typeof(T), values.Select(x => x as IConfig).ToArray());
    }
}
