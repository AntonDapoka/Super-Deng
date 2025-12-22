
using UnityEngine;

public abstract class MenuKeyboardInputInteractorScript : MonoBehaviour, IMenuKeyboardInputInteractorScript
{
    [SerializeField] private MonoBehaviour repository;
    private IMenuSecretRepositoryScript Repository => repository as IMenuSecretRepositoryScript;

    public void HandleKeyboardBuffer(KeyCode[] buffer)
    {
        if (buffer == null || buffer.Length == 0 || Repository == null)
        {
            Debug.LogWarning("Something is null");
            return;
        }
            
        if (Repository.Contains(buffer))
        {
            HandleCode();
        }
    }

    protected abstract void HandleCode();
}
