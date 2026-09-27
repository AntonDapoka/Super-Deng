using UnityEngine;
using UnityEngine.InputSystem;

public class KonamiSecretRepositoryScript : MenuSecretRepositoryScript
{
    private static readonly Key[] KonamiCode =
       {
        Key.UpArrow,
        Key.UpArrow,
        Key.DownArrow,
        Key.DownArrow,
        Key.LeftArrow,
        Key.RightArrow,
        Key.LeftArrow,
        Key.RightArrow,
        Key.B,
        Key.A
    };

    private void Awake()
    {
        code = KonamiCode;
    }
}