using UnityEngine;

namespace Menu.Screens.LevelSelection
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