using UnityEngine;

public class LevelInitializerScript : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private ActionScenarioDataBase scenarioData; 
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;

    [Header("Controller Data")]
    [SerializeField] private KeyBindingDataScript playerMovementKeyBindingData;

    [Header("Controllers")]
    [SerializeField] private PlayerMovementControllerScript playerMovementController;

    [Header("Presenters")]
    [SerializeField] private PlayerMovementKeyBindingHintsPresenterScript playerMovementKeyBindingHintsPresenter;

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

        playerMovementController.SetKeyBindings(playerMovementKeyBindingData);
        playerMovementKeyBindingHintsPresenter.SetKeyBindings(playerMovementKeyBindingData);
        playerMovementKeyBindingHintsPresenter.TurnOn();
        playerMovementKeyBindingHintsPresenter.Initialize();
        fieldInitializer.InitializeField();
        LevelTimeManagement.InitializeTime(0f, musicTrack.length);
        rhythmManager.StartWithSync();
        actionInitializer.SetActionScenarioDataBase(scenarioData, basicSettingsData);
        cameraBehaivorInteractor.InitializeCamera(rhythmManager.GetBeatInterval());
    }

    private void Start()
    {
        timeIntializer.TurnOn();

        playerInitializer.InitializePlayer(rhythmManager.GetBeatInterval());
        startCountDownInteractor.StartStartCountDown(rhythmManager.GetBeatInterval());
    }
}
