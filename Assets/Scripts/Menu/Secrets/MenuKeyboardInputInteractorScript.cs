
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MenuKeyboardInputInteractorScript : MonoBehaviour
{
    private MenuSecretRepositoryScript repository;

    protected abstract MenuSecretRepositoryScript CreateRepository();

    public void HandleKeyboardBuffer(Key[] buffer)
    {
        repository ??= CreateRepository();

        if (buffer == null || buffer.Length == 0)
        {
            Debug.LogWarning("Something is null");
            return;
        }

        if (repository.Contains(buffer)) HandleCode();
    }

    protected abstract void HandleCode();
}
