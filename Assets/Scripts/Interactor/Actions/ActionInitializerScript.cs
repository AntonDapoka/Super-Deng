using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [SerializeField] private ActionScenarioDataBase scenarioData;
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private RedFaceSpawnerScript redFaceSpawner;
    private List<ScenarioEntry> entries;

    private void Awake()
    {
        //There we should read files
        entries = new List<ScenarioEntry>
        {
            new ScenarioEntry
            {
                action = redFaceSpawner,
                settings = scenarioData.Actions[0]
            }
        };
        SetScenario();
    }

    private void SetScenario()
    {
        actionInteractor.SetScenario(entries.ToArray());
    }
    
}
