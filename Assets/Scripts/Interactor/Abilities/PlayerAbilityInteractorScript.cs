using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityInteractorScript : MonoBehaviour
{
    [SerializeField] private PlayerAbilityTauntInteractorScript tauntInteractor;
    [SerializeField] private PlayerMovementInteractorScript movementInteractor;

    public void ActivateAbility(AbilityType type)
    {
        if (type == AbilityType.Taunt)
        {
            tauntInteractor.Activate(movementInteractor.GetCurrentFace());
        }
    }
}
