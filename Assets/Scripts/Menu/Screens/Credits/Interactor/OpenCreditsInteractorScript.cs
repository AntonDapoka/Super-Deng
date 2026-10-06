using System.Threading.Tasks;
using UnityEngine;

namespace Menu.Screens.Credits
{
    public class OpenCreditsInteractorScript : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private MenuCreditsInteractorScript creditsInteractor;
        [SerializeField] private MenuNavigationScript navigation;

        public async Task OpenCreditsAsync()
        {
            await navigation.NavigateTo(MenuState.Credits);
            creditsInteractor.StartCredits();
        }
    }
}