using System.Threading.Tasks;
using UnityEngine;

public class OpenSettingsInteractorScript : MonoBehaviour
{    
    [Header("Presenter")]
    [SerializeField] private MenuNavigationScript menuNavigation;

    public async Task OpenSettingsAsync()
    {
        await menuNavigation.NavigateTo(MenuState.Settings);
    }
}
