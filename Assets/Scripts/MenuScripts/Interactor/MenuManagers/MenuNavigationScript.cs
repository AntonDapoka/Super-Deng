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
                return Task.CompletedTask;

            case MenuState.StartingLevel:
                return StartLevelAsync();

            case MenuState.LevelSelection:
                return menuPresenter.ShowMenuStateAsync(MenuState.LevelSelection);

            case MenuState.Settings:
                return OpenSettingsAsync();

            case MenuState.Credits:
                return Task.CompletedTask;

            default:
                Debug.Log("No match found");
                return Task.CompletedTask;
        }
    }

    private async Task StartLevelAsync()
    {
        await menuPresenter.HideEveryButtonAsync();
    }

    private async Task OpenSettingsAsync()
    {
        menuPresenter.HideMainButtons();
        await menuPresenter.ShowMenuStateAsync(MenuState.Settings);
    }

    public MenuState GetMenuState()
    {
        return menuStateCurrent;
    }
}
