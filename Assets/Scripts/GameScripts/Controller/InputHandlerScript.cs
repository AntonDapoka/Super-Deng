using UnityEngine;

public abstract class InputHandlerScript: MonoBehaviour
{
    [SerializeField] protected KeyBindingDataScript keyBindings;
    [SerializeField] protected PlayerMovementInteractorScript playerMovementInteractorScript;

    public void SetKeyBindings(KeyBindingDataScript keyBindingsNew)
    {
        keyBindings = keyBindingsNew;
    }
    
    public abstract void HandleInput(KeyCode key);
}