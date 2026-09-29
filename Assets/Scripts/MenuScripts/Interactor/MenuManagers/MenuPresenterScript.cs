using UnityEngine;
using UnityEngine.UI;

public class MenuPresenterScript : MonoBehaviour
{
    [SerializeField] private MenuAnimationManagerScript menuAnimationManager;

    public Button StartButton;
    public Button LevelButton;
    public Button SettingsButton;
    public Button CreditsButton;

    public Button BackButton;
    public Button ChooseButton;

    public Image SavingsImage;
    public Image SettingsImage;
    public Image LevelDescription;
    public Image Panel;
    public Image Wall;

    public void HideMainButtons()
    {
        
    }

    public void ShowMenuState(MenuState state)
    {
        /*показать Settings image
            ↓
        показать Settings buttons*/
    }
}
