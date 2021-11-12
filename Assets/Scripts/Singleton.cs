using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _quitting;
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(Create);

    private static T Create()
    {
        var instance = FindObjectOfType<T>();

        if (instance != null) return instance;

        var singletonObject = new GameObject();
        instance = singletonObject.AddComponent<T>();
        singletonObject.name = "[" + typeof(T).Name + "] (Singleton)";
        DontDestroyOnLoad(singletonObject);

        return instance;
    }

    public static T Instance
        => _quitting ? null : LazyInstance.Value;

    private void OnApplicationQuit()
    {
        _quitting = true;
    }

    private void OnDestroy()
    {
        _quitting = true;
    }
}