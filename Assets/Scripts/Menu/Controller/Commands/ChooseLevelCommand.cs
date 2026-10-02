using UnityEngine;

namespace Menu.Commands.Buttons
{
    public class ChooseLevelCommand : Command
    {
        [SerializeField] private ChooseLevelInteractorScript interactor;

        public override void Execute()
        {
            interactor.StartToChooseLevel();
        }
    }
}