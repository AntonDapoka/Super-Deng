using System;
using UnityEngine;

namespace Menu.Screens.Credits
{
    [CreateAssetMenu(fileName = "MenuCreditsSettings", menuName = "ScriptableObjects/MenuCreditsSettings")]
    public class MenuCreditsSettings : ScriptableObject
    {
        [Header("Camera")]
        public float speedCamera = 0.7f;
        public float durationCameraSpeedUp = 1.5f;
        public float durationCameraReturn = 1.5f;
        public float thresholdCameraArrival = 0.001f;
        public AnimationCurve curveSpeedByDistanceToTarget = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        [Header("Timings")]
        public float durationLine = 1.2f;
        public float durationModifierWordsFadeOut = 0.25f;
        public float durationModifierFinalWord = 0.5f;

        [Header("Logo")]
        public float durationLogoTurnOff = 0.75f;
        public float durationLogoTurnOn = 0.75f;

        [Header("Word delays")]
        public float delayFirstLineNextWordMin = 0.7f;
        public float delayFirstLineNextWordMax = 1.5f;

        public float delayOtherLinesNextWordMin = 0.35f;
        public float delayOtherLinesNextWordMax = 0.5f;

        [Header("Line time modifiers")]
        public MenuCreditsLineTimeModifier[] lineTimeModifiers;

        [Header("Word color curve")]
        public AnimationCurve curveColorChangeTurnOn = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Glitch chances")]
        public int maxGlitchRoll = 100;
        public int rollSingleInterruptionMin = 40;
        public int rollDoubleInterruptionMin = 87;

        [Header("Glitch timing")]
        public float shareTimeSingleInterruptionMin = 0.2f; //Minimum proportion of first-stop time for a single interruption
        public float shareTimeSingleInterruptionMax = 0.8f;
        public float shareTimeDoubleInterruptionFirstStopMin = 0.22f;
        public float shareTimeDoubleInterruptionFirstStopMax = 0.45f;
        public float shareTimeDoubleInterruptionSecondStopExtraMin = 1.0005f; //Minimum multiplier (>1) of the remaining time for the second stop.
        public float shareTimeDoubleInterruptionSecondStopMax = 0.9f; //Maximum proportion of the second stop duration in the case of a double interruption.

        private void OnValidate()
        {
            speedCamera = Mathf.Max(0f, speedCamera);
            durationCameraSpeedUp = Mathf.Max(0f, durationCameraSpeedUp);
            durationCameraReturn = Mathf.Max(0f, durationCameraReturn);
            durationLine = Mathf.Max(0.01f, durationLine);

            maxGlitchRoll = Mathf.Max(1, maxGlitchRoll);
            rollSingleInterruptionMin = Mathf.Clamp(rollSingleInterruptionMin, 0, maxGlitchRoll);
            rollDoubleInterruptionMin = Mathf.Clamp(rollDoubleInterruptionMin, rollSingleInterruptionMin, maxGlitchRoll);

            shareTimeSingleInterruptionMin = Mathf.Clamp01(shareTimeSingleInterruptionMin);
            shareTimeSingleInterruptionMax = Mathf.Clamp(shareTimeSingleInterruptionMax, shareTimeSingleInterruptionMin, 1f);
            shareTimeDoubleInterruptionFirstStopMin = Mathf.Clamp01(shareTimeDoubleInterruptionFirstStopMin);
            shareTimeDoubleInterruptionFirstStopMax = Mathf.Clamp(shareTimeDoubleInterruptionFirstStopMax, shareTimeDoubleInterruptionFirstStopMin, 1f);
            shareTimeDoubleInterruptionSecondStopExtraMin = Mathf.Max(1f, shareTimeDoubleInterruptionSecondStopExtraMin);
            shareTimeDoubleInterruptionSecondStopMax = Mathf.Clamp01(shareTimeDoubleInterruptionSecondStopMax);
        }
    }

    [Serializable]
    public class MenuCreditsLineTimeModifier
    {
        public int lineIndex;
        public float timeModifier;
    }
}

    