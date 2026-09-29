using UnityEngine;

public class MenuNavigationScript : MonoBehaviour
{
    [SerializeField] private MenuPresenterScript menuPresenter;

    public void NavigateTo(MenuState state)
    {
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
        
    }
}
