using UnityEngine;

public class ActionInteractorScript : MonoBehaviour
{
    [SerializeField] private LevelTimeManagementScript levelTimeManagement;

    private ScenarioEntry[] entries;
    private bool[] spawnExecuted;
    private bool[] spawnCanceled;
    private float time;
    private int currentSpawnIndex = 0;

    public void SetScenario(ScenarioEntry[] newEntries)
    {
        entries = newEntries;
        spawnExecuted = new bool[entries.Length];
        spawnCanceled = new bool[entries.Length];
    }

    private void FixedUpdate()
    {
        time = levelTimeManagement.GetCurrentTime();

        for (int i = 0; i < entries.Length; i++)
        {
            if (time >= entries[i].settings.timeStartSeconds && time < entries[i].settings.timeEndSeconds && !spawnExecuted[currentSpawnIndex])
            {
                Debug.Log(entries[i].settings.timeStartSeconds.ToString());
                entries[i].action.TurnOn();
                entries[i].action.SetSettings(entries[i].settings);
                spawnExecuted[currentSpawnIndex] = true;
            }
            else if (time >= entries[i].settings.timeEndSeconds && !spawnCanceled[currentSpawnIndex])
            {
                Debug.Log(entries[i].settings.timeEndSeconds.ToString());
                entries[i].action.Cancel();
                spawnCanceled[currentSpawnIndex] = true;
            }

            if (i + 1 < entries.Length && time > entries[i + 1].settings.timeStartSeconds)
            {
                currentSpawnIndex++;
            }
        }
    }
}