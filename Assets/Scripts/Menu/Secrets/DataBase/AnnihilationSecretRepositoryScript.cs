using UnityEngine.InputSystem;

public class AnnihilationSecretRepositoryScript : MenuSecretRepositoryScript
{
    private static readonly Key[] AnnihilationCode =
       {
        Key.A,
        Key.S,
        Key.Comma,
        Key.Z,
        Key.B
    };

    private protected override Key[] Code => AnnihilationCode;
}