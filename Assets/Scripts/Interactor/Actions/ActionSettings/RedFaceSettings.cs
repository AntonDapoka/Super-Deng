using UnityEngine;

[CreateAssetMenu(fileName = "RedFaceSettings", menuName = "ScriptableObjects/RedFaceSettings", order = 2)]
public class RedFaceSettings : ActionSettingsScript
{
    public bool isRandom;
    public bool isCertain; // Или активируются конкретные грани?
    //public bool isResetAfterDeath;

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

    public bool isColorDurationBeatsChange;
    public float colorDurationBeats;
    public float colorDurationSeconds;

    public bool isScaleUpDurationChange;
    public float scaleUpDurationBeats;
    public float scaleUpDurationSeconds;

    public bool isWaitDurationChange;
    public float waitDurationBeats;
    public float waitDurationSeconds;

    public bool isScaleDownDurationChange;
    public float scaleDownDurationBeats;
    public float scaleDownDurationSeconds;

    public bool isHeightChange;
    public float height;

    public bool isOffsetChange;
    public float offset;
}
