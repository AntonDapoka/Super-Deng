using UnityEngine;

public abstract class ActionBasicSettingsScript : ScriptableObject, IActionBasicSettingsScript
{
    public ActionType type;

    public ActionType Type => type;
}