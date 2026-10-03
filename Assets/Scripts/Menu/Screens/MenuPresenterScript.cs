using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuPresenterScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuAnimationManagerScript menuAnimationManager;
    [SerializeField] private MenuReferencesHolder referencesHolder;

    private bool isSettingsPanelOpen;

    public async Task HideEveryButtonAsync()
    {
        await CloseSettingsPanelAsync();
        await menuAnimationManager.HideButtonsAsync(referencesHolder.GetButtonsCurrent());
    }

    public async Task ShowMenuStateAsync(MenuState state)
    {
        //        await CloseSettingsPanelAsync();

        switch (state)
        {
            case MenuState.Main:
                await Task.WhenAll(CloseSettingsPanelAsync(), ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain()));
                referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsMain());
                break;

            case MenuState.LevelSelection:
                await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsLevelSelection());
                referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsLevelSelection());
                break;

            case MenuState.Settings:
                await Task.WhenAll(
                    ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsSettings()),
                    menuAnimationManager.ShowPanelAsync(referencesHolder.GetRectTransformSettings()));
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

    private Task CloseSettingsPanelAsync()
    {
        if (!isSettingsPanelOpen) return Task.CompletedTask;

        isSettingsPanelOpen = false;
        return menuAnimationManager.HidePanelAsync(referencesHolder.GetRectTransformSettings());
    }
}
