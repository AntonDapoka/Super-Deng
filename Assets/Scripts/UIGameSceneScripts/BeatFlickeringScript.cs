using UnityEngine;

public class BeatFlickeringScript : MonoBehaviour
{
    public bool isTurnOn = false;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private BeatController BC;


    public Material materialToFade;



    private void Start()
    {
        SetMaterialAlpha(0f);
    }

    private void Update()
    {
        if (isTurnOn)
        {
            if (BC.canPress)
            {
                SetMaterialAlpha(0.25f);
                if (BC.canCombo)
                {
                    SetMaterialAlpha(0f);
                    //Debug.Log("0f");
                }

            }
            else
            {
                SetMaterialAlpha(0.75f);
                //Debug.Log("1f");
            }
        }
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = materialToFade.color;
        color.a = alpha;
        materialToFade.color = color;
    }
}
