using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Screens.Settings
{
    public class DisplaySettingsViewScript : MonoBehaviour
    {
        [Header("Display")]
        [SerializeField] private Toggle toggleFullscreen;
        [SerializeField] private TMP_Dropdown dropdownResolution;

        [Header("Other")]
        [SerializeField] private TMP_Dropdown dropdownLanguage;
        [SerializeField] private TMP_Dropdown dropdownQuality;

        public void SetResolutions(List<string> options, int currentIndex)
        {
            dropdownResolution.ClearOptions();
            dropdownResolution.AddOptions(options);
            dropdownResolution.value = currentIndex;
            dropdownResolution.RefreshShownValue();
        }

        public void SetFullscreen(bool isFullscreen)
        {
            toggleFullscreen.isOn = isFullscreen;
        }

        public void SetQuality(int quality)
        {
            dropdownQuality.value = quality;
        }

        public void SetLanguage(int language)
        {
            dropdownLanguage.value = language;
        }
    }
}
