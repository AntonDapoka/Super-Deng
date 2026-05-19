using UnityEngine;

public class PlayerAbilityControllerScript : InputHandlerScript
{
    [SerializeField] private PlayerAbilityInteractorScript playerAbilityInteractorScript;

    public override void HandleInput(KeyCode key)
    {
        if (key == keyBindings.keyLeft)
            ActivateAbility(AbilityType.Taunt);
        else if (key == keyBindings.keyRight)
            ActivateAbility(AbilityType.Taunt);
        else if (key == keyBindings.keyTop)
            ActivateAbility(AbilityType.Taunt);
        //else if (key == keyBindings.keyCenter)
        //    ActivateAbility(AbilityType.Taunt);
    }


    private void ActivateAbility(AbilityType type)
    {
        playerAbilityInteractorScript.ActivateAbility(type);
    }
}
