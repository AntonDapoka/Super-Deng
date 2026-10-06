using System;
using UnityEngine;

namespace Menu.Screens.Settings
{
    public class VolumeSettingsInteractorScript
    {
        public event Action<VolumeChannel, float> OnVolumeChanged;

        private const int Step = 5;
        private const float MinValue = 0;
        private const int MaxValue = 100;
        private const int EasterEggPressesRequired = 15;
        private const float EasterEggValue = 999;

        private float volumeMaster = 100;
        private float volumeMusic = 100;
        private float volumeSFX = 100;
        private int easterEggCounter;

        public float GetVolume(VolumeChannel channel)
        {
            switch (channel)
            {
                case VolumeChannel.Master: return volumeMaster;
                case VolumeChannel.Music: return volumeMusic;
                default: return volumeSFX;
            }
        }

        public void Increase(VolumeChannel channel)
        {
            float newValue = Mathf.Clamp(GetVolume(channel) + Step, MinValue, MaxValue);

            if (channel == VolumeChannel.Music && newValue == MaxValue) // Easter egg
            {
                easterEggCounter++;
                if (easterEggCounter >= EasterEggPressesRequired) newValue = EasterEggValue;
            }

            SetVolume(channel, newValue);
        }

        public void Decrease(VolumeChannel channel)
        {
            if (channel == VolumeChannel.Music) easterEggCounter = 0;

            SetVolume(channel, Mathf.Clamp(GetVolume(channel) - Step, MinValue, MaxValue));
        }

        public void SetVolume(VolumeChannel channel, float value)
        {
            switch (channel)
            {
                case VolumeChannel.Master: volumeMaster = value; break;
                case VolumeChannel.Music: volumeMusic = value; break;
                default: volumeSFX = value; break;
            }

            OnVolumeChanged?.Invoke(channel, value);
        }
    }
}
