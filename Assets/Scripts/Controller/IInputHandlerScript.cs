using UnityEngine;

public interface IInputHandlerScript
{
    PlayerMovementInteractorScript PlayerMovementInteractorScript { get; }

    void HandleInput(KeyCode key);
}