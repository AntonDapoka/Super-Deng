using System.Collections.Generic;
using UnityEngine;

public class RedFaceSpawnerScript : SpawnerActionScript
{
    private List<RedFaceScript> redFaces = new();
    private RedFaceSettings redFaceSettings;
    private RedFaceBasicSettings redFaceBasicSettings;
    [SerializeField] private RedFaceSpawnerPresenterScript presenter;

    public override void SetSettings<T>(T settings)
    {
        redFaceSettings = settings as RedFaceSettings;
        if (redFaceSettings == null)
        {
            Debug.LogError($"RedFaceSpawner REQUIRES RedFaceSettings, but received {settings?.GetType().Name ?? "null"}");
            return;
        }

        isRandomSpawn = redFaceSettings.isRandom;
        isCertainSpawn = redFaceSettings.isCertain;
        isBasicSettingsChange = redFaceSettings.isBasicSettingsChange;

        if (isRandomSpawn)
        {
            isStableQuantity = redFaceSettings.isStableQuantity;
            quantityExact = redFaceSettings.quantityExact;
            quantityMin = redFaceSettings.quantityMin;
            quantityMax = redFaceSettings.quantityMax;
        }

        if (isCertainSpawn)
        {
            isRelativeToPlayer = redFaceSettings.isRelativeToPlayer;
            arrayOfFacesRelativeToPlayer = redFaceSettings.arrayOfFacesRelativeToPlayer;
            isRelativeToFigure = redFaceSettings.isRelativeToFigure;
            arrayOfFacesRelativeToFigure = redFaceSettings.arrayOfFacesRelativeToFigure;
        }

        isProximityLimit = redFaceSettings.isProximityLimit;
        proximityLimit = redFaceSettings.proximityLimit;
        isDistanceLimit = redFaceSettings.isDistanceLimit;
        distanceLimit = redFaceSettings.distanceLimit;

        if (isBasicSettingsChange)
        {
            /////
        }
        else ApplyBasicSettings();
    }

    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject face)
    {
        if (isTurnOn) redFaces.Add(CreateRedFace(face));
    }

    private RedFaceScript CreateRedFace(GameObject face)
    {
        if (face == null)
            Debug.Log("Face null");

        if (redFaceSettings == null)
            Debug.Log("redFaceSettings null");

        if (presenter == null)
            Debug.Log("presenter null");

        return new RedFaceScript(face, redFaceSettings, presenter);
    }

    private void Update()
    {
        for (int i = redFaces.Count - 1; i >= 0; i--)
        {
            redFaces[i].Update();

            if (redFaces[i].IsFinished)
                redFaces.RemoveAt(i);
        }
    }

    public override void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings)
    {
        if (actionBasicSettings is not RedFaceBasicSettings redFaceSettings)
        {
            Debug.LogError("actionBasicSettings must be of type RedFaceBasicSettingsScript");
            return;
        }

        redFaceBasicSettings = redFaceSettings;
    }

    private void ApplyBasicSettings()
    {

    }
}