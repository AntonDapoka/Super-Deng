using UnityEngine;

namespace Menu.Screens.Credits
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