using UnityEngine;

public class StartLevelInteractorScript : MonoBehaviour
{
    [SerializeField] private MenuNavigationScript menuNavigation;

    public void StartLevel()
    {
        menuNavigation.ClearMenu();
    }
}
