using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class OpenLevelSelectionCommand : Command
    {
        [SerializeField] private OpenLevelSelectionInteractorScript interactor;

        public override void Execute()
        {
            interactor.StartToChooseLevel();
        }
    }
}