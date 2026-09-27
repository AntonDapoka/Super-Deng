using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MenuSecretRepositoryScript : MonoBehaviour, IMenuSecretRepositoryScript
{
    private protected Key[] code;

    public bool Contains(Key[] sequence)
    {
        if (sequence == null || sequence.Length < code.Length)
            return false;

        int offset = sequence.Length - code.Length;

        for (int i = 0; i < code.Length; i++)
        {
            if (sequence[offset + i] != code[i])
                return false;
        }

        return true;
    }
}