using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityInteractorScript : MonoBehaviour
{
    public List<AbilityEntry> abilities;
    [SerializeField] private PlayerAbilityTauntInteractorScript tauntInteractor;
    [SerializeField] private PlayerMovementInteractorScript movementInteractor;

    private void Awake() //REWRITE!!!!
    {
        abilities = new List<AbilityEntry>()
        {
            new() {
                abilityScript = tauntInteractor,
                type = AbilityType.Taunt
            }
        };
    }

    public void ActivateAbility(AbilityType type)
    {
        foreach (var ability in abilities)
        {
            if (ability.type == type)
            {
                ability.abilityScript.Activate(movementInteractor.GetCurrentFace());
            }
        }
    }
}

public class AbilityEntry
{
    public IAbilityScript abilityScript;  
    public AbilityType type;        
}