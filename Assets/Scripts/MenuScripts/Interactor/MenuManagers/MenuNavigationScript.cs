using UnityEngine;

public class MenuNavigationScript : MonoBehaviour
{
    [SerializeField] private MenuState menuStateCurrent;
    [Header("References")]
    [SerializeField] private MenuPresenterScript menuPresenter;

    public void NavigateTo(MenuState state)
    {
        menuStateCurrent = state;

        switch (state)
        {
            case MenuState.Main:
                OpenMain();
                break;
            
            case MenuState.StartingLevel:
                StartLevel();
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

    private void StartLevel()
    {
        //StartAnimations
        ClearMenu();
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

    private void ClearMenu()
    {
        menuPresenter.HideEveryButton();
    }

    public MenuState GetMenuState()
    {
        return menuStateCurrent;
    }
}