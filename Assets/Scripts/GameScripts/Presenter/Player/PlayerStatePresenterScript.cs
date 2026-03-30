using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePresenterScript : MonoBehaviour
{
    [SerializeField] private PlayerStateViewScript playerStateView;

    [SerializeField] private GameObject partTop;
    [SerializeField] private GameObject partMiddle;
    [SerializeField] private GameObject partLeft;
    [SerializeField] private GameObject partRight;

    [SerializeField] private Material materialTurnOn;
    [SerializeField] private Material materialTurnOff;
    [SerializeField] private Material materialRed;

    private MeshRenderer rendPartTop;
    private MeshRenderer rendPartMiddle;
    private MeshRenderer rendPartLeft;
    private MeshRenderer rendPartRight;

    private int hp = 4;

    private void Start() //Replace for Initialize
    {
        rendPartTop = partTop.GetComponent<MeshRenderer>();
        rendPartMiddle = partMiddle.GetComponent<MeshRenderer>();
        rendPartLeft = partLeft.GetComponent<MeshRenderer>();
        rendPartRight = partRight.GetComponent<MeshRenderer>();
    }

    public void SetNewHP(int hp)
    {
        this.hp = hp;
    }

    public void DisplayHP()
    {
        Material[] materials = new Material[] { materialTurnOff, materialTurnOn };
        MeshRenderer[] parts = new MeshRenderer[] { rendPartMiddle, rendPartTop, rendPartLeft, rendPartRight};

        for (int i = 0; i < 4; i++)
        {
            parts[i].material = materials[(hp >= i + 1) ? 1 : 0];
        }
    }

    public void SetPartsMaterial(Material material)
    {
        rendPartTop.material = material;
        rendPartMiddle.material = material;
        rendPartLeft.material = material;
        rendPartRight.material = material;
    }

    public void SetColoredState()
    {
        SetPartsMaterial(materialRed);
    }

    public void RemoveColoredState()
    {
        DisplayHP();
    }

    public void SetTakingHealthState()
    {
        
    }

    public void RemoveTakingHealthState()
    {
        DisplayHP();
    }

    public void SetBlinkingState()
    {
        
    }

    public void RemoveBlinkingState()
    {
        DisplayHP();
    }

    public void SetInvincibilityFramesState()
    {
        
    }

    public void RemoveInvincibilityFramesState()
    {
        DisplayHP();
    }

        /*
    public void ResetMaterials()
    {
        //Material[] materials = new Material[] { materialTurnOff, materialTurnOn };
        Material[] parts = new Material[] { rendPartTop.material, rendPartMiddle.material, rendPartLeft.material, rendPartRight.material };

        for (int i = 0; i < 4; i++)
        {
            parts[i] = materials[(hp >= i + 1) ? 1 : 0];
        }

        rendPartTop.material = parts[0];
        rendPartMiddle.material = parts[1];
        rendPartLeft.material = parts[2];
        rendPartRight.material = parts[3];
    }*/
}
