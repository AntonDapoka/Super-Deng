using UnityEngine.InputSystem;

public abstract class MenuSecretRepositoryScript
{
    private protected abstract Key[] Code { get; }

    public bool Contains(Key[] sequence)
    {
        if (sequence == null || sequence.Length < Code.Length)
            return false;

        int offset = sequence.Length - Code.Length;

        for (int i = 0; i < Code.Length; i++)
        {
            if (sequence[offset + i] != Code[i])
                return false;
        }

        return true;
    }
}