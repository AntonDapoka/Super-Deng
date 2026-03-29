using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePresenterScript : MonoBehaviour
{
    [SerializeField] private PlayerStateViewScript playerStateView;

    [SerializeField] private GameObject partMiddle;
    [SerializeField] private GameObject partRight;
    [SerializeField] private GameObject partLeft;
    [SerializeField] private GameObject partTop;

    private MeshRenderer rendPartTop;
    private MeshRenderer rendPartMiddle;
    private MeshRenderer rendPartLeft;
    private MeshRenderer rendPartRight;

    public void SetColoredState()
    {
        
    }

    public void RemoveColoredState()
    {
        
    }

    public void SetBlinkingState()
    {
        
    }

    public void RemoveBlinkingState()
    {
        
    }

    public void SetInvincibilityFramesState()
    {
        
    }

    public void RemoveInvincibilityFramesState()
    {
        
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

        /*
    public void SetPartsMaterial(Material material)
    {
        rendPartTop.material = material;
        rendPartMiddle.material = material;
        rendPartLeft.material = material;
        rendPartRight.material = material;
    }*/

    public void TakeHP(int hp)
    {
        if (hp < 4)
        {
            hp += 1;
        }
        /*if (hp == 2)
        {
            rendPartLeft.material = materialTurnOn;
        }
        if (hp == 3)
        {
            rendPartRight.material = materialTurnOn;
        }
        if (hp == 4)
        {
            rendPartTop.material = materialTurnOn;
        }*/
    }
}
