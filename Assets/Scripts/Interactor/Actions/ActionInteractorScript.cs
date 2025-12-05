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
            if (time >= entries[i].definition.TimeStartSeconds && time < entries[i].definition.TimeEndSeconds && !spawnExecuted[currentSpawnIndex])
            {
                Debug.Log(entries[i].definition.TimeStartSeconds.ToString());
                //entry.action.Execute(entry.definition);
                spawnExecuted[currentSpawnIndex] = true;
            }
            else if (time >= entries[i].definition.TimeEndSeconds && !spawnCanceled[currentSpawnIndex])
            {
                Debug.Log(entries[i].definition.TimeEndSeconds.ToString());
                //entry.action.Cancel(entry.definition);
                spawnCanceled[currentSpawnIndex] = true;
            }

            if (i + 1 < entries.Length && time > entries[i + 1].definition.TimeStartSeconds)
            {
                currentSpawnIndex++;
            }
        }
    }
}

public class ScenarioEntry
{
    public IActionScript action;     
    public IActionDefinitionScript definition;        
}
