using System.Threading.Tasks;
using UnityEngine;

public class BackToMainInteractorScript : MonoBehaviour
{
    [Header("Presenter")]
    [SerializeField] private MenuNavigationScript menuNavigation;

    public void BackToMain()
    {
        _ = menuNavigation.NavigateTo(MenuState.Main);
    }
}
