using UnityEngine;

namespace Menu.Screens.Main
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