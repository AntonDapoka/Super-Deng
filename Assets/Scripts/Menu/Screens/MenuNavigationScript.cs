using System.Threading.Tasks;
using UnityEngine;

public class MenuNavigationScript : MonoBehaviour
{
    [SerializeField] private MenuState menuStateCurrent;
    [Header("References")]
    [SerializeField] private MenuBlockWallManagerScript blockWallManager;
    [SerializeField] private MenuPresenterScript menuPresenter;
    

    public Task NavigateTo(MenuState state)
    {
        menuStateCurrent = state;
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
        await menuPresenter.ShowMenuStateAsync(MenuState.Main);
        blockWallManager.TurnOffBlockWall();
    }

    private async Task StartLevelAsync()
    {
        await menuPresenter.HideEveryButtonAsync();
        blockWallManager.TurnOffBlockWall();
    }

    private async Task OpenLevelSelectionAsync()
    {
        await menuPresenter.ShowMenuStateAsync(MenuState.LevelSelection);
        blockWallManager.TurnOffBlockWall();
    }

    private async Task OpenSettingsAsync()
    {
        await menuPresenter.ShowMenuStateAsync(MenuState.Settings);
        blockWallManager.TurnOffBlockWall();
    }

    private async Task OpenCreditsAsync()
    {
        await menuPresenter.ShowMenuStateAsync(MenuState.Credits);
        blockWallManager.TurnOffBlockWall();
    }

    public MenuState GetMenuState()
    {
        return menuStateCurrent;
    }
}
