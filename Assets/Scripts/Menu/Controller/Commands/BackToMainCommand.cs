using UnityEngine;

using Menu.Screens.Main;

namespace Menu.Controller.Commands
{
    public class BackToMainCommand : Command
    {
        [SerializeField] private BackToMainInteractorScript interactor;

        public override void Execute()
        {
            interactor.BackToMain();
        }
    }
}