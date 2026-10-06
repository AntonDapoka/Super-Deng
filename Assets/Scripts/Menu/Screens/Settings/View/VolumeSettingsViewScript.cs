using TMPro;
using UnityEngine;

namespace Menu.Screens.Settings
{
    public class VolumeSettingsViewScript : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI textMaster;
        [SerializeField] private TextMeshProUGUI textMusic;
        [SerializeField] private TextMeshProUGUI textSFX;

        public void SetText(VolumeChannel channel, string text)
        {
            switch (channel)
            {
                case VolumeChannel.Master: textMaster.text = text; break;
                case VolumeChannel.Music: textMusic.text = text; break;
                default: textSFX.text = text; break;
            }
        }
    }
}
