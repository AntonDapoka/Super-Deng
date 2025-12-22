using System;
using UnityEngine;

public class AnnihilationInteractorScript : MonoBehaviour, IMenuKeyboardInputInteractorScript
{
    private readonly IMenuSecretStringRepositoryScript _repository;
    private readonly Action<string> _onMatch;

    public AnnihilationInteractorScript(
        IMenuSecretStringRepositoryScript repository,
        Action<string> onMatch)
    {
        _repository = repository;
        _onMatch = onMatch;
    }

    public void HandleKeyboardBuffer(char[] buffer)
    {
        if (buffer == null || buffer.Length == 0)
            return;

        string input = new string(buffer);

        if (_repository.Contains(input))
        {
            _onMatch?.Invoke(input);
        }
    }
}