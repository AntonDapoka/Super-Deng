using System.Collections.Generic;
using UnityEngine;

public class RedWaveSpawnerScript : SpawnerActionScript
{
    private List<RedWaveScript> redWaves = new();
    private RedWaveSettings redWaveSettings;
    [SerializeField] private RedWaveBasicSettings redWaveBasicSettings;
    private RedWaveSpawnerPresenterScript redWavePresenter;

    public override void SetSettings(ActionSettingsScript settings)
    {
        if (settings == null || faces == null || settings as RedWaveSettings == null)
        {
            Debug.LogError($"RedWaveSpawner REQUIRES RedWaveSettings, but received {settings?.GetType().Name ?? "null"}");
            return;
        }
        redWaveSettings = settings as RedWaveSettings;
        redWavePresenter = presenter as RedWaveSpawnerPresenterScript;

        isRandomSpawn = redWaveSettings.isRandom;
        if (isRandomSpawn) SetRandomSpawnSettings(redWaveSettings);

        isCertainSpawn = redWaveSettings.isCertain;
        if (isCertainSpawn) SetCertainSettings(redWaveSettings);

        isProximityLimit = redWaveSettings.isProximityLimit;
        if (isProximityLimit) SetProximityLimitSettings(redWaveSettings);

        isDistanceLimit = redWaveSettings.isDistanceLimit;
        if (isDistanceLimit) SetDistanceLimitSettings(redWaveSettings);

        isForcedBreak = false;

        Debug.Log("Everything is setted");
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
            Debug.LogError("actionBasicSettings must be of type RedWaveBasicSettings");
            return;
        }

        redWaveBasicSettings = redWaveSettings;

        Debug.Log("Basic is setted");
    }

    public override bool IsSuitableSpecialRequirements(FaceScript FS, FaceStateScript FSS)
    {
        return true;
    }

    public override void SetActionFace(GameObject face)
    {
        if (isTurnOn && !isForcedBreak) redWaves.Add(CreateRedWave(face));
    }

    public override void Cancel()
    {
        TurnOff();
    }

    public override void ForcedBreak()
    {
        isForcedBreak = true;

        for (int i = redWaves.Count - 1; i >= 0; i--)
        {
            redWaves[i].ForcedBreak();
            redWaves.RemoveAt(i);
        }
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
        Debug.Log("cREATED");
        return new RedWaveScript(face, redWaveSettings, redWaveBasicSettings, redWavePresenter);
    }

    private void Update()
    {
        if (isForcedBreak) return;

        for (int i = redWaves.Count - 1; i >= 0; i--)
        {
            redWaves[i].Update();

            if (redWaves[i].ShouldBud && isTurnOn)
            {
                GameObject budFace = SelectBudFace(redWaves[i].CurrentFace);
                if (budFace != null)
                    redWaves.Add(CreateRedWave(budFace));
                redWaves[i].MarkBudded();
            }

            if (redWaves[i].IsFinished)
                redWaves.RemoveAt(i);
        }
    }

    private GameObject SelectBudFace(GameObject currentFace)
    {
        FaceScript currentFaceScript = currentFace.GetComponent<FaceScript>();

        if (currentFaceScript.GetPathObjectCount() == 0)
            return null;

        FaceScript[] adjacentFaces = new FaceScript[3];
        if (currentFaceScript.side1 != null)
            adjacentFaces[0] = currentFaceScript.side1.GetComponent<FaceScript>();
        if (currentFaceScript.side2 != null)
            adjacentFaces[1] = currentFaceScript.side2.GetComponent<FaceScript>();
        if (currentFaceScript.side3 != null)
            adjacentFaces[2] = currentFaceScript.side3.GetComponent<FaceScript>();

        List<FaceScript> validFaces = new List<FaceScript>();
        foreach (FaceScript adj in adjacentFaces)
        {
            if (adj == null) continue;

            FaceStateScript adjState = adj.GetComponent<FaceStateScript>();
            if (CheckIsSuitableFace(adj, adjState))
            {
                validFaces.Add(adj);
            }
        }

        if (validFaces.Count == 0)
            return null;

        if (redWaveSettings.isChasingPlayer)
        {
            FaceScript target = null;
            int minPath = int.MaxValue;

            foreach (FaceScript adj in validFaces)
            {
                int pathCount = adj.GetPathObjectCount();
                if (pathCount < minPath)
                {
                    minPath = pathCount;
                    target = adj;
                }
            }

            return target?.gameObject;
        }
        else
        {
            return validFaces[Random.Range(0, validFaces.Count)].gameObject;
        }
    }
}
