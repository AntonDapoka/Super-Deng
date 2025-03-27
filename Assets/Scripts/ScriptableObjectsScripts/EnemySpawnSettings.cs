using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "ScriptableObjects/EnemySpawnSettings", order = 1)]
public class EnemySpawnSettings : ScriptableObject
{
    public SpawnTimeData[] spawnTimes;
}

[System.Serializable]
public class SpawnTimeData
{
    public float time;


    public bool isRedFaceTurnOn;
    public bool isRedFaceRandom;
    public int[] arrayOfRedFaces;
    public int quantityOfRedFaces;

    public bool isRedWaveTurnOn;
    public bool isRedWaveRandom;
    public int[] arrayOfRedWaves;
    public float lifeDurationOfRedWaves;
    public int quantityOfRedWaves;
    public int proximityLimitOfRedWaves;

    public bool isFallFaceTurnOn;
    public bool isFallFaceRandom;
    public int[] arrayOfFallFaces;
    public int quantityOfFallFaces;
    public int proximityLimitOfFallFaces;
    public bool isResetDelay;
    public int resetDelayTime;

    public bool isResetFallFaceTurnOn;

    public bool isBonusTurnOn;
    public bool isBonusRandom;
    public int[] arrayOfBonuses;
    public int quantityOfBonuses;
    public int proximityLimitOfBonuses;
    public bool isBonusHealth;
    public bool isBonusCombo;

    public bool isPortalTurnOn;
    public bool isPortalRandom;
    public int[] arrayOfPortals;
    public int quantityOfPortals;
    public int proximityLimitOfPortals;

    public bool isFaceDanceTurnOn;
    public float durationOfCycleFaceDance;
    public float scaleFactorFaceDance;
    public bool isChangingFaceDance;
    public bool isIncreaseFaceDance;
    public float durationChangingFaceDance;

    public bool isSphereDanceTurnOn;
    public float angleSphereDance;
    public float durationOfCycleSphereDance;


    public bool isCameraRotationTurnOn;
    public bool isCameraRotationClockwise;
    public float speedCameraRotation;

    public bool isRGBTurnOn;
    public float speedRGB;
    public float targetValueRGB;
    public bool isSetRGBIncrease;
    public bool isSetRGBDecrease;

    public bool isOrbitsOn;
    public int quantityOfOrbits;
    public Vector3[] sizesOfOrbits;
    public float[] speedsOfOrbits;
    public Material[] materialsOfOrbits;
    public float minChangeIntervalOfOrbits;
    public float maxChangeIntervalOfOrbits;
}

