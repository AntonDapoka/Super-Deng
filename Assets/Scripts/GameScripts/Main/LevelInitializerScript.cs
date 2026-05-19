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

    [Header("Music References")]
    [SerializeField] private LevelRhythmManagementScript rhythmManager;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;

    [Header("Other References")]
    [SerializeField] private StartCountDownInteractorScript startCountDownInteractor;
    [SerializeField] private CameraBehaivorInteractorScript cameraBehaivorInteractor;

    private void Awake()
    {
        InitializeMusic();

        fieldInitializer.InitializeField();
        timeIntializer.InitializeTime(0f, musicTrack.length);

        rhythmManager.StartWithSync();
        
        actionInitializer.SetActionScenarioDataBase(scenarioData, basicSettingsData);
        cameraBehaivorInteractor.InitializeCamera(rhythmManager.GetBeatInterval());
    }

    private void InitializeMusic()
    {
        musicManager.clip = musicTrack;
        musicManager.Play();
    }

    private void Start()
    {
        timeIntializer.TurnOn();

        playerInitializer.InitializePlayer(playerMovementKeyBindingData, playerAbilityKeyBindingData, rhythmManager.GetBeatInterval());
        startCountDownInteractor.StartStartCountDown(rhythmManager.GetBeatInterval());
    }
}
