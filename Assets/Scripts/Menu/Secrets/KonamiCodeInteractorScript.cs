using UnityEngine;

public class KonamiCodeInteractorScript : MenuKeyboardInputInteractorScript
{
    protected override MenuSecretRepositoryScript CreateRepository() => new KonamiSecretRepositoryScript();

    protected override void HandleCode()
    {
        Debug.Log("KonamiCode");
    }
}