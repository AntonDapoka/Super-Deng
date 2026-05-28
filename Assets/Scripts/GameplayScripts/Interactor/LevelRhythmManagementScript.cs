using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelRhythmManagementScript : MonoBehaviour, IRhythmableScript
{
    public bool isTurnOn = false;
    [SerializeField] private float bpm = 90f;
    [SerializeField] private float beatInterval;
    [SerializeField] private int currentBeat = 0;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private List<Intervals> intervals = new List<Intervals>();

    public float CurrentBeat => currentBeat;

    private void Awake()
    {
        beatInterval = 60f / bpm;
    }

    public float GetBeatInterval()
    {
        return beatInterval;
    }

    public float GetBPM()
    {
        return bpm;
    }

    public void SetBPM(float newBPM)
    {
        bpm = newBPM;
        beatInterval = 60f / bpm;
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
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

    private void FixedUpdate()
    {
        if (!isTurnOn) return;

        currentBeat = Mathf.FloorToInt(musicManager.timeSamples / (float)musicManager.clip.frequency / (60f / bpm));
        foreach (Intervals interval in intervals)
        {
            float sampledTime = musicManager.timeSamples / (musicManager.clip.frequency * interval.GetIntervalLength(bpm));
            interval.CheckForNewInterval(sampledTime);
        }
    }

    public void AddNewIntervalByStep(float step, UnityAction action)
    {
        foreach (var interval in intervals)
        {
            if (interval.MatchesStep(step))
            {
                interval.AddListener(action);
                return;
            }
        }

        Intervals newInterval = new(step);
        newInterval.AddListener(action);
        intervals.Add(newInterval);
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    public Intervals(float steps)
    {
        this.steps = steps;
        trigger = new UnityEvent();
    }

    public bool MatchesStep(float step)
    {
        return Mathf.Approximately(steps, step);
    }

    public void AddListener(UnityAction action)
    {
        if (trigger == null)
            trigger = new UnityEvent();

        trigger.AddListener(action);
    }

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