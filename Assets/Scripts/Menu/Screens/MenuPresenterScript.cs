using System.Threading.Tasks;
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

    public Task HideEveryButtonAsync()
    {
        return menuAnimationManager.HideButtonsAsync(referencesHolder.GetButtonsMain());
    }

    public Task ShowMenuStateAsync(MenuState state)
    {
        switch (state)
        {
            case MenuState.Main:
                return menuAnimationManager.ChangeButtonsAsync(
                    referencesHolder.GetButtonsCurrent(),
                    referencesHolder.GetButtonsMain());

            case MenuState.LevelSelection:
                return menuAnimationManager.ChangeButtonsAsync(
                    referencesHolder.GetButtonsMain(),
                    referencesHolder.GetButtonsLevelSelection());

            case MenuState.Settings:
                return ShowSettingsAsync();

            case MenuState.Credits:
                return menuAnimationManager.ChangeButtonsAsync(
                    referencesHolder.GetButtonsMain(),
                    referencesHolder.GetButtonsCredits());
            default:
                return Task.CompletedTask;
        }
    }

    private async Task ShowSettingsAsync()
    {
        Debug.Log("HERE");
        await menuAnimationManager.ChangeButtonsAsync(referencesHolder.GetButtonsMain(), referencesHolder.GetButtonsSettings());
        menuAnimationManager.ShowPanel(referencesHolder.GetRectTransformSettings());
        Debug.Log("THERE");
    }
}
