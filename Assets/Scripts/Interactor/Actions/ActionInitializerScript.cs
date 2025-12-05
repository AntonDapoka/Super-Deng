using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private RedFaceSpawnerScript redFaceSpawner;
    [SerializeField] private RedFaceSettings redFaceSettings;
    private List<ScenarioEntry> entries;

    private void Awake()
    {
        //There we should read files
        entries = new List<ScenarioEntry>
        {
            new ScenarioEntry
            {
                action = redFaceSpawner,
                definition = redFaceSettings
            }
        };
        SetScenario();
    }

    private void SetScenario()
    {
        actionInteractor.SetScenario(entries.ToArray());
    }

}
