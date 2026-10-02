using UnityEngine;

[CreateAssetMenu(fileName = "BonusFaceSettings", menuName = "ScriptableObjects/BonusFaceSettings", order = 7)]
public class BonusFaceSettings : ActionSpawnerSettingsScript
{
    public BonusType typeBonus;

    public bool isLifeDuration; //Or it will be kinda immortal? 
    public float lifeDurationSeconds;
    public float lifeDurationBeats;

    public bool isDeathDuration; //Or it will just disappear without animation? 
    public float deathDurationSeconds;
    public float deathDurationBeats;

    /// <summary>
    /// Don't mess up with the stuff below
    /// </summary>

    public bool isMaterialChange;
    public Material material;

    public bool isBonusSymbolPrefabChange;
    public GameObject bonusSymbolPrefab;
}
