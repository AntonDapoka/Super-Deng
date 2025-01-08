using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RhythmManager : MonoBehaviour
{
    public bool isTurnOn = false;
    public float bpm = 90f;
    public float beatInterval; // Длительность одного такта в секундах
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private Intervals[] intervals;

    private void Awake()
    {
        beatInterval = 60f / bpm;
    }

    private void FixedUpdate()
    {
        if (isTurnOn)
        {
            foreach (Intervals interval in intervals)
            {
                float sampledTime = (musicManager.timeSamples / (musicManager.clip.frequency * interval.GetIntervalLength(bpm)));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }

    public void StartWithSync()
    {
        StartCoroutine(SynchronizeAndTurnOn());
    }

    private IEnumerator SynchronizeAndTurnOn()
    {
        float currentTime = musicManager.time % beatInterval;
        float waitTime = beatInterval - currentTime;
        yield return new WaitForSeconds(waitTime);
        isTurnOn = true;
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            trigger?.Invoke();
        }
    }
}
