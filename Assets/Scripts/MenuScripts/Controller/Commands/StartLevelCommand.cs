using UnityEngine;

public sealed class StartLevelCommand : Command
{
    [SerializeField] private StartLevelInteractorScript interactor;

    public StartLevelCommand(StartLevelInteractorScript interactor)
    {
        this.interactor = interactor;
    }

    public override void Execute()
    {
        interactor.StartLevel();
    }
}