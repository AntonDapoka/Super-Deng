using UnityEngine;

public interface IInputHandlerScript
{
    PlayerInteractorScript PlayerInteractorScript { get; }

    void HandleInput(KeyCode key);
}