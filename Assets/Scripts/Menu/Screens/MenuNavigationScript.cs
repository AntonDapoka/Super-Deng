using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Menu.Screens
{
    public class MenuNavigationScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuBlockWallManagerScript blockWallManager;
        [SerializeField] private MenuPresenterScript menuPresenter;

        private MenuState menuStateCurrent = MenuState.Main;
        private MenuState menuStatePrevious = MenuState.Main;

        public event Action<MenuState, MenuState> OnMenuStateChanged;

        public MenuState GetMenuState()
        {
            return menuStateCurrent;
        }

        public Task NavigateTo(MenuState state)
        {
            menuStatePrevious = menuStateCurrent;
            menuStateCurrent = state;
            OnMenuStateChanged?.Invoke(menuStatePrevious, menuStateCurrent);
            
            blockWallManager.TurnOnBlockWall();

            switch (state)
            {
                case MenuState.Main:
                    return OpenMainAsync();

                case MenuState.StartingLevel:
                    return StartLevelAsync();

                case MenuState.LevelSelection:
                    return OpenLevelSelectionAsync();

                case MenuState.Settings:
                    return OpenSettingsAsync();

                case MenuState.Credits:
                    return OpenCreditsAsync();

                default:
                    Debug.Log("No match found");
                    blockWallManager.TurnOffBlockWall();
                    return Task.CompletedTask;
            }
        }
        
        private async Task OpenMainAsync()
        {
            await menuPresenter.ShowMenuStateAsync(MenuState.Main, menuStatePrevious);
            blockWallManager.TurnOffBlockWall();
        }

        private async Task StartLevelAsync()
        {
            await menuPresenter.HideEveryButtonAsync();
            blockWallManager.TurnOffBlockWall();
        }

        private async Task OpenLevelSelectionAsync()
        {
            await menuPresenter.ShowMenuStateAsync(MenuState.LevelSelection, menuStatePrevious);
            blockWallManager.TurnOffBlockWall();
        }

        private async Task OpenSettingsAsync()
        {
            await menuPresenter.ShowMenuStateAsync(MenuState.Settings, menuStatePrevious);
            blockWallManager.TurnOffBlockWall();
        }

        private async Task OpenCreditsAsync()
        {
            await menuPresenter.ShowMenuStateAsync(MenuState.Credits, menuStatePrevious);
            blockWallManager.TurnOffBlockWall();
        }
    }
}