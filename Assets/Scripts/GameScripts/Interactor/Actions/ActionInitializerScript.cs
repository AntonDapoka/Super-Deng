using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private ActionScript[] actions;

    private ActionSettingsScript[] settings;
    private ActionBasicSettingsScript[] settingsBasic;

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
        actionInteractor.SetBasicSettings(BuildBasicSettingsEntries());
        actionInteractor.SetScenario(BuildScenarioEntries());
    }

    private ScenarioEntry[] BuildScenarioEntries()
    {
        return BuildEntries(
            scenarioData.Settings,
            (script, set) => new ScenarioEntry
            {
                action = script,
                settings = set
            }
        );
    }

    private BasicSettingsEntry[] BuildBasicSettingsEntries()
    {
        return BuildEntries(
            basicSettingsData.BasicSettings,
            (script, set) => new BasicSettingsEntry
            {
                action = script,
                settingsBasic = set
            }
        );
    }

    private TEntry[] BuildEntries<TSettings, TEntry>(TSettings[] settingsArray, System.Func<ActionScript, TSettings, TEntry> entryFactory) where TSettings : IActionTypeHolder
    {
        Dictionary<ActionType, ActionScript> actionByType = actions.ToDictionary(s => s.type, s => s);

        List<TEntry> result = new();

        foreach (var set in settingsArray)
        {
            if (!actionByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден ___ для ActionType {set.Type}");
                continue;
            }

            result.Add(entryFactory(script, set));
        }

        return result.ToArray();
    }
}

public class BasicSettingsEntry
{
    public ActionScript action;
    public ActionBasicSettingsScript settingsBasic;
}
