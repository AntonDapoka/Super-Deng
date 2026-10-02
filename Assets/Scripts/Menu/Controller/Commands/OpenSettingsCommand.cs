using UnityEngine;

public class OpenSettingsCommand : Command
{
    [SerializeField] private OpenSettingsInteractorScript interactor;

    public override void Execute()
    {
        _ = interactor.OpenSettingsAsync();
    }
}
