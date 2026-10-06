using UnityEngine;

namespace Menu.Screens.LevelStart
{
    public sealed class StartLevelCommand : Command
    {
        [SerializeField] private StartLevelInteractorScript interactor;

        public override void Execute()
        {
            _ = interactor.StartLevelAsync();
        }
    }
}