using TMPro;
using UnityEngine;

namespace Menu.Screens.Settings
{
    public class VolumeSettingsViewScript : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI masterText;
        [SerializeField] private TextMeshProUGUI musicText;
        [SerializeField] private TextMeshProUGUI sfxText;

        public void SetText(VolumeChannel channel, string text)
        {
            switch (channel)
            {
                case VolumeChannel.Master: masterText.text = text; break;
                case VolumeChannel.Music: musicText.text = text; break;
                default: sfxText.text = text; break;
            }
        }
    }
}
