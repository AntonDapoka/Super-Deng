using UnityEngine;

public class RedFaceSpawnerInitializer : SpawnerInitializerScript
{
    [SerializeField] private ActionType type;
    [Header("References")] 
    [SerializeField] private RedFaceBasicSettings settingsBasic;
    [SerializeField] private RedFaceSpawnerScript redFaceSpawner;

    public override void Initialize()
    {
        if (settingsBasic == null) return;

        redFaceSpawner.SetBasicSettings(settingsBasic);

        redFaceSpawner.Initialize();
    }
}
