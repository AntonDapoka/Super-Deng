using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class ChooseLevelCommand : Command
    {
        [SerializeField] private OpenLevelSelectionInteractorScript interactor;

        public override void Execute()
        {
            interactor.StartToChooseLevel();
        }
    }
}