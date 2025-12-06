using UnityEngine;

public abstract class ActionSettingsScript : ScriptableObject, IActionSettingsScript
{
    public float timeStartSeconds;
    public float timeEndSeconds;

    float IActionSettingsScript.TimeStartSeconds => timeStartSeconds;
    float IActionSettingsScript.TimeEndSeconds => timeEndSeconds;
}