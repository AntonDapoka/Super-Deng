using UnityEngine;

[CreateAssetMenu(fileName = "BonusSettings", menuName = "ScriptableObjects/BonusSettings", order = 7)]
public class BonusSettings : ActionSpawnerSettingsScript
{
    public BonusType typeBonus;

    public bool isLifeDuration; //Or it will be kinda immortal? 
    public float lifeDurationSeconds;
    public float lifeDurationBeats;

    /// <summary>
    /// Don't mess up with the stuff below
    /// </summary>

    public bool isMaterialChange;
    public Material material;
}
