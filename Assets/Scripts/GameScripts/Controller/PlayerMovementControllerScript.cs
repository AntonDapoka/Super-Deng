using UnityEngine;

public class PlayerMovementControllerScript : InputHandlerScript
{
    [SerializeField] private AbilityKeyBindingDataScript abilityKeyBindings;

    public void SetAbilityKeyBindings(AbilityKeyBindingDataScript bindings)
    {
        abilityKeyBindings = bindings;
    }

    public override void HandleInput(KeyCode key)
    {
        if (IsAnyAbilityKeyHeld()) return;

        if (key == keyBindings.keyLeft)
            MovePlayer("Left");
        else if (key == keyBindings.keyRight)
            MovePlayer("Right");
        else if (key == keyBindings.keyTop)
            MovePlayer("Top");
    }

    private bool IsAnyAbilityKeyHeld()
    {
        if (abilityKeyBindings == null) return false;

        return Input.GetKey(abilityKeyBindings.keyLeft)
            || Input.GetKey(abilityKeyBindings.keyRight)
            || Input.GetKey(abilityKeyBindings.keyTop)
            || Input.GetKey(abilityKeyBindings.keyCenter);
    }

    private void MovePlayer(string direction)
    {
        playerMovementInteractorScript.MovePlayer(direction);
    }
}
