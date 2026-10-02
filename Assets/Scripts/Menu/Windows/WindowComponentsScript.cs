using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WindowSettingsScript))]
[RequireComponent(typeof(WindowStateScript))]
public class WindowComponentsScript : MonoBehaviour
{
    private WindowSettingsScript windowSettings;
    private WindowStateScript windowState;
    [SerializeField] private RectTransform windowSpace;
    [SerializeField] private RectTransform windowSpaceInner;
    [SerializeField] private RectTransform windowTitleBar;
    [SerializeField] private TextMeshProUGUI windowTitleBarText;
    [SerializeField] private TextMeshProUGUI windowSpaceInnerText;
    [SerializeField] private Image iconButtonMinMax;
    [SerializeField] private Image iconButtonClose;
    [SerializeField] private Image imageButtonMinMax;
    [SerializeField] private Image imageButtonClose;

    
    public WindowSettingsScript GetWindowSettings() => windowSettings;
    public WindowStateScript GetWindowState() => windowState;
    public RectTransform GetWindowSpace() => windowSpace;
    public RectTransform GetWindowSpaceInner() => windowSpaceInner;
    public RectTransform GetWindowTitleBar() => windowTitleBar;
    public TextMeshProUGUI GetWindowTitleBarText() => windowTitleBarText;
    public TextMeshProUGUI GetWindowSpaceInnerText() => windowSpaceInnerText;
    public Image GetIconButtonMinMax() => iconButtonMinMax;
    public Image GetIconButtonClose() => iconButtonClose;
    public Image GetImageButtonMinMax() => imageButtonMinMax;
    public Image GetImageButtonClose() => imageButtonClose;

    private void Awake()
    {
        windowSettings = gameObject.GetComponent<WindowSettingsScript>();
        windowState = gameObject.GetComponent<WindowStateScript>();
    }
}
