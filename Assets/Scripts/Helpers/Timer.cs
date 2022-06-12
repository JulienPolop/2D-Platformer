using UnityEngine;
using System.Collections;
using System;

public class Timer : MonoBehaviour
{
    public string Name = "";
    public float TimeLimit;
    public float TimeCounter;
    public bool TimerIsStopped = false;


    public event EventHandler TimerEndEvent;
    public Action OnEndTimerCallback;

    public Timer()
    {
        TimerIsStopped = true;
        TimeLimit = 0;
        TimeCounter = 0;
    }
    public void SetUpTimer(float timeLmit)
    {
        TimeLimit = timeLmit;
        OnEndTimerCallback = null;
    }
    public void SetUpTimer(float timeLmit, Action onEndTimerCallback)
    {
        TimeLimit = timeLmit;
        OnEndTimerCallback = onEndTimerCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimerIsStopped)
        {
            TimeCounter -= Time.deltaTime;
            if (TimeCounter <= 0)
            {
                OnEndTimer();
                TimeCounter = TimeLimit;
                TimerIsStopped = true;
            }
        }
    }

    public void StartCounter(float timeLimit)
    {
        TimeLimit = timeLimit;
        StartCounter();
    }
    public void StartCounter()
    {
        TimeCounter = TimeLimit;
        TimerIsStopped = false;
    }
    public void StopCounter()
    {
        TimerIsStopped = true;
    }
    public void ContinueCounter()
    {
        TimerIsStopped = false;
    }


    public void OnEndTimer()
    {
        EventHandler handler = TimerEndEvent;
        handler?.Invoke(this, null);

        OnEndTimerCallback?.Invoke();
    }
}
