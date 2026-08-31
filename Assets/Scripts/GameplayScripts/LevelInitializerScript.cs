using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelInitializerScript : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private LevelSettings levelSettings;/*
    [SerializeField] private ActionScenarioDataBase scenarioData; 
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;*/

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
    private AsyncOperationHandle<AudioClip> musicHandle;
    private AudioClip musicTrack;
    private bool musicLoaded;

    [Header("Other References")]
    [SerializeField] private StartCountDownInteractorScript startCountDownInteractor;
    [SerializeField] private CameraBehaivorInteractorScript cameraBehaivorInteractor;

    private async void Awake()
    {
        fieldInitializer.InitializeField();

        await LoadLevel(levelSettings);
        InitializeMusic();

        timeIntializer.InitializeTime(0f, musicTrack.length);
        rhythmManager.StartWithSync();
        actionInitializer.SetActionScenarioDataBase(levelSettings.ScenarioData, levelSettings.BasicSettingsData);
        cameraBehaivorInteractor.InitializeCamera(rhythmManager.GetBeatInterval());
    }

    public async Task LoadLevel(LevelSettings settings)
    {
        if (settings == null)
        {
            Debug.LogError("LevelData is null.");
            return;
        }

        levelSettings = settings;
        await LoadMusic(settings);
    }

    private async Task LoadMusic(LevelSettings settings)
    {
        if (settings.Music == null || !settings.Music.RuntimeKeyIsValid())
        {
            Debug.LogWarning($"Level '{settings.LevelName}' doesn't have music track.");
            return;
        }

        musicHandle = settings.Music.LoadAssetAsync<AudioClip>();
        await musicHandle.Task;

        if (musicHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load music for '{settings.LevelName}'.");
            return;
        }

        musicTrack = musicHandle.Result;
        musicLoaded = true;
        Debug.Log($"Loaded '{musicTrack.name}' for '{settings.LevelName}'.");
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

    public void UnloadLevel()
    {
        if (musicLoaded)
        {
            Addressables.Release(musicHandle);

            musicLoaded = false;
            musicHandle = default;
        }
    }

    private void OnDestroy()
    {
        UnloadLevel();
    }
}
