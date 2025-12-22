using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelRhythmManagementScript : MonoBehaviour, IRhythmableScript
{
    public bool isTurnOn = false;
    public float bpm = 90f;
    public float beatInterval;
    public int currentBeat = 0;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private Intervals[] intervals;

    public float CurrentBeat => currentBeat;

    private void Awake()
    {
        beatInterval = 60f / bpm;  //Вычисляем длительность одного такта в секундах
    }

    public float GetBPM()
    {
        return bpm;
    }

    public void SetBPM(float newBPM)
    {
        bpm = newBPM;
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
        currentBeat = Mathf.FloorToInt((musicManager.timeSamples / (float)musicManager.clip.frequency) / (60f / bpm));
        /*
         * На каждом вызове FixedUpdate проверяется текущее положение аудио (timeSamples) и вычисляется, какой интервал сейчас активен (sampledTime).
         * Для каждого интервала вызывается CheckForNewInterval, где сравнивается новый интервал с предыдущим (lastInterval).
         * Если интервал изменился, вызывается привязанное событие.
         * */
        if (isTurnOn)
        {
            foreach (Intervals interval in intervals)
            {
                float sampledTime = (musicManager.timeSamples / (musicManager.clip.frequency * interval.GetIntervalLength(bpm)));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps; //Сколько раз за бит нужно вызвать функцию
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
