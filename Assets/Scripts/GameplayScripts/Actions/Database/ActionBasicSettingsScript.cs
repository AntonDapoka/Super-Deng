using UnityEngine;

public abstract class ActionBasicSettingsScript : ScriptableObject, IActionTypeHolder
{
    public ActionType type;

    public ActionType Type => type;
}