using System.Collections.Generic;
using UnityEngine;

public class BonusFaceSpawnerScript : SpawnerActionScript
{
    private List<BonusFaceScript> bonusFaces = new();
    private BonusFaceSettings bonusFaceSettings;
    [SerializeField] private BonusFaceBasicSettings bonusFaceBasicSettings;
    private BonusFaceSpawnerPresenterScript bonusFacePresenter;

    public override void SetSettings(ActionSettingsScript settings)
    {
        if (settings == null || faces == null || settings as BonusFaceSettings == null)
        {
            Debug.LogError($"BonusFaceSpawner REQUIRES BonusFaceSettings, but received {settings?.GetType().Name ?? "null"}");
            return;
        }
        bonusFaceSettings = settings as BonusFaceSettings;
        bonusFacePresenter = presenter as BonusFaceSpawnerPresenterScript;

        isRandomSpawn = bonusFaceSettings.isRandom;
        if (isRandomSpawn) SetRandomSpawnSettings(bonusFaceSettings);

        isCertainSpawn = bonusFaceSettings.isCertain;
        if (isCertainSpawn) SetCertainSettings(bonusFaceSettings);

        isProximityLimit = bonusFaceSettings.isProximityLimit;
        if (isProximityLimit) SetProximityLimitSettings(bonusFaceSettings);

        isDistanceLimit = bonusFaceSettings.isDistanceLimit;
        if (isDistanceLimit) SetDistanceLimitSettings(bonusFaceSettings);

        isForcedBreak = false;
    }

    private void SetRandomSpawnSettings(BonusFaceSettings settings)
    {
        isStableQuantity = settings.isStableQuantity;
        quantityExact = settings.quantityExact;
        quantityMin = settings.quantityMin;
        quantityMax = settings.quantityMax;
    }

    private void SetCertainSettings(BonusFaceSettings settings)
    {
        isRelativeToPlayer = settings.isRelativeToPlayer;
        arrayOfFacesRelativeToPlayer = settings.arrayOfFacesRelativeToPlayer;
        isRelativeToFigure = settings.isRelativeToFigure;
        arrayOfFacesRelativeToFigure = settings.arrayOfFacesRelativeToFigure;
    }

    private void SetProximityLimitSettings(BonusFaceSettings settings)
    {
        isProximityLimit = settings.isProximityLimit;
        proximityLimit = settings.proximityLimit;
    }

    private void SetDistanceLimitSettings(BonusFaceSettings settings)
    {
        isDistanceLimit = settings.isDistanceLimit;
        distanceLimit = settings.distanceLimit;
    }

    public override void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings)
    {
        if (actionBasicSettings is not BonusFaceBasicSettings bonusFaceSettings)
        {
            Debug.LogError("actionBasicSettings must be of type BonusFaceBasicSettingsScript");
            return;
        }

        bonusFaceBasicSettings = bonusFaceSettings;
    }

    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject face)
    {
        if (isTurnOn && !isForcedBreak) bonusFaces.Add(CreateBonusFace(face, bonusFaceSettings.typeBonus)); //Replace
    }

    public override void Cancel()
    {
        TurnOff();
    }

    public override void ForcedBreak()
    {
        isForcedBreak = true;

        for (int i = bonusFaces.Count - 1; i >= 0; i--)
        {
            bonusFaces[i].ForcedBreak();
            bonusFaces.RemoveAt(i);
        }
    }

    private BonusFaceScript CreateBonusFace(GameObject face, BonusType bonusType)
    {
        if (face == null)
            Debug.Log("Face null");

        if (bonusFaceSettings == null)
            Debug.Log("bonusFaceSettings null");

        if (bonusFaceBasicSettings == null)
            Debug.Log("bonusFaceBasicSettings null");

        if (bonusFacePresenter == null)
            Debug.Log("presenter null");

        return new BonusFaceScript(face, bonusFaceSettings, bonusFaceBasicSettings, bonusFacePresenter, bonusType);
    }

    private void Update()
    {
        if (isForcedBreak) return;

        for (int i = bonusFaces.Count - 1; i >= 0; i--)
        {
            bonusFaces[i].Update();

            if (bonusFaces[i].IsFinished)
                bonusFaces.RemoveAt(i);
        }
    }
}