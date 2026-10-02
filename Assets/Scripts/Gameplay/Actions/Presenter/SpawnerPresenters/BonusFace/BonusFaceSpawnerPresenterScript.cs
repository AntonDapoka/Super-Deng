using UnityEngine;

public class BonusFaceSpawnerPresenterScript : SpawnerActionPresenterScript
{
    public override void ApplyFaceActionMaterial(GameObject face, Material material)
    {
        FaceScript faceScript = face.GetComponent<FaceScript>();
        faceScript.rend.material = material;
    }

    public override void ChangeFaceBackToDefault(GameObject face)
    {
        FaceScript faceScript = face.GetComponent<FaceScript>();
        FaceStateScript faceState = faceScript.FaceState;

        if (faceState.GetFaceState(FaceProperty.IsColored)
        || faceState.GetFaceState(FaceProperty.IsKilling)
        || faceState.GetFaceState(FaceProperty.IsBlinking)
        || faceState.GetFaceState(FaceProperty.IsBonus))
        {
            return;
        }
        else if (faceState.GetFaceState(FaceProperty.IsRight))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Right);
        else if (faceState.GetFaceState(FaceProperty.IsLeft))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Left);
        else if (faceState.GetFaceState(FaceProperty.IsTop))
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Top);
        else 
            materialHolder.SetMaterial(faceScript.rend, MaterialType.Default);
    }

    public void DestroyBonus(GameObject bonus)
    {
        if (bonus != null)
            Destroy(bonus);
    }

    public void PresentBonusType(GameObject face, ref GameObject bonusInstance, BonusFaceSettings settings, BonusFaceBasicSettings settingsBasic)
    {
        GameObject bonusPrefab = null;

        switch (settings.typeBonus)
        {
            case BonusType.Health: bonusPrefab = settingsBasic.bonusHealthPrefabBasic; break;
            case BonusType.Combo:  bonusPrefab = settingsBasic.bonusComboPrefabBasic; break;
            case BonusType.SlowMo: bonusPrefab = settingsBasic.bonusSlowMoPrefabBasic; break;
            case BonusType.Speed:  bonusPrefab = settingsBasic.bonusSpeedPrefabBasic; break;
            case BonusType.Shield: bonusPrefab = settingsBasic.bonusShieldPrefabBasic; break;
        }

        if (settings.isBonusSymbolPrefabChange)
            bonusPrefab = settings.bonusSymbolPrefab;

        if (bonusPrefab != null)
        {
            bonusInstance = Instantiate(
                bonusPrefab,
                face.transform.localPosition,
                Quaternion.identity,
                face.transform);

            bonusInstance.transform.localPosition = Vector3.zero;
            bonusInstance.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
            bonusInstance.SetActive(true);
        }

        Material material = GetMaterialFromData(settings, settingsBasic);

        if (material != null)
            ApplyFaceActionMaterial(face, material);
    }

    private Material GetMaterialFromData(BonusFaceSettings settings, BonusFaceBasicSettings settingsBasic)
    {
        Material material = null;
        BonusType bonusType = settings.typeBonus;

        switch (bonusType)
        {
            case BonusType.Health: material = settingsBasic.materialHealthBasic; break;
            case BonusType.Combo: material = settingsBasic.materialComboBasic; break;
            case BonusType.SlowMo: material = settingsBasic.materialSlowMoBasic; break;
            case BonusType.Speed: material = settingsBasic.materialSpeedBasic; break;
            case BonusType.Shield: material = settingsBasic.materialShieldBasic; break;
        }

        if (settings.isBasicSettingsChange && settings.isMaterialChange) material = settings.material;

        return material;
    }

    public void PlayBonusDyingAnimation(GameObject face, GameObject bonus, BonusFaceSettings settings, BonusFaceBasicSettings settingsBasic, float timerCurrent, float duration)
    {
        float t = Mathf.Clamp01(timerCurrent / duration);

        float minFrequency = 1.0f;
        float maxFrequency = 3f;

        float frequency = Mathf.Lerp(minFrequency, maxFrequency, t * t);

        bool useFirst = Mathf.FloorToInt(timerCurrent * frequency) % 2 == 0;

        Material material = GetMaterialFromData(settings, settingsBasic);

        if (material != null)
        {
            ApplyFaceActionMaterial(face, useFirst ? material : materialHolder.GetMaterial(MaterialType.Grey));
        }
        if (bonus != null)
            bonus.SetActive(useFirst);
    }
}
