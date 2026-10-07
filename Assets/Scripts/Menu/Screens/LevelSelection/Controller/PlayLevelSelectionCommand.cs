using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class PlayLevelSelectionCommand : Command
        {
            [SerializeField] private LevelSelectionInteractorScript interactor;

            public override void Execute()
            {
                interactor.PlaySelectedLevel();
            }
    }
}