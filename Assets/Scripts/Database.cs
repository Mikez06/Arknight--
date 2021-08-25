﻿using System;
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
            AddAsync<UnitData>("UnitData"),
            AddAsync<MapData>("MapData"),
            AddAsync<SkillData>("SkillData"),
            AddAsync<BuffData>("BuffData"),
            AddAsync<BulletData>("BulletData"),
            AddAsync<WaveData>("WaveData"),
            AddAsync<ModifyData>("ModifyData")
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
            Add<UnitData>("UnitData");
            Add<MapData>("MapData");
            Add<SkillData>("SkillData");
            Add<BuffData>("BuffData");
            Add<BulletData>("BulletData");
            Add<WaveData>("WaveData");
            Add<ModifyData>("ModifyData");
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
        var text = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(PathHelper.DataPath + name + ".txt").text;
        var arr = text.Split('\n');
        IConfig[] values = new IConfig[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            values[i] = JsonHelper.FromJson<T>(arr[i]);
        }
        dic.Add(typeof(T), values);
#endif
    }

    private async Task AddAsync<T>(string name) where T : IConfig
    {
        var operation = Addressables.LoadAssetAsync<TextAsset>(PathHelper.DataPath + name);
        //var text= operation.WaitForCompletion().text;
        await operation.Task;
        var text = operation.Result.text;
        var arr = text.Split('\n');
        IConfig[] values = new IConfig[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            try
            {
                values[i] = JsonHelper.FromJson<T>(arr[i]);
                if (values[i] is BuffData buffData && buffData.Id == "恢复")
                {
                    Debug.Log(111);
                }
                if (values[i] == null) throw new Exception();
            }
            catch (Exception e)
            {
                Debug.LogError(arr[i] + "\n" + e.ToString());
            }
        }
        //Debug.Log(Time.time);
        Addressables.ReleaseInstance(operation);
        dic.Add(typeof(T), values);
    }
}
