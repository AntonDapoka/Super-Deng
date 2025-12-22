using UnityEngine;

public class KonamiSecretRepositoryScript : MenuSecretRepositoryScript
{
    private static readonly KeyCode[] KonamiCode =
       {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };

    private void Awake()
    {
        code = KonamiCode;
    }
}