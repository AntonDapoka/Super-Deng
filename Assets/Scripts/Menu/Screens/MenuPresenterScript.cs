using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuPresenterScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuAnimationManagerScript menuAnimationManager;
    [SerializeField] private MenuReferencesHolder referencesHolder;
    [SerializeField] private MenuCreditsScript menuCredits;

    private bool isSettingsPanelOpen;

    public async Task HideEveryButtonAsync()
    {
        await CloseSettingsPanelAsync();
        await menuAnimationManager.HideButtonsAsync(referencesHolder.GetButtonsCurrent());
    }

    public async Task ShowMenuStateAsync(MenuState stateCurrent, MenuState statePrevious)
    {
        switch (stateCurrent)
        {
            case MenuState.Main:
                await ShowMainStateAsync(statePrevious);
                break;
            case MenuState.LevelSelection:
                await ShowLevelSelectionStateAsync();
                break;
            case MenuState.Settings:
                await ShowSettingsStateAsync();
                break;
            case MenuState.Credits:
                await ShowCreditsStateAsync();
                break;
        }
    }

    private async Task ShowMainStateAsync(MenuState statePrevious)
    {
        switch (statePrevious)
        {
            case MenuState.LevelSelection:
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain());
                break;
            case MenuState.Settings:
                await Task.WhenAll(CloseSettingsPanelAsync(), ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain()));
                break;
            case MenuState.Credits:
                menuCredits.EndCredits();
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain());
                break;
        }
        
        referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsMain());
    }

    private async Task ShowLevelSelectionStateAsync()
    {
        await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsLevelSelection());
        referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsLevelSelection());
    }

    private async Task ShowSettingsStateAsync()
    {
        await Task.WhenAll(
            ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsSettings()),
            menuAnimationManager.ShowPanelAsync(referencesHolder.GetRectTransformSettings()));
        isSettingsPanelOpen = true;
        referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsSettings());
    }

    private async Task ShowCreditsStateAsync()
    {
        await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsCredits());
        referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsCredits());
    }

    private Task ChangeButtonsAsync(Button[] buttonsToHide, Button[] buttonsToShow)
    {
        return menuAnimationManager.ChangeButtonsAsync(buttonsToHide, buttonsToShow);
    }

    private Task CloseSettingsPanelAsync()
    {
        if (!isSettingsPanelOpen) return Task.CompletedTask;

        isSettingsPanelOpen = false;
        return menuAnimationManager.HidePanelAsync(referencesHolder.GetRectTransformSettings());
    }
}
