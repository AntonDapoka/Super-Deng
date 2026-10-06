using UnityEngine;

using Menu.Screens.Credits;

namespace Menu.Controller.Commands
{
    public class OpenCreditsCommand : Command
    {
        [SerializeField] private OpenCreditsInteractorScript interactor;

        public override void Execute()
        {
            _ = interactor.OpenCreditsAsync();
        }
    }
}