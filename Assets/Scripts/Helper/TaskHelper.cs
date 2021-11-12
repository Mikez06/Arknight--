using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Runtime.CompilerServices;

public static class TaskHelper
{
    public static AsyncOperationAwaiter<T> GetAwaiter<T>(this T operation) where T : AsyncOperation
    {
        return new AsyncOperationAwaiter<T>(operation);
    }
    public struct AsyncOperationAwaiter<T> : INotifyCompletion where T : AsyncOperation
    {
        private T asyncOperation;
        public AsyncOperationAwaiter(T asyncOperation)
        {
            this.asyncOperation = asyncOperation;
        }
        public bool IsCompleted => asyncOperation.isDone;

        public void OnCompleted(Action continuation)
        {
            asyncOperation.completed += (x) => continuation?.Invoke();
        }

        public T GetResult() => asyncOperation;
    }
    public static Task PlayAsync(this Transition self)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        self.Play(() =>
        {
            tcs.SetResult(true);
        });
        return tcs.Task;
    }
    public static Task Wait(this Tween self)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        self.OnComplete(() =>
        {
            tcs.TrySetResult(true);
        });
        self.OnKill(() =>
        {
            tcs.TrySetResult(true);
        });
        return tcs.Task;
    }
}