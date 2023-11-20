using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReloadTimer 
{
    private static Dictionary<string, float> timers = new Dictionary<string, float>();

    public static void StartTimer(string timerId, float duration)
    {
        timers[timerId] = duration;
    }

    public static bool IsTimerDone(string timerId)
    {
        if (timers.TryGetValue(timerId, out float timeLeft))
        {
            return timeLeft <= 0;
        }

        return false; // Timer not found
    }

    public static void Update()
    {
        List<string> keys = new List<string>(timers.Keys);
        foreach (var key in keys)
        {
            timers[key] -= Time.deltaTime;
        }
    }

    public static void StopTimer(string TimerID)
    {
        timers.Remove(TimerID);
    }
}
