using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatSyncPresenterScript : MonoBehaviour
{
    private bool isTurnOn = false;
    [SerializeField] private PlayerBeatSyncViewScript playerBeatSyncView;
    [SerializeField] private Material materialToFade;
    [SerializeField] private float factorCanCombo = 0f;
    [SerializeField] private float factorBasic = 0f;
    [SerializeField] private float factorCanPress = 0.25f;
    [SerializeField] private float factorNoNo = 0.75f;

    public void Initialize()
    {
        playerBeatSyncView.SetMaterialAlpha(materialToFade, 0f);

        TurnOn();
    }

    public void SetCanComboState()
    {
        if (!isTurnOn) return;

        playerBeatSyncView.SetMaterialAlpha(materialToFade, factorCanCombo);
    }

    public void SetCanPressState()
    {
        if (!isTurnOn) return;

        playerBeatSyncView.SetMaterialAlpha(materialToFade, factorCanPress);
    }

    public void SetCanNoNoState()
    {
        if (!isTurnOn) return;

        playerBeatSyncView.SetMaterialAlpha(materialToFade, factorNoNo);
    }

    public void SetCanBasicState()
    {
        playerBeatSyncView.SetMaterialAlpha(materialToFade, factorBasic);
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }
}
