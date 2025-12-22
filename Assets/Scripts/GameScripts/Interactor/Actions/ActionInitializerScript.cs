using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionInitializerScript : MonoBehaviour
{
    [SerializeField] private ActionScenarioDataBase scenarioData;
    [SerializeField] private ActionInteractorScript actionInteractor;

    [SerializeField] private MonoBehaviour[] actionObjects;
    private IActionScript[] actions;
    private IActionSettingsScript[] settings;

    private void Awake()
    {
        ConvertActions();
        ConvertSettings();
        SetScenario();
    }

    private void ConvertActions()
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

    private void ConvertSettings()
    {
        var raw = scenarioData.Actions;

        if (raw == null)
        {
            Debug.LogError("scenarioData.Actions == null");
            settings = new IActionSettingsScript[0];
            return;
        }

        settings = raw
            .Select(s =>
            {
                if (s is not IActionSettingsScript setting)
                {
                    Debug.LogError($"ScriptableObject {s.name} does NOT realise IActionSettingsScript");
                    return null;
                }
                return setting;
            })
            .Where(s => s != null)
            .ToArray();
    }

    private void SetScenario()
    {
        actionInteractor.SetScenario(BuildScenario());
    }

    public ScenarioEntry[] BuildScenario()
    {
        settings = scenarioData.Actions;

        Dictionary<ActionType, IActionScript> actionByType =
            actions.ToDictionary(s => s.Type, s => s);

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
}