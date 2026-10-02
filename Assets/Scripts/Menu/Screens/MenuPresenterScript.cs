using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuPresenterScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuAnimationManagerScript menuAnimationManager;
    [SerializeField] private MenuReferencesHolder referencesHolder;

    private bool isSettingsPanelOpen;

    public Task HideEveryButtonAsync()
    {
        CloseSettingsPanel();
        return menuAnimationManager.HideButtonsAsync(referencesHolder.GetButtonsCurrent());
    }

    public async Task ShowMenuStateAsync(MenuState state)
    {
        CloseSettingsPanel();

        switch (state)
        {
            case MenuState.Main:
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain());
                referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsMain());
                break;

            case MenuState.LevelSelection:
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsLevelSelection());
                referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsLevelSelection());
                break;

            case MenuState.Settings:
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsSettings());
                menuAnimationManager.ShowPanel(referencesHolder.GetRectTransformSettings());
                isSettingsPanelOpen = true;
                referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsSettings());
                break;

            case MenuState.Credits:
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsCredits());
                referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsCredits());
                break;
        }
    }

    private Task ChangeButtonsAsync(Button[] buttonsToHide, Button[] buttonsToShow)
    {
        return menuAnimationManager.ChangeButtonsAsync(buttonsToHide, buttonsToShow);
    }

    private void CloseSettingsPanel()
    {
        if (!isSettingsPanelOpen) return;

        menuAnimationManager.HidePanel(referencesHolder.GetRectTransformSettings());
        isSettingsPanelOpen = false;
    }
}
