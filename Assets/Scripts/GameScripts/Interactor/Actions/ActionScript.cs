using UnityEngine;

public abstract class ActionScript : MonoBehaviour
{
    public ActionType Type;

    public abstract void Initialize();

    public abstract void SetSettings(ActionSettingsScript actionSettings);

    public abstract void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings);

    public abstract void Execute();

    public abstract void ForcedBreak();

    public abstract void Cancel();

    public abstract void TurnOn();

    public abstract void TurnOff();
}