using UnityEngine;

public class AnnihilationSecretRepositoryScript : MenuSecretRepositoryScript
{
    private static readonly KeyCode[] AnnihilationCode =
       {
        KeyCode.A,
        KeyCode.S,
        KeyCode.Comma,
        KeyCode.Z,
        KeyCode.B
    };

    private void Awake()
    {
        code = AnnihilationCode;
    }
}