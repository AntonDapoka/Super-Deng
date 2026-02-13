using UnityEngine;

[CreateAssetMenu(fileName = "RedWaveSettings", menuName = "ScriptableObjects/RedWaveSettings", order = 3)]
public class RedWaveSettings : ActionSettingsScript
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

    public bool isChasingPlayer; //Or it will be random

    public bool isLifeDuration;  //Or it will be kinda immortal? It can still die, but only from hitting the player or getting stuck
    public float lifeDurationSeconds;
    public float lifeDurationBeats;

    public RedWaveBuddingType typeRedWaveBudding;

    /// <summary>
    /// Часть ниже является необязательной и даже нежелательной, но к ней нужно проявить дотошное внимание
    /// </summary>

    public bool isBasicSettingsChange;

    public bool isMaterialChange;
    public Material material;

    public bool isColorDurationChange;
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
