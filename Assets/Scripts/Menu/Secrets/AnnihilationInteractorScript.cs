using UnityEngine;

public class AnnihilationInteractorScript : MenuKeyboardInputInteractorScript
{
    protected override MenuSecretRepositoryScript CreateRepository() => new AnnihilationSecretRepositoryScript();

    protected override void HandleCode()
    {
        Debug.Log("Annihilation");
    }
}