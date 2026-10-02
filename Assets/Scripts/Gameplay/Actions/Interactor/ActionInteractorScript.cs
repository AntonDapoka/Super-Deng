using UnityEngine;

public class ActionInteractorScript : MonoBehaviour
{
    [SerializeField] private LevelTimeManagementScript levelTimeManagement;

    private ScenarioEntry[] entries;
    private bool[] spawnExecuted;
    private bool[] spawnCanceled;
    private float time;

    private bool isTurnOn = false;

    public void SetScenario(ScenarioEntry[] newEntries)
    {
        entries = newEntries;
        spawnExecuted = new bool[entries.Length];
        spawnCanceled = new bool[entries.Length];
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }

    private void FixedUpdate()
    {
        if (!isTurnOn || entries == null) return;

        time = levelTimeManagement.GetCurrentTime();

        for (int i = 0; i < entries.Length; i++)
        {
            ScenarioEntry entry = entries[i];

            if (!spawnExecuted[i] &&
                time >= entry.settings.timeStartSeconds && 
                (!entry.settings.isTimeEnd ||
                time < entry.settings.timeEndSeconds) &&
                (!entry.settings.isTimeForcedBreak ||
                time < entry.settings.timeForcedBreakSeconds))
            {
                Debug.Log($"Start action {i} at {time}");

                entry.action.SetSettings(entry.settings);
                entry.action.TurnOn();

                spawnExecuted[i] = true;
            }

            if (!spawnCanceled[i] &&
                entry.settings.isTimeEnd &&
                time >= entry.settings.timeEndSeconds)
            {
                Debug.Log($"Cancel action {i} at {time}");

                entry.action.Cancel();
                spawnCanceled[i] = true;
            }

            if (!spawnCanceled[i] &&
                entry.settings.isTimeForcedBreak &&
                time >= entry.settings.timeForcedBreakSeconds)
            {
                Debug.Log($"Forced Break action {i} at {time}");

                entry.action.ForcedBreak();
                spawnCanceled[i] = true;
            }
        }
    }
}