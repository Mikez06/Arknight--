using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private readonly Dictionary<T, Stack<T>> _pool = new Dictionary<T, Stack<T>>();
    private readonly Dictionary<T, T> _live = new Dictionary<T, T>();

    public T Spawn(T original, Vector3 position, Quaternion? rotation)
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
                result.enabled = true;
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
        obj.transform.SetParent(null);
        _pool[_live[obj]].Push(obj);
        _live.Remove(obj);
    }
}