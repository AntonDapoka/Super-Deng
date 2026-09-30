using UnityEngine;

public class MenuNavigationScript : MonoBehaviour
{
    [SerializeField] private MenuState menuStateCurrent;
    [SerializeField] private MenuPresenterScript menuPresenter;

    public void NavigateTo(MenuState state)
    {
        menuStateCurrent = state;

        switch (state)
        {
            case MenuState.Main:
                OpenMain();
                break;

            case MenuState.LevelSelection:
                OpenLevelSelection();
                break;

            case MenuState.Settings:
                OpenSettings(state);
                break;

            case MenuState.Credits:
                OpenCredits();
                break;

            default:
                Debug.Log("No match found");
                OpenMain();
                break;
        }
    }

    private void OpenMain()
    {
        
    }
    
    private void OpenLevelSelection()
    {
        menuPresenter.ShowMenuState(MenuState.LevelSelection);
    }
    
    private void OpenSettings(MenuState state)
    {
        menuPresenter.HideMainButtons();

        menuPresenter.ShowMenuState(state);

    }
    
    private void OpenCredits()
    {
        
    }

    public void ClearMenu()
    {
        menuPresenter.HideEveryButton();
    }

    public MenuState GetMenuState()
    {
        return menuStateCurrent;
    }
}