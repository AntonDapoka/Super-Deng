using UnityEngine;

[CreateAssetMenu(fileName = "PortalSettings", menuName = "ScriptableObjects/PortalSettings", order = 9)]
public class PortalSettings : ActionSettingsScript
{
    public bool isRandom;
    public bool isCertain; // Or are specific facets activated?

    public bool isStableQuantity;
    public int quantityExact;
    public int quantityMin;
    public int quantityMax;

    public bool isRelativeToPlayer;
    public int[] arrayOfFacesRelativeToPlayer;
    public bool isRelativeToFigure;
    public int[] arrayOfFacesRelativeToFigure;

    public bool isProximityLimit;
    public int proximityLimit;
    public bool isDistanceLimit;
    public int distanceLimit;

    public BonusType typeBonus;

    public bool isLifeDuration; //Or it will be kinda immortal? 
    public float lifeDurationSeconds;
    public float lifeDurationBeats;

    /// <summary>
    /// Часть ниже является необязательной и даже нежелательной, но к ней нужно проявить дотошное внимание
    /// </summary>

    public bool isBasicSettingsChange;

    public bool isMaterialChange;
    public Material material;
}
