
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Level Settings"
)]
public class LevelSettings : ScriptableObject
{
    [Header("Level")]
    [SerializeField] private int id;
    [SerializeField] private string levelName;

    [Header("Gameplay")]
    [SerializeField] private ActionScenarioDataBase scenarioData;
    [SerializeField] private ActionBasicSettingsDataBase basicSettingsData;

    [Header("Background")]
    [SerializeField] private GameObject background;

    [Header("Music")]
    [SerializeField] private AssetReferenceT<AudioClip> music;

    public int ID => id;
    public string LevelName => levelName;
    public AssetReferenceT<AudioClip> Music => music;
    public ActionScenarioDataBase ScenarioData => scenarioData;
    public ActionBasicSettingsDataBase BasicSettingsData => basicSettingsData;
}