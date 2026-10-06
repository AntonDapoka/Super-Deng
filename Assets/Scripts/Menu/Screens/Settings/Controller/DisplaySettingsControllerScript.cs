using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Screens.Settings
{
    public class DisplaySettingsControllerScript : MonoBehaviour
    {
        [SerializeField] private SettingsInteractorScript settingsInteractor;

        [Header("Display")]
        [SerializeField] private Toggle toggleFullscreen;
        [SerializeField] private TMP_Dropdown dropdownResolution;

        [Header("Other")]
        [SerializeField] private TMP_Dropdown dropdownLanguage;
        [SerializeField] private TMP_Dropdown dropdownQuality;

        private void Awake()
        {
            dropdownResolution.onValueChanged.AddListener(index => settingsInteractor.ChangeResolution(index));
            toggleFullscreen.onValueChanged.AddListener(value => settingsInteractor.ChangeFullscreen(value));
            dropdownQuality.onValueChanged.AddListener(index => settingsInteractor.ChangeQuality(index));
            dropdownLanguage.onValueChanged.AddListener(index => settingsInteractor.ChangeLanguage(index));
        }
    }
}