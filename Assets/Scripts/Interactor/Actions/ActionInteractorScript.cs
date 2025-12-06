using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInteractorScript : MonoBehaviour
{
    public ScenarioEntry[] entries;
    private float time;
    private bool[] spawnExecuted;
    private bool[] spawnCanceled;
    public int currentSpawnIndex = 0;


    private void Awake()
    {
        spawnExecuted = new bool[entries.Length];
        spawnCanceled = new bool[entries.Length];
    }

    public void SetScenario(ScenarioEntry[] newEntries)
    {
        entries = newEntries;
    }

    private void Update()
    {
        time += Time.deltaTime;

        for (int i = 0; i < entries.Length; i++)
        {
            if (time >= entries[i].settings.TimeStartSeconds && time < entries[i].settings.TimeEndSeconds && !spawnExecuted[currentSpawnIndex])
            {
                Debug.Log(entries[i].settings.TimeStartSeconds.ToString());
                entries[i].action.SetSettings(entries[i].settings);
                spawnExecuted[currentSpawnIndex] = true;
            }
            else if (time >= entries[i].settings.TimeEndSeconds && !spawnCanceled[currentSpawnIndex])
            {
                Debug.Log(entries[i].settings.TimeEndSeconds.ToString());
                //entry.action.Cancel(entry.definition);
                spawnCanceled[currentSpawnIndex] = true;
            }

            if (i + 1 < entries.Length && time > entries[i + 1].settings.TimeStartSeconds)
            {
                currentSpawnIndex++;
            }
        }
    }
}

public class ScenarioEntry
{
    public IActionScript action;     
    public IActionSettingsScript settings;        
}
