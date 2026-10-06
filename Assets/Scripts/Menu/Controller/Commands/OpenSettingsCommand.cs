using UnityEngine;

using Menu.Screens.Settings;

namespace Menu.Controller.Commands
{
    public class OpenSettingsCommand : Command
    {
        [SerializeField] private OpenSettingsInteractorScript interactor;

        public override void Execute()
        {
            _ = interactor.OpenSettingsAsync();
        }
    }
}