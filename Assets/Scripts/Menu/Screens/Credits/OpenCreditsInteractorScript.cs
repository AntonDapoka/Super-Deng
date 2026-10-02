using System.Threading.Tasks;
using UnityEngine;

public class OpenCreditsInteractorScript : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private MenuCreditsScript menuCredits;
    [SerializeField] private MenuNavigationScript menuNavigation;

    public async Task OpenCreditsAsync()
    {
        await menuNavigation.NavigateTo(MenuState.Credits);
        menuCredits.StartCredits();
    }
}
