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

    public float TimeScale;
    public bool FastSpeed;
    public bool Pause;
    
    private void Awake()
    {
        instance = this;
        TimeScale = Time.timeScale;
    }

    private void Update()
    {
        while (timers.Count > 0 && timers[0].Time < Time.time)
        {
            timers[0].tcs.TrySetResult(true);
            timers.RemoveAt(0);
        }
    }

    public void SetGameSpeed(float rate)
    {
        TimeScale = rate;
        calGameSpeed();
    }

    public void SetFastSpeed(bool bo)
    {
        FastSpeed = bo;
        calGameSpeed();
    }
    public void SetPause(bool bo)
    {
        Pause = bo; 
        calGameSpeed();
    }

    protected void calGameSpeed()
    {
        Time.timeScale = TimeScale * (TimeScale == 1 && FastSpeed ? 2 : 1) * (Pause ? 0 : 1);
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
