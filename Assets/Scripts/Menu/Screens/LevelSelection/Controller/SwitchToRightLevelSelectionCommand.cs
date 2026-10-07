using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class SwitchToRightLevelSelectionCommand : Command
    {
        [SerializeField] private LevelSelectionInteractorScript interactor;

        public override void Execute()
        {
            interactor.SwitchToRightLevel();
        }
    }
}