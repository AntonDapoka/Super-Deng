using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu.Screens.Settings
{
    public class SettingsPresenterScript : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private VolumeSettingsViewScript viewVolume;
        [SerializeField] private KeyBindingViewScript viewKeyBinding;
        [SerializeField] private DisplaySettingsViewScript viewDisplay;

        [Header("Services")]
        [SerializeField] private AudioMixerSettingsApplierScript audioApplier;

        public void UpdateVolumeDisplay(VolumeChannel channel, float value)
        {
            audioApplier.ApplyVolume(channel, value);
            viewVolume.SetText(channel, value.ToString());
        }

        public void ShowBinding(MovementDirection direction, Key key)
        {
            viewKeyBinding.ShowBinding(direction, key);
        }

        public void BeginRebindDisplay(MovementDirection direction)
        {
            viewKeyBinding.ShowCapturingState(direction);
            viewKeyBinding.SetAllInteractableExcept(direction, false);
        }

        public void EndRebindDisplay()
        {
            viewKeyBinding.SetAllInteractable(true);
        }

        public void PlayBindingError()
        {
            viewKeyBinding.PlayErrorSound();
        }

        public void ShowResolutions(List<string> options, int currentIndex)
        {
            viewDisplay.SetResolutions(options, currentIndex);
        }

        public void SyncDisplayControls(bool isFullscreen, int quality, int language)
        {
            viewDisplay.SetFullscreen(isFullscreen);
            viewDisplay.SetQuality(quality);
            viewDisplay.SetLanguage(language);
        }
    }
}
