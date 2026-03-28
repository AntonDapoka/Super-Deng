using System.Collections;
using UnityEngine;

public class RhythmManager : MonoBehaviour, IRhythmableScript
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
        beatInterval = 60f / bpm;
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
         * �� ������ ������ FixedUpdate ����������� ������� ��������� ����� (timeSamples) � �����������, ����� �������� ������ ������� (sampledTime).
         * ��� ������� ��������� ���������� CheckForNewInterval, ��� ������������ ����� �������� � ���������� (lastInterval).
         * ���� �������� ���������, ���������� ����������� �������.
         * */
        if (isTurnOn)
        {
            foreach (Intervals interval in intervals) 
            {
                float sampledTime = musicManager.timeSamples / (musicManager.clip.frequency * interval.GetIntervalLength(bpm));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }

    public void AddNewInterval()
    {
        
    }
} 
