using UnityEngine;

[CreateAssetMenu(fileName = "FallFaceSettings", menuName = "ScriptableObjects/FallFaceSettings", order = 5)]
public class FallFaceSettings : ActionSettingsScript
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

    /// <summary>
    /// Часть ниже является необязательной и даже нежелательной, но к ней нужно проявить дотошное внимание
    /// </summary>

    public bool isBasicSettingsChange;

    public bool isMaterialChange;
    public Material material;

    public bool isBlinkingDurationChange;
    public float blinkingDurationBeats;
    public float blinkingDurationSeconds;

    public bool isReturningDurationChange;
    public float returningDurationBeats;
    public float returningDurationSeconds;

    public bool isImpulseChange;
    public float impulse;
}
