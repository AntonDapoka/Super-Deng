using UnityEngine;

public class PlayerMovementControllerScript : InputHandlerScript
{
    public override void HandleInput(KeyCode key)
    {
        if (key == keyBindings.moveLeft)
            MovePlayer("Left");
        else if (key == keyBindings.moveRight)
            MovePlayer("Right");
        else if (key == keyBindings.moveTop)
            MovePlayer("Top");
    }

    private void MovePlayer(string direction)
    {
        playerMovementInteractorScript.MovePlayer(direction);
    }
}
