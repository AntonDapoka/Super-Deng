using Menu.Screens.Settings;
using UnityEngine;

namespace Menu.Controller.Commands
{
    public class SettingsResetCommand : Command
    {
        [SerializeField] private SettingsInteractorScript interactor;

        public override void Execute()
        {
            interactor.ResetToDefaults();
        }
    }
}