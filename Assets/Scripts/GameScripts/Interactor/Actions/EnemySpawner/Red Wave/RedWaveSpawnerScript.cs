using System.Collections.Generic;
using UnityEngine;

public class RedWaveSpawnerScript : SpawnerActionScript
{
    private List<RedWaveScript> redWaves = new();
    private RedWaveSettings redWaveSettings;
    [SerializeField] private RedWaveBasicSettings redWaveBasicSettings;
    [SerializeField] private RedWaveSpawnerPresenterScript redWavePresenter;

    public override void SetSettings(ActionSettingsScript settings)
    {
        if (settings == null || faces == null || settings as RedWaveSettings == null)
        {
            Debug.LogError($"RedWaveSpawner REQUIRES RedWaveSettings, but received {settings?.GetType().Name ?? "null"}");
            return;
        }
        redWaveSettings = settings as RedWaveSettings;

        isRandomSpawn = redWaveSettings.isRandom;
        if (isRandomSpawn) SetRandomSpawnSettings(redWaveSettings);

        isCertainSpawn = redWaveSettings.isCertain;
        if (isCertainSpawn) SetCertainSettings(redWaveSettings);

        isProximityLimit = redWaveSettings.isProximityLimit;
        if (isProximityLimit) SetProximityLimitSettings(redWaveSettings);

        isDistanceLimit = redWaveSettings.isDistanceLimit;
        if (isDistanceLimit) SetDistanceLimitSettings(redWaveSettings);
    }

    private void SetRandomSpawnSettings(RedWaveSettings settings)
    {
        isStableQuantity = settings.isStableQuantity;
        quantityExact = settings.quantityExact;
        quantityMin = settings.quantityMin;
        quantityMax = settings.quantityMax;
    }

    private void SetCertainSettings(RedWaveSettings settings)
    {
        isRelativeToPlayer = settings.isRelativeToPlayer;
        arrayOfFacesRelativeToPlayer = settings.arrayOfFacesRelativeToPlayer;
        isRelativeToFigure = settings.isRelativeToFigure;
        arrayOfFacesRelativeToFigure = settings.arrayOfFacesRelativeToFigure;
    }

    private void SetProximityLimitSettings(RedWaveSettings settings)
    {
        isProximityLimit = settings.isProximityLimit;
        proximityLimit = settings.proximityLimit;
    }

    private void SetDistanceLimitSettings(RedWaveSettings settings)
    {
        isDistanceLimit = settings.isDistanceLimit;
        distanceLimit = settings.distanceLimit;
    }

    public override void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings)
    {
        if (actionBasicSettings is not RedWaveBasicSettings redWaveSettings)
        {
            Debug.LogError("actionBasicSettings must be of type RedWaveBasicSettingsScript");
            return;
        }

        redWaveBasicSettings = redWaveSettings;
    }

    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject face)
    {
        if (isTurnOn) redWaves.Add(CreateRedWave(face));
    }

    private RedWaveScript CreateRedWave(GameObject face)
    {
        if (face == null)
            Debug.Log("Face null");

        if (redWaveSettings == null)
            Debug.Log("redWaveSettings null");

        if (redWaveBasicSettings == null)
            Debug.Log("redWaveBasicSettings null");

        if (redWavePresenter == null)
            Debug.Log("presenter null");

        return new RedWaveScript(face, redWaveSettings, redWaveBasicSettings, redWavePresenter);
    }

    private void Update()
    {
        for (int i = redWaves.Count - 1; i >= 0; i--)
        {
            redWaves[i].Update();

            if (redWaves[i].IsFinished)
                redWaves.RemoveAt(i);
        }
    }
}