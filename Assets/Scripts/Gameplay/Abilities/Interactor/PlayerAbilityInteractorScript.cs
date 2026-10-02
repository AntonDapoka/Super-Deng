using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAbilityInteractorScript : MonoBehaviour
{
    public List<AbilityEntry> abilities;

    [Header("Ability Scripts")]
    [SerializeField] private MonoBehaviour[] abilityMonoBehaviours;
    [SerializeField] private PlayerMovementInteractorScript movementInteractor;

    private void Awake()
    {
        InitializeAbilities();
    }

    public void InitializeAbilities()
    {
        abilities = new List<AbilityEntry>();

        if (abilityMonoBehaviours == null) return;

        foreach (var mono in abilityMonoBehaviours)
        {
            if (mono == null || mono is not IAbilityScript abilityScript) return;

            AbilityType? type = ResolveAbilityType(mono);
            if (!type.HasValue)
            {
                Debug.LogWarning($"[PlayerAbilityInteractorScript] Could not resolve AbilityType for {mono.GetType().Name}!", mono);
                continue;
            }

            abilities.Add(new AbilityEntry
            {
                abilityScript = abilityScript,
                type = type.Value
            });
        }
    }

    private AbilityType? ResolveAbilityType(MonoBehaviour mono)
    {
        return mono switch
        {
            PlayerAbilityTauntInteractorScript => AbilityType.Taunt,
            PlayerAbilityRedFaceInteractorScript => AbilityType.RedFaceCreation,
            PlayerAbilityJumpFaceInteractorScript => AbilityType.JumpFaceCreation,
            PlayerAbilityPortalFaceInteractorScript => AbilityType.PortalCreation,
            _ => null
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

    public void ReleaseAbility(AbilityType type)
    {
        foreach (var ability in abilities)
        {
            if (ability.type == type && ability.abilityScript is IHoldableAbilityScript holdable)
            {
                holdable.Release();
            }
        }
    }
}

public class AbilityEntry
{
    public IAbilityScript abilityScript;
    public AbilityType type;
}
