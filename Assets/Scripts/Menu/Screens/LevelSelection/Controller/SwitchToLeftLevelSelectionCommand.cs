using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class SwitchToLeftLevelSelectionCommand : Command
        {
            [SerializeField] private LevelSelectionInteractorScript interactor;

            public override void Execute()
            {
                interactor.SwitchToLeftLevel();
            }
    }
}