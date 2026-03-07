using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private ActionScript[] actions;

    private ActionScenarioDataBase scenarioData;
    private ActionBasicSettingsDataBase basicSettingsData;

    public void SetActionScenarioDataBase(ActionScenarioDataBase scenario, ActionBasicSettingsDataBase basicSettings) 
    {
        scenarioData = scenario;
        basicSettingsData = basicSettings;

        ApplyScenario();
    }

    private void ApplyScenario()
    {
        Dictionary<ActionType, ActionScript> actionByType = actions.ToDictionary(s => s.Type, s => s);

        foreach (var set in basicSettingsData.BasicSettings)
        {
            if (!actionByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден Settings Type для ActionType {set.Type}");
                continue;
            }
            script.SetBasicSettings(set);
        }

        actionInteractor.SetScenario(BuildScenarioEntries());
    }

    private ScenarioEntry[] BuildScenarioEntries()
    {
        return BuildEntries(scenarioData.Settings,
            (script, set) => new ScenarioEntry
            {
                action = script,
                settings = set
            }
        );
    }

    private TEntry[] BuildEntries<TSettings, TEntry>(TSettings[] settingsArray, System.Func<ActionScript, TSettings, TEntry> entryFactory) where TSettings : IActionTypeHolder
    {
        Dictionary<ActionType, ActionScript> actionByType = actions.ToDictionary(s => s.Type, s => s);

        List<TEntry> result = new();

        foreach (var set in settingsArray)
        {
            if (!actionByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден Settings Type для ActionType {set.Type}");
                continue;
            }

            result.Add(entryFactory(script, set));
        }
        return result.ToArray();
    }
}