using System.Threading.Tasks;
using UnityEngine;

public class MenuNavigationScript : MonoBehaviour
{
    [SerializeField] private MenuState menuStateCurrent;
    [Header("References")]
    [SerializeField] private MenuPresenterScript menuPresenter;

    public Task NavigateTo(MenuState state)
    {
        menuStateCurrent = state;

        switch (state)
        {
            case MenuState.Main:
                return OpenMainAsync();

            case MenuState.StartingLevel:
                return StartLevelAsync();

            case MenuState.LevelSelection:
                return menuPresenter.ShowMenuStateAsync(MenuState.LevelSelection);

            case MenuState.Settings:
                return OpenSettingsAsync();

            case MenuState.Credits:
                return OpenCreditsAsync();

            default:
                Debug.Log("No match found");
                return Task.CompletedTask;
        }
    }
    
    private async Task OpenMainAsync()
    {
        await menuPresenter.ShowMenuStateAsync(MenuState.Main);
    }

    private async Task StartLevelAsync()
    {
        await menuPresenter.HideEveryButtonAsync();
    }

    private async Task OpenSettingsAsync()
    {
        await menuPresenter.ShowMenuStateAsync(MenuState.Settings);
    }

    private async Task OpenCreditsAsync()
    {
        await menuPresenter.ShowMenuStateAsync(MenuState.Credits);
    }

    public MenuState GetMenuState()
    {
        return menuStateCurrent;
    }
}
