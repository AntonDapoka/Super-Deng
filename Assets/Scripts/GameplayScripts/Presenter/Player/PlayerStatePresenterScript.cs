using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePresenterScript : MonoBehaviour
{
    [SerializeField] private PlayerStateViewScript playerStateView;

    [SerializeField] private Material materialTurnOn;
    [SerializeField] private Material materialTurnOff;
    [SerializeField] private Material materialRed;

    private MeshRenderer[] heartParts;

    private int hp = 4;

    public void Initialize(PlayerReferencesHolderScript referencesHolder)
    {
        heartParts = CollectMeshRenderers(referencesHolder.HeartParts);
    }

    private MeshRenderer[] CollectMeshRenderers(GameObject[] objects)
    {
        List<MeshRenderer> result = new();

        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;

            if (obj.TryGetComponent<MeshRenderer>(out var renderer)) result.Add(renderer);
        }

        return result.ToArray();
    }

    public void SetNewHP(int hp)
    {
        this.hp = hp;
    }

    public void DisplayHP()
    {
        Material[] materials = new Material[] { materialTurnOff, materialTurnOn };
        for (int i = 0; i < 4; i++)
            heartParts[i].material = materials[(hp >= i + 1) ? 1 : 0];
    }

    public void SetPartsMaterial(Material material)
    {
        foreach (MeshRenderer part in heartParts)
            part.material = material;
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
