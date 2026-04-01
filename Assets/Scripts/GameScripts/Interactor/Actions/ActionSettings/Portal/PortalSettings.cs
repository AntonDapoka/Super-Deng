using UnityEngine;

[CreateAssetMenu(fileName = "PortalSettings", menuName = "ScriptableObjects/PortalSettings", order = 9)]
public class PortalSettings : ActionSpawnerSettingsScript
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
