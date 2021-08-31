using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

//TODO 资源池
public class ResHelper
{
    public static T GetAsset<T>(string path)
    {
        var op = Addressables.LoadAssetAsync<T>(path);
        op.WaitForCompletion();
        if (op.IsDone)
        {
            var result = op.Result;
            //Addressables.Release(op);
            return result;
        }
        else
        {
            throw new Exception();
        }
    }

    public static async Task<T> GetAssetAsync<T>(string path)
    {
        var op = Addressables.LoadAssetAsync<T>(path);
        await op.Task;
        return op.Task.Result;
    }

    public static GameObject Instantiate(string path)
    {
        var op = Addressables.InstantiateAsync(path);
        op.WaitForCompletion();
        return op.Result;
    }

    public static async Task<GameObject> InstantiateAsyncImmediate(string path)
    {
        //TaskCompletionSource<GameObject> tcs = new TaskCompletionSource<GameObject>();
        var op = Addressables.LoadAssetAsync<GameObject>(path);
        var g = await op.Task;
        var r = await Addressables.InstantiateAsync(path).Task;
        //Addressables.InstantiateAsync(path).Completed += (x) =>
        //{
        //    tcs.SetResult(x.Result);
        //};
        //var g= await tcs.Task;
        return r;
    }

    public static void Return(GameObject go)
    {
        Addressables.ReleaseInstance(go);
    }
    public static void Return<T>(T go)
    {
        Addressables.Release(go);
    }
}

