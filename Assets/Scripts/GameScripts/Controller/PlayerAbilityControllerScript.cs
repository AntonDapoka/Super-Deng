using UnityEngine;

public class PlayerAbilityControllerScript : InputHandlerScript
{
    [SerializeField] private PlayerAbilityInteractorScript playerAbilityInteractorScript;

    private AbilityType? currentlyHeldAbility;

    public override void HandleInput(KeyCode key)
    {
        AbilityKeyBindingDataScript keyBindingAbility = keyBindings as AbilityKeyBindingDataScript;

        if (keyBindingAbility != null)
        {
            AbilityType? type = null;

            if (key == keyBindingAbility.keyLeft)
                type = AbilityType.RedFaceCreation;
            else if (key == keyBindingAbility.keyRight)
                type = AbilityType.JumpFaceCreation;
            else if (key == keyBindingAbility.keyTop)
                type = AbilityType.PortalCreation;
            else if (key == keyBindingAbility.keyCenter)
                type = AbilityType.Taunt;

            if (type.HasValue)
            {
                ActivateAbility(type.Value);
                currentlyHeldAbility = type.Value;
            }
        }
        else
        {
            Debug.LogError("Wrong KeyAbilityBindings");
        }
    }

    private void Update()
    {
        if (!currentlyHeldAbility.HasValue) return;

        AbilityKeyBindingDataScript keyBindingAbility = keyBindings as AbilityKeyBindingDataScript;
        if (keyBindingAbility == null) return;

        bool isAnyAbilityKeyHeld =
            Input.GetKey(keyBindingAbility.keyLeft)
            || Input.GetKey(keyBindingAbility.keyRight)
            || Input.GetKey(keyBindingAbility.keyTop)
            || Input.GetKey(keyBindingAbility.keyCenter);

        if (!isAnyAbilityKeyHeld)
        {
            playerAbilityInteractorScript.ReleaseAbility(currentlyHeldAbility.Value);
            currentlyHeldAbility = null;
        }
    }

    private void ActivateAbility(AbilityType type)
    {
        playerAbilityInteractorScript.ActivateAbility(type);
    }
}
