using UnityEngine;

public abstract class ActionSettingsScript : ScriptableObject, IActionSettingsScript
{
    public string effectName;
    public bool isHint;

    public float bpm;
    public float timeStartSeconds;
    public float timeStartBeats;
    public bool isTimeEnd; // Заканчивается ли эффект?
    public float timeEndSeconds; 
    public float timeEndBeats;

    public ActionType type;

    public ActionType Type => type;

    float IActionSettingsScript.TimeStartSeconds => timeStartSeconds;
    float IActionSettingsScript.TimeEndSeconds => timeEndSeconds;
}