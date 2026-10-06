using Menu.Screens.Settings;
using UnityEngine;

namespace Menu.Controller.Commands
{
    public class SettingsVolumeDecreaseCommand : Command
    {
        [SerializeField] private SettingsInteractorScript interactor;
        [SerializeField] private VolumeChannel channel;

        public override void Execute()
        {
            interactor.DecreaseVolume(channel);
        }
    }
}