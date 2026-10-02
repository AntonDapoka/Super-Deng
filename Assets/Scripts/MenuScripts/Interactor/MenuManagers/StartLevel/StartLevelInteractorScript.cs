using UnityEngine;

public class StartLevelInteractorScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MenuSceneLoaderScript sceneLoader;
    [Header("Presenter")]
    [SerializeField] private MenuNavigationScript menuNavigation;

    public void StartLevel()
    {
        menuNavigation.NavigateTo(MenuState.StartingLevel);

        //Get Response

        sceneLoader.LoadSceneByIndex(1);
    }
}
