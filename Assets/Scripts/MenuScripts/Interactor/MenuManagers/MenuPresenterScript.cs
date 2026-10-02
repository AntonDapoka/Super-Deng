using UnityEngine;

public class MenuPresenterScript : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private CreditsButtonViewScript
    [SerializeField] private MenuAnimationManagerScript menuAnimationManager;
    [SerializeField] private MenuReferencesHolder referencesHolder;

    public void HideMainButtons()
    {
        referencesHolder.GetButtonsMain();
    }

    public void HideEveryButton()
    {
        menuAnimationManager.HideButtons(referencesHolder.GetButtonsMain());
    }

    public void ShowMenuState(MenuState state)
    {
        switch (state)
        {
            case MenuState.Main:
                
                break;

            case MenuState.LevelSelection:
                menuAnimationManager.ChangeButtons(referencesHolder.GetButtonsMain(), referencesHolder.GetButtonsLevelSelection());
                break;

            case MenuState.Settings:
                menuAnimationManager.ChangeButtons(referencesHolder.GetButtonsMain(), referencesHolder.GetButtonsSettings());
                menuAnimationManager.ShowPanel(referencesHolder.GetRectTransformSettings());
                break;

            case MenuState.Credits:
                //OpenCredits();
                break;

            default:
                Debug.Log("No match found");
                //OpenMain();
                break;
        }
    }
}
