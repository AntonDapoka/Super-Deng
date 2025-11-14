using UnityEngine;

public abstract class InputHandlerScript: MonoBehaviour
{
    PlayerMovementInteractorScript PlayerMovementInteractorScript { get; }

    public abstract void HandleInput(KeyCode key);
}