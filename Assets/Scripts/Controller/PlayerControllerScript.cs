using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour, IInputHandlerScript
{
    [SerializeField] private KeyBindingDataScript keyBindings;
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractorScript;

    public PlayerMovementInteractorScript PlayerMovementInteractorScript => playerMovementInteractorScript;

    public void HandleInput(KeyCode key)
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
        PlayerMovementInteractorScript.MovePlayer(direction);
    }
}
