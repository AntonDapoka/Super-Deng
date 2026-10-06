using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class ChooseLevelInteractorScript : MonoBehaviour
    {
        [SerializeField] private MenuNavigationScript menuNavigation;

        public void StartToChooseLevel()
        {
            _ = menuNavigation.NavigateTo(MenuState.LevelSelection);
        }
    }
}