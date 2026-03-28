using UnityEngine;

public class BeatFlickeringScript : MonoBehaviour
{
    public bool isTurnOn = false;
    [SerializeField] private PlayerBeatSyncValidatorScript BC;

    public Material materialToFade;

    private void Start()
    {
        SetMaterialAlpha(0f);
    }

    private void Update()
    {
        if (isTurnOn)
        {
            if (BC.CanPress())
            {
                SetMaterialAlpha(0.25f);
                if (BC.CanCombo()) SetMaterialAlpha(0f);
            }
            else SetMaterialAlpha(0.75f);
        }
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = materialToFade.color;
        color.a = alpha;
        materialToFade.color = color;
    }
}
