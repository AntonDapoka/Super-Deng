using System.Threading.Tasks;
using UnityEngine;

namespace Menu.Screens.LevelStart
{
    public class StartLevelInteractorScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuSceneLoaderScript sceneLoader;
        [Header("Presenter")]
        [SerializeField] private MenuNavigationScript menuNavigation;
        [SerializeField] private StartLevelAnimationScript animationManager;

        public async Task StartLevelAsync()
        {
            await menuNavigation.NavigateTo(MenuState.StartingLevel);
            await animationManager.StartTransitionAsync();
            sceneLoader.LoadSceneByIndex(1);
        }
    }
}