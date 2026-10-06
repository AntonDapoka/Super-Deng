using UnityEngine;

namespace Menu.Screens.Main
{
    public class BackToMainInteractorScript : MonoBehaviour
    {
        [Header("Presenter")]
        [SerializeField] private MenuNavigationScript menuNavigation;

        public void BackToMain()
        {
            _ = menuNavigation.NavigateTo(MenuState.Main);
        }
    }
}