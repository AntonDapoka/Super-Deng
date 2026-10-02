using UnityEngine;

[CreateAssetMenu(fileName = "ActionBasicSettingsDataBase", menuName = "ScriptableObjects/ActionBasicSettingsDataBase", order = 1)]
public class ActionBasicSettingsDataBase : ScriptableObject
{
    [SerializeReference]
    private ActionBasicSettingsScript[] basicSettings;

    public ActionBasicSettingsScript[] BasicSettings => basicSettings;
}
