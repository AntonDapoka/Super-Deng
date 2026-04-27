using TMPro;
using UnityEngine;

[RequireComponent(typeof(WindowSettingsScript))]
public class WindowComponentsScript : MonoBehaviour
{
    private WindowSettingsScript windowSettings;
    [SerializeField] private RectTransform windowSpace;
    [SerializeField] private RectTransform windowSpaceInner;
    [SerializeField] private RectTransform windowTitleBar;
    [SerializeField] private TextMeshProUGUI windowTitleBarText;
    [SerializeField] private TextMeshProUGUI windowSpaceInnerText;
    
    public WindowSettingsScript GetWindowSettings() => windowSettings;
    public RectTransform GetWindowSpace() => windowSpace;
    public RectTransform GetWindowSpaceInner() => windowSpaceInner;
    public RectTransform GetWindowTitleBar() => windowTitleBar;
    public TextMeshProUGUI GetWindowTitleBarText() => windowTitleBarText;
    public TextMeshProUGUI GetWindowSpaceInnerText() => windowSpaceInnerText;

    private void Awake()
    {
        windowSettings = gameObject.GetComponent<WindowSettingsScript>();
    }
}
