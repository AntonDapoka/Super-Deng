
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Level Settings"
)]
public class LevelSettings : ScriptableObject
{
    [Header("Level")]
    [SerializeField] private byte idLevel;
    [SerializeField] private string levelName;

    [Header("Gameplay")]
    [SerializeField] private ushort idFaceStart;
    [SerializeField] private ActionScenarioDataBase scenarioData;
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;
    [SerializeField] private FieldType fieldType;

    [Header("Background")]
    [SerializeField] private GameObject background;

    [Header("Music")]
    [SerializeField] private AssetReferenceT<AudioClip> music;

    public byte IDLevel => idLevel;
    public string LevelName => levelName;
    public ushort IDFaceStart => idFaceStart;
    public AssetReferenceT<AudioClip> Music => music;
    public ActionScenarioDataBase ScenarioData => scenarioData;
    public ActionBasicSettingsDataBase BasicSettingsData => basicSettingsData;
}