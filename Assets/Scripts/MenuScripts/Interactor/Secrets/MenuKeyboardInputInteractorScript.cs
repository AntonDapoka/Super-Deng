
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MenuKeyboardInputInteractorScript : MonoBehaviour
{
    [SerializeField] private MenuSecretRepositoryScript repository;

    public void HandleKeyboardBuffer(Key[] buffer)
    {
        if (buffer == null || buffer.Length == 0 || repository == null)
        {
            Debug.LogWarning("Something is null");
            return;
        }
            
        if (repository.Contains(buffer)) HandleCode();
    }

    protected abstract void HandleCode();
}
