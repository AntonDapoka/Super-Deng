using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityControllerScript : InputHandlerScript
{
    [SerializeField] private KeyBindingDataScript keyBindings;
    [SerializeField] private PlayerAbilityInteractorScript playerAbilityInteractorScript;

    public override void HandleInput(KeyCode key)
    {
        if (key == keyBindings.moveLeft)
            ActivateAbility(AbilityType.Taunt);
        else if (key == keyBindings.moveRight)
            ActivateAbility(AbilityType.Taunt);
        else if (key == keyBindings.moveTop)
            ActivateAbility(AbilityType.Taunt);
    }

    private void ActivateAbility(AbilityType type)
    {
        playerAbilityInteractorScript.ActivateAbility(type);
    }
}
