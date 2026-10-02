using System.Threading.Tasks;
using UnityEngine;

public class StartLevelInteractorScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuSceneLoaderScript sceneLoader;
    [Header("Presenter")]
    [SerializeField] private MenuNavigationScript menuNavigation;
    [SerializeField] private StartLevelAnimationManagerScript animationManager;

    public async Task StartLevelAsync()
    {
        await menuNavigation.NavigateTo(MenuState.StartingLevel);
        await animationManager.StartTransitionAsync();
        sceneLoader.LoadSceneByIndex(1);
    }
}
