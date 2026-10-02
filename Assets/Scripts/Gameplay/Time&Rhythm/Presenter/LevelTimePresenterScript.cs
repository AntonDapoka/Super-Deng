using System;
using UnityEngine;

public class LevelTimePresenterScript : MonoBehaviour
{
    [SerializeField] private LevelTimeViewScript timeView;

    public void InitializeTime(float timeElapsed, float timeTotal)
    {
        timeView.InitializeTime(ConvertTimeFloatToString(timeElapsed), timeElapsed, timeTotal);
    }

    public void UpdateTime(float timeElapsed)
    {
        timeView.DisplayTime(ConvertTimeFloatToString(timeElapsed), timeElapsed);
    }

    private string ConvertTimeFloatToString(float timeFloat)
    {
        string timeString = TimeSpan.FromSeconds(timeFloat)
            .ToString(TimeSpan.FromSeconds(timeFloat).TotalHours >= 1 ? @"hh\:mm\:ss" : @"mm\:ss");
        return timeString;
    }
}
