using UnityEngine;

using Menu.Screens.LevelStart;

namespace Menu.Controller.Commands
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