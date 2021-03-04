using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
    public static TimeHelper Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("TimeHelper");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<TimeHelper>();
            }
            return instance;
        }
    }
    private static TimeHelper instance;
    private class Timer
    {
        public float Time;
        public TaskCompletionSource<bool> tcs;
    }

    List<Timer> timers = new List<Timer>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        while (timers.Count > 0 && timers[0].Time < Time.time)
        {
            timers[0].tcs.TrySetResult(true);
            timers.RemoveAt(0);
        }
    }

    public Task WaitAsync(float time, CancellationToken cancellationToken)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        Timer timer = new Timer() { tcs = tcs, Time = Time.time + time };
        addNewTimer(timer);
        cancellationToken.Register(() => { timers.Remove(timer); });
        return tcs.Task;
    }


    public Task WaitAsync(float time)
    {
        if (time == 0) return Task.CompletedTask;
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        Timer timer = new Timer() { tcs = tcs, Time = Time.time + time };
        addNewTimer(timer);
        return tcs.Task;
    }

    void addNewTimer(Timer timer)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (timer.Time < timers[i].Time)
            {
                timers.Insert(i, timer);
                return;
            }
        }
        timers.Add(timer);
    }
}
