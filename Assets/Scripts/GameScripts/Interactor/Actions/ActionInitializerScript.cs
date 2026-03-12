using System.Collections.Generic;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private ActionScript[] actions;
    private Dictionary<ActionType, ActionScript> actionsByType;

    private ActionScenarioDataBase scenarioData;
    private ActionBasicSettingsDataBase basicSettingsData;

    private void Awake()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        actionsByType = new Dictionary<ActionType, ActionScript>(actions.Length);

        foreach (var action in actions)
        {
            if (actionsByType.ContainsKey(action.Type))
            {
                Debug.LogError($"Duplicate ActionType: {action.Type}");
                continue;
            }

            actionsByType[action.Type] = action;
        }
    }

    public void SetActionScenarioDataBase(ActionScenarioDataBase scenario, ActionBasicSettingsDataBase basicSettings) 
    {
        scenarioData = scenario;
        basicSettingsData = basicSettings;

        InitializeActions();
        ApplyScenario();
    }

    private void InitializeActions()
    {
        foreach (var action in actions)
        {
            action.Initialize();
        }
    }

    private void ApplyScenario()
    {
        if (scenarioData == null || basicSettingsData == null)
        {
            Debug.LogError("Scenario or BasicSettings not assigned");
            return;
        }

        ApplyBasicSettings();

        actionInteractor.SetScenario(BuildScenarioEntries());
    }

    private void ApplyBasicSettings()
    {
        foreach (var set in basicSettingsData.BasicSettings)
        {
            if (!actionsByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден Settings Type для ActionType {set.Type}");
                continue;
            }
            script.SetBasicSettings(set);
        }
    }

    private ScenarioEntry[] BuildScenarioEntries()
    {
        List<ScenarioEntry> result = new(scenarioData.Settings.Length);

        foreach (var set in scenarioData.Settings)
        {
            if (!actionsByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден Settings Type для ActionType {set.Type}");
                continue;
            }

            result.Add(new ScenarioEntry
            {
                action = script,
                settings = set
            });
        }

        return result.ToArray();
    }
}