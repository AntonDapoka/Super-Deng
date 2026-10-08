using System.Threading.Tasks;
using Menu.Effects.Flickering.Logo;
using Menu.Screens.LevelSelection;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Screens
{
    public class MenuPresenterScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuAnimationManagerScript menuAnimationManager;
        [SerializeField] private MenuReferenceHolderScript referencesHolder;
        [SerializeField] private MenuLogoPresenterScript logoNeonPresenter;
        [SerializeField] private LevelSelectionPresenterScript levelSelectionPresenter;

        private bool isLevelSelectionOpen;
        private bool isSettingsOpen;

        public async Task HideEveryButtonAsync()
        {
            await CloseSettingsAsync();
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
                    await Task.WhenAll(ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain()), CloseLevelSelectionAsync());
                    break;
                case MenuState.Settings:
                    await Task.WhenAll(CloseSettingsAsync(), ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain()));
                    break;
                case MenuState.Credits:
                    await ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsMain());
                    break;
            }
            
            referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsMain());
        }

        private async Task ShowLevelSelectionStateAsync()
        {
            logoNeonPresenter.TurnOff(isChangingIcon : false, isDisappearMode:true);
            levelSelectionPresenter.ShowButtons();
            levelSelectionPresenter.ShowLevelIcons();
            await Task.WhenAll(
                ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsLevelSelection()),
                menuAnimationManager.ShowPanelAsync(referencesHolder.GetRectTransformLevelDescription()));
 
            isLevelSelectionOpen = true;
            referencesHolder.SetButtonsCurrent(referencesHolder.GetButtonsLevelSelection());
        }

        private async Task ShowSettingsStateAsync()
        {
            logoNeonPresenter.TurnOff(isFlickerTriangle : true);
            await Task.WhenAll(
                ChangeButtonsAsync(referencesHolder.GetButtonsCurrent(), referencesHolder.GetButtonsSettings()),
                menuAnimationManager.ShowPanelAsync(referencesHolder.GetRectTransformSettings()));
            isSettingsOpen = true;
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

        private Task CloseLevelSelectionAsync()
        {
            if (!isLevelSelectionOpen) return Task.CompletedTask;
            logoNeonPresenter.TurnOn(isChangingIcon : false);
            levelSelectionPresenter.HideButtons();
            levelSelectionPresenter.HideSideLevelIcons();
            isLevelSelectionOpen = false;
            return menuAnimationManager.HidePanelAsync(referencesHolder.GetRectTransformLevelDescription());
        }

        private Task CloseSettingsAsync()
        {
            if (!isSettingsOpen) return Task.CompletedTask;
            logoNeonPresenter.TurnOn();
            isSettingsOpen = false;
            return menuAnimationManager.HidePanelAsync(referencesHolder.GetRectTransformSettings());
        }
    }
}