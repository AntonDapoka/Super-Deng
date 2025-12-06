using UnityEngine;

[CreateAssetMenu(fileName = "ActionScenarioDataBase", menuName = "ScriptableObjects/ActionScenarioDataBase", order = 0)]
public class ActionScenarioDataBase : ScriptableObject
{
    [SerializeReference]
    private ActionSettingsScript[] actions;

    public ActionSettingsScript[] Actions => actions;
}