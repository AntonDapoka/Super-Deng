using UnityEngine;

public class ChooseLevelInteractorScript : MonoBehaviour
{
    [SerializeField] private MenuNavigationScript menuNavigation;

    public void StartToChooseLevel()
    {
        menuNavigation.NavigateTo(MenuState.LevelSelection);
    }
}
