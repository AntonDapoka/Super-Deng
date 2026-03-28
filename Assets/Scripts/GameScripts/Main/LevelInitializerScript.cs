using UnityEngine;

public class LevelInitializerScript : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private ActionScenarioDataBase scenarioData; 
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;

    [Header("Script References")]
    [SerializeField] private LevelRhythmManagementScript rhythmManager;
    [SerializeField] private LevelTimeManagementScript LevelTimeManagement;
    [SerializeField] private FieldInitializerScript fieldInitializer;
    [SerializeField] private BackgroundInitializerScript backgroundInitializer;
    [SerializeField] private PlayerInitializerScript playerInitializer;
    [SerializeField] private ActionInitializerScript actionInitializer;
    [SerializeField] private LevelTimeManagementScript timeIntializer;
    [SerializeField] private StartCountDownInteractorScript startCountDownInteractor;
    [SerializeField] private PlayerBeatSyncValidatorScript playerBeatSyncValidator;
    [SerializeField] private CameraBehaivorInteractorScript cameraBehaivorInteractor;

    [Header("Other References")]
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;

    private void Awake()
    {
        musicManager.clip = musicTrack;
        musicManager.Play();

        fieldInitializer.InitializeField();
        LevelTimeManagement.InitializeTime(0f, musicTrack.length);
        rhythmManager.StartWithSync();
        actionInitializer.SetActionScenarioDataBase(scenarioData, basicSettingsData);
        cameraBehaivorInteractor.InitializeCamera(rhythmManager.GetBeatInterval());
    }

    private void Start()
    {
        timeIntializer.TurnOn();

        playerInitializer.InitializePlayer();
        playerBeatSyncValidator.Initialize(rhythmManager.GetBeatInterval()); //Add data
        startCountDownInteractor.StartStartCountDown(rhythmManager.GetBeatInterval());
    }
}
