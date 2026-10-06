using System.Collections.Generic;
using UnityEngine;

namespace Menu.Screens.Settings
{
    public class DisplaySettingsInteractorScript
    {
        private Resolution[] resolutions;
        private int currentResolutionIndex;

        public int ResolutionIndex { get; set; }
        public int CurrentResolutionIndex => currentResolutionIndex;
        public bool IsFullscreen { get; private set; }
        public int Quality { get; set; }
        public int Language { get; set; }

        public List<string> BuildResolutionOptions()
        {
            resolutions = Screen.resolutions;
            currentResolutionIndex = 0;

            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                options.Add(resolutions[i].width + "x" + resolutions[i].height);
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            return options;
        }

        public void ApplyResolution(int index)
        {
            if (resolutions == null || index < 0 || index >= resolutions.Length) return;

            ResolutionIndex = index;
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            IsFullscreen = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }

        public void SetQuality(int qualityIndex)
        {
            Quality = qualityIndex;
        }
    }
}
