using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu.Screens.Settings
{
    public class SettingsPresenterScript : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private VolumeSettingsViewScript volumeView;
        [SerializeField] private KeyBindingViewScript keyBindingView;
        [SerializeField] private DisplaySettingsViewScript displayView;

        [Header("Services")]
        [SerializeField] private AudioMixerSettingsApplierScript audioApplier;

        public void UpdateVolumeDisplay(VolumeChannel channel, float value)
        {
            audioApplier.ApplyVolume(channel, value);
            volumeView.SetText(channel, value.ToString());
        }

        public void ShowBinding(MovementDirection direction, Key key)
        {
            keyBindingView.ShowBinding(direction, key);
        }

        public void BeginRebindDisplay(MovementDirection direction)
        {
            keyBindingView.ShowCapturingState(direction);
            keyBindingView.SetAllInteractableExcept(direction, false);
        }

        public void EndRebindDisplay()
        {
            keyBindingView.SetAllInteractable(true);
        }

        public void PlayBindingError()
        {
            keyBindingView.PlayErrorSound();
        }

        public void ShowResolutions(List<string> options, int currentIndex)
        {
            displayView.SetResolutions(options, currentIndex);
        }

        public void SyncDisplayControls(bool isFullscreen, int quality, int language)
        {
            displayView.SetFullscreen(isFullscreen);
            displayView.SetQuality(quality);
            displayView.SetLanguage(language);
        }
    }
}
