using UnityEngine;

public class PlayerMovementControllerScript : InputHandlerScript
{
    public override void HandleInput(KeyCode key)
    {
        if (key == keyBindings.keyLeft)
            MovePlayer("Left");
        else if (key == keyBindings.keyRight)
            MovePlayer("Right");
        else if (key == keyBindings.keyTop)
            MovePlayer("Top");
    }

    private void MovePlayer(string direction)
    {
        playerMovementInteractorScript.MovePlayer(direction);
    }
}
