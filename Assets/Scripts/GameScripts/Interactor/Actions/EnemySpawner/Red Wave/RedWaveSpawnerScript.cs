using System.Collections;
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
        if (settings == null || faces == null || settings as RedFaceSettings == null)
        {
            Debug.LogError($"RedFaceSpawner REQUIRES RedFaceSettings, but received {settings?.GetType().Name ?? "null"}");
            return;
        }
        redWaveSettings = settings as RedWaveSettings;

        isRandomSpawn = redWaveSettings.isRandom;
        //if (isRandomSpawn) SetRandomSpawnSettings(redWaveSettings);

        isCertainSpawn = redWaveSettings.isCertain;
        //if (isCertainSpawn) SetCertainSettings(redWaveSettings);

        isProximityLimit = redWaveSettings.isProximityLimit;
        //if (isProximityLimit) SetProximityLimitSettings(redWaveSettings);

        isDistanceLimit = redWaveSettings.isDistanceLimit;
        //if (isDistanceLimit) SetDistanceLimitSettings(redWaveSettings);
    }

    private void SetRandomSpawnSettings(RedFaceSettings settings)
    {
        isStableQuantity = settings.isStableQuantity;
        quantityExact = settings.quantityExact;
        quantityMin = settings.quantityMin;
        quantityMax = settings.quantityMax;
    }

    private void SetCertainSettings(RedFaceSettings settings)
    {
        isRelativeToPlayer = settings.isRelativeToPlayer;
        arrayOfFacesRelativeToPlayer = settings.arrayOfFacesRelativeToPlayer;
        isRelativeToFigure = settings.isRelativeToFigure;
        arrayOfFacesRelativeToFigure = settings.arrayOfFacesRelativeToFigure;
    }

    private void SetProximityLimitSettings(RedFaceSettings settings)
    {
        isProximityLimit = settings.isProximityLimit;
        proximityLimit = settings.proximityLimit;
    }

    private void SetDistanceLimitSettings(RedFaceSettings settings)
    {
        isDistanceLimit = settings.isDistanceLimit;
        distanceLimit = settings.distanceLimit;
    }

    public override void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings)
    {
        if (actionBasicSettings is not RedFaceBasicSettings redFaceSettings)
        {
            Debug.LogError("actionBasicSettings must be of type RedFaceBasicSettingsScript");
            return;
        }

        //redFaceBasicSettings = redFaceSettings;
    }

    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject face)
    {
        //if (isTurnOn) redFaces.Add(CreateRedFace(face));
    }
/*
    private RedWaveScript CreateRedWave(GameObject face)
    {
        /*if (face == null)
            Debug.Log("Face null");

        if (redWaveSettings == null)
            Debug.Log("redFaceSettings null");

        if (redFaceBasicSettings == null)
            Debug.Log("redFaceBasicSettings null");

        if (presenter == null)
            Debug.Log("presenter null");

        return new RedFaceScript(face, redFaceSettings, redFaceBasicSettings, presenter);
    }
/*
    private void Update()
    {
        //Debug.Log((faces.Length).ToString());

        for (int i = redFaces.Count - 1; i >= 0; i--)
        {
            redFaces[i].Update();

            if (redFaces[i].IsFinished)
                redFaces.RemoveAt(i);
        }
    }*/
}