using UnityEngine;

public abstract class ActionSettingsScript : ScriptableObject, IActionTypeHolder
{
    public string effectName;
    public bool isHint;

    public float bpm;
    public float timeStartSeconds;
    public float timeStartBeats;

    public bool isTimeEnd; // Заканчивается ли эффект?
    public float timeEndSeconds; 
    public float timeEndBeats;

    public bool isTimeForcedBreak; 
    public float timeForcedBreakSeconds;
    public float timeForcedBreakBeats;

    public ActionType type;

    public ActionType Type => type;
}