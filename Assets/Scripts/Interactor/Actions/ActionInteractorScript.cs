using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInteractorScript : MonoBehaviour
{
    public ScenarioEntry[] entries;
    private float time;
    private bool[] spawnExecuted;

    private void Awake()
    {
        spawnExecuted = new bool[entries.Length];
    }

    public void SetScenario(ScenarioEntry[] newEntries)
    {
        entries = newEntries;
    }

    private void Update()
    {
        time += Time.deltaTime;

        foreach (var entry in entries)
        {
            if (time >= entry.definition.TimeStartSeconds && time < entry.definition.TimeEndSeconds)
            {
                Debug.Log(entry.definition.TimeStartSeconds.ToString());
                //entry.action.Execute(entry.definition);
            }
            else if (time >= entry.definition.TimeEndSeconds)
            {
                Debug.Log(entry.definition.TimeEndSeconds.ToString());
                //entry.action.Cancel(entry.definition);
            }
        }
    }
}

public class ScenarioEntry
{
    public IActionScript action;     
    public IActionDefinitionScript definition;        
}
