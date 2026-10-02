using UnityEngine;

public class StartLevelInteractorScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuSceneLoaderScript sceneLoader;
    [Header("Presenter")]
    [SerializeField] private MenuNavigationScript menuNavigation;
    [SerializeField] private StartLevelAnimationManagerScript animationManager;

    public void StartLevel()
    {
        menuNavigation.NavigateTo(MenuState.StartingLevel);
        //Get Response and then do the next action

        animationManager.StartTransition();
        //Get Response and then do the next action

        sceneLoader.LoadSceneByIndex(1);
    }
}
