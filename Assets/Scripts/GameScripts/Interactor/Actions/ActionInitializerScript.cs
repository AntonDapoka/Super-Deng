using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ActionInteractorScript actionInteractor;
    [SerializeField] private ActionScript[] actionObjects;

    private IActionScript[] actions;
    private IActionSettingsScript[] settings;
    private IActionBasicSettingsScript[] settingsBasic;

    private ActionScenarioDataBase scenarioData;
    private ActionBasicSettingsDataBase basicSettingsData;


    public void SetActionScenarioDataBase(ActionScenarioDataBase scenario, ActionBasicSettingsDataBase basicSettings) 
    {
        scenarioData = scenario;
        basicSettingsData = basicSettings;

        ConvertActions();

        ApplyScenario();
    }

    private void ConvertActions() //Why?
    {
        actions = actionObjects
            .Select(s =>
            {
                if (s is not IActionScript action)
                {
                    Debug.LogError($"Объект {s.name} не реализует IActionScript");
                    return null;
                }
                return action;
            })
            .Where(a => a != null)
            .ToArray();
    }

    private void ApplyScenario()
    {
        actionInteractor.SetBasicSettings(BuildBasicSettingsEntries());
        actionInteractor.SetScenario(BuildScenarioEntries());
    }

    private ScenarioEntry[] BuildScenarioEntries()
    {
        settings = scenarioData.Settings;

        Dictionary<ActionType, IActionScript> actionByType = actions.ToDictionary(s => s.Type, s => s);

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

        Dictionary<ActionType, IActionScript> actionByType = actions.ToDictionary(s => s.Type, s => s);

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
    public IActionScript action;
    public IActionBasicSettingsScript settingsBasic;
}
