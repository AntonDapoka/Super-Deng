using UnityEngine;
using UnityEngine.Audio;

namespace Menu.Screens.Settings
{
    public class AudioMixerSettingsApplierScript : MonoBehaviour
    {
        [Header("Mixer")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private int decibelScale = 25;

        [Header("Mixer parameters")]
        [SerializeField] private string parameterMaster = "MasterVolume";
        [SerializeField] private string parameterMusic = "MusicVolume";
        [SerializeField] private string parameterSFX = "SFXVolume";

        private const float MinAudibleValue = 0.00001f;

        public void ApplyVolume(VolumeChannel channel, float value)
        {
            float mixerValue = value == 0 ? MinAudibleValue : value;
            audioMixer.SetFloat(GetParameter(channel), ToDecibels(mixerValue));
        }

        private float ToDecibels(float value)
        {
            return Mathf.Log10(value) * decibelScale - 45;
        }

        private string GetParameter(VolumeChannel channel)
        {
            switch (channel)
            {
                case VolumeChannel.Master: return parameterMaster;
                case VolumeChannel.Music: return parameterMusic;
                default: return parameterSFX;
            }
        }
    }
}
