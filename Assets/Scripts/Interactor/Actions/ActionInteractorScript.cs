using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInteractorScript : MonoBehaviour
{
    public List<ScenarioEntry> entries;
    private float time;

    void Update()
    {
        time += Time.deltaTime;

        foreach (var entry in entries)
        {
            if (time >= entry.definition.startTime && time < entry.definition.endTime)
            {
                entry.action.Execute(entry.definition);
            }
            else if (time >= entry.definition.endTime)
            {
                entry.action.Cancel(entry.definition);
            }
        }
    }
}

public class ScenarioEntry
{
    public IActionScript action;     
    public IActionDefinitionScript definition;        
}
