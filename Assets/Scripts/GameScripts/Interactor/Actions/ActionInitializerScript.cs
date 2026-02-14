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
        settings = scenarioData.Settings;

        Dictionary<ActionType, ActionScript> actionByType = actions.ToDictionary(s => s.Type, s => s);

        List<ScenarioEntry> result = new();

        foreach (var set in settings)
        {
            if (!actionByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден IActionScript для ActionType {set.Type}");
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

    private BasicSettingsEntry[] BuildBasicSettingsEntries()
    {
        settingsBasic = basicSettingsData.BasicSettings;

        Dictionary<ActionType, ActionScript> actionByType = actions.ToDictionary(s => s.Type, s => s);

        List<BasicSettingsEntry> result = new();

        foreach (var set in settingsBasic)
        {
            if (!actionByType.TryGetValue(set.Type, out var script))
            {
                Debug.LogError($"Не найден IActionScript для ActionType {set.Type}");
                continue;
            }

            result.Add(new BasicSettingsEntry
            {
                action = script,
                settingsBasic = set
            });
        }

        return result.ToArray();
    }
}

public class BasicSettingsEntry
{
    public ActionScript action;
    public IActionBasicSettingsScript settingsBasic;
}
