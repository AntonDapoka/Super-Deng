using UnityEngine;
using UnityEngine.UI;

public class MenuReferencesHolder : MonoBehaviour
{
    [Header("Button Group")]
    [SerializeField] private Button[] buttonsMain;
    [SerializeField] private Button[] buttonsLevelSelection;
    [SerializeField] private Button[] buttonsSettings;
    [SerializeField] private Button[] buttonsCredits;

    [Header("Images")]
    [SerializeField] private RectTransform imageSettings;
    [SerializeField] private RectTransform imageLevelDescription;

    private Button[] buttonsCurrent;

    private void Start()
    {
        buttonsCurrent = buttonsMain;
    }

    public void SetButtonsCurrent(Button[] buttons)
    {
        buttonsCurrent = buttons;
    }

    public Button[] GetButtonsCurrent()
    {
        return buttonsCurrent;
    }

    public Button[] GetButtonsMain()
    {
        return buttonsMain;
    }

    public Button[] GetButtonsLevelSelection()
    {
        return buttonsLevelSelection;
    }

    public Button[] GetButtonsSettings()
    {
        return buttonsSettings;
    }

    public Button[] GetButtonsCredits()
    {
        return buttonsCredits;
    }

    public RectTransform GetRectTransformSettings()
    {
        return imageSettings;
    }

    public RectTransform GetRectTransformLevelDescription()
    {
        return imageLevelDescription;
    }
}
