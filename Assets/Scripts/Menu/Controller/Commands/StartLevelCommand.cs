using UnityEngine;

public sealed class StartLevelCommand : Command
{
    [SerializeField] private StartLevelInteractorScript interactor;

    public override void Execute()
    {
        _ = interactor.StartLevelAsync();
    }
}
