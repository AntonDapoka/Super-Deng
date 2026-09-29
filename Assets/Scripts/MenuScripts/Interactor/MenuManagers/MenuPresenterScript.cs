using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPresenterScript : MonoBehaviour
{
    [SerializeField] private MenuAnimationManagerScript menuAnimationManager;

    [Header("Main Buttons")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonLevel;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonCredits;

    [Header("Other Buttons")]
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonChoose;
    [SerializeField] private Button buttonSettingsSave;
    [SerializeField] private Button buttonSettingsCorrection;
    [SerializeField] private Button buttonSavingsPlay;
    [SerializeField] private Button buttonSavingsDelete;
    [SerializeField] private Button buttonCreditsContact;

    [Header("Images")]
    [SerializeField] private Image imageSavings;
    [SerializeField] private Image imageSettings;
    [SerializeField] private Image imageLevelDescription;
    [SerializeField] private Image panel;
    [SerializeField] private Image wall;

    private List<Button> buttonsAll;

    private void Start()
    {
        buttonsAll = new List<Button>
        {
            buttonStart,
            buttonLevel,
            buttonSettings,
            buttonCredits,

            buttonBack,
            buttonChoose,
            buttonSettingsSave,
            buttonSettingsCorrection,
            buttonSavingsPlay,
            buttonSavingsDelete,
            buttonCreditsContact
        };
    }

    public void HideMainButtons()
    {
        
    }

    public void HideEveryButton()
    {
        menuAnimationManager.HideButtons(buttonsAll.ToArray());
    }

    public void ShowMenuState(MenuState state)
    {
        /*показать Settings image
            ↓
        показать Settings buttons*/
    }
}
