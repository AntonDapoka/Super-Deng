using UnityEngine;

public class LevelInitializerScript : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private ActionScenarioDataBase scenarioData; 
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;

    [Header("Controller Data")]
    [SerializeField] private MovementKeyBindingDataScript playerMovementKeyBindingData;
    [SerializeField] private AbilityKeyBindingDataScript playerAbilityKeyBindingData;

    [Header("Initializers")]
    [SerializeField] private FieldInitializerScript fieldInitializer;
    [SerializeField] private BackgroundInitializerScript backgroundInitializer;
    [SerializeField] private PlayerInitializerScript playerInitializer;
    [SerializeField] private ActionInitializerScript actionInitializer;
    [SerializeField] private LevelTimeManagementScript timeIntializer;

    [Header("Script References")]
    [SerializeField] private LevelRhythmManagementScript rhythmManager;
    [SerializeField] private LevelTimeManagementScript LevelTimeManagement;
    [SerializeField] private StartCountDownInteractorScript startCountDownInteractor;
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

        playerInitializer.InitializePlayer(playerMovementKeyBindingData, playerAbilityKeyBindingData, rhythmManager.GetBeatInterval());
        startCountDownInteractor.StartStartCountDown(rhythmManager.GetBeatInterval());
    }
}
