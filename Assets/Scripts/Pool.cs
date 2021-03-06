using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pool<T> where T : Component
{
    private readonly Dictionary<T, Stack<T>> _pool = new Dictionary<T, Stack<T>>();
    private readonly Dictionary<T, T> _live = new Dictionary<T, T>();
    public static Transform PoolRoot;


    public T Spawn(T original, Vector3 position, Quaternion? rotation = null)
    {
        T result;
        if (_pool.TryGetValue(original, out var pool))
        {
            if (pool.Count > 0)
            {
                result = pool.Pop();
                if (rotation.HasValue)
                {
                    result.transform.SetPositionAndRotation(position, rotation.Value);
                }
                else
                {
                    result.transform.position = position;
                }

                result.gameObject.SetActive(true);
                //result.transform.enabled = true;
            }
            else
            {
                if (rotation.HasValue)
                {
                    result = Object.Instantiate(original, position, rotation.Value);
                }
                else
                {
                    result = Object.Instantiate(original, position, original.transform.rotation);
                    //result.transform.position = position;
                }
            }
        }
        else
        {
            _pool.Add(original, new Stack<T>());
            if (rotation.HasValue)
            {
                result = Object.Instantiate(original, position, rotation.Value);
            }
            else
            {
                result = Object.Instantiate(original, position, original.transform.rotation);
                //result.transform.position = position;
            }
        }

        _live[result] = original;

        return result;
    }

    public void Despawn(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Init.Instance.transform);
        if (!_live.ContainsKey(obj)) return;
        _pool[_live[obj]].Push(obj);
        _live.Remove(obj);
    }

    public void DespawnAll()
    {
        foreach (var obj in _live.Keys.ToArray())
        {
            if (obj == null) continue;
            Despawn(obj);
        }
        _live.Clear();
    }
}