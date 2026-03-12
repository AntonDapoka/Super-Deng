using UnityEngine;

public class LevelInitializerScript : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private ActionScenarioDataBase scenarioData; 
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;

    [Header("Script References")]
    [SerializeField] private FieldInitializerScript fieldInitializer;
    [SerializeField] private BackgroundInitializerScript backgroundInitializer;
    [SerializeField] private PlayerInitializerScript playerInitializer;
    [SerializeField] private ActionInitializerScript actionInitializer;
    [SerializeField] private LevelTimeManagementScript timeIntializer;

    [Header("Other References")]
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;

    private void Awake()
    {
        fieldInitializer.InitializeField();
        actionInitializer.SetActionScenarioDataBase(scenarioData, basicSettingsData);
    }

    private void Start()
    {
        timeIntializer.TurnOn();

        playerInitializer.InitializePlayer();
    }
}
