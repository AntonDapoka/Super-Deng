using System;
using UnityEngine;

namespace Menu.Screens.Credits
{
    [CreateAssetMenu(fileName = "MenuCreditsSettings", menuName = "ScriptableObjects/MenuCreditsSettings")]
    public class MenuCreditsSettings : ScriptableObject
    {
        [Header("Camera")]
        [Tooltip("Постоянная скорость движения камеры вниз во время титров.")]
        public float cameraSpeed = 0.7f;

        [Tooltip("Время разгона камеры от нуля до полной скорости.")]
        public float durationCameraSpeedUp = 1.5f;

        [Tooltip("Время возврата камеры в исходную позицию при выходе из титров.")]
        public float durationCameraReturn = 1.5f;

        [Tooltip("Расстояние, ниже которого камера считается достигшей цели.")]
        public float thresholdCameraArrival = 0.001f;

        [Tooltip("Кривая скорости возврата камеры: нормированное оставшееся расстояние -> множитель скорости.")]
        public AnimationCurve curveSpeedByDistanceToTarget = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        [Header("Timings")]
        [Tooltip("Базовое время показа строки титров (до модификаторов).")]
        public float durationLine = 1.2f;

        [Tooltip("Множитель времени финальной строки после последнего слова.")]
        public float durationModifierFinalWord = 0.5f;

        [Tooltip("Множитель времени затухания слов при выходе (от durationLine).")]
        public float durationModifierWordsFadeOut = 0.25f;

        [Header("Logo")]
        [Tooltip("Время выключения логотипа в начале титров.")]
        public float durationLogoTurnOff = 0.75f;

        [Tooltip("Время включения логотипа.")]
        public float durationLogoTurnOn = 0.75f;

        [Header("Word delays (множители timeForWord)")]
        [Tooltip("Минимальная задержка перед следующим словом первой строки.")]
        public float delayFirstLineNextWordMin = 0.7f;

        [Tooltip("Максимальная задержка перед следующим словом первой строки.")]
        public float delayFirstLineNextWordMax = 1.5f;

        [Tooltip("Минимальная задержка перед следующим словом остальных строк.")]
        public float delayOtherLinesNextWordMin = 0.35f;

        [Tooltip("Максимальная задержка перед следующим словом остальных строк.")]
        public float delayOtherLinesNextWordMax = 0.5f;

        [Header("Line time modifiers")]
        public MenuCreditsLineTimeModifier[] lineTimeModifiers;

        [Header("Word color curve")]
        [Tooltip("Кривая затухания слова: нормированное время сегмента -> прогресс лерпа цвета.")]
        public AnimationCurve curveColorChangeTurnOn = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Glitch chances (roll of 0..maxGlitchRoll)")]
        [Tooltip("Верхняя граница ролла.")]
        public int maxGlitchRoll = 100;

        [Tooltip("Минимальный ролл одиночного прерывания затухания.")]
        public int rollSingleInterruptionMin = 40;

        [Tooltip("Минимальный ролл двойного прерывания затухания.")]
        public int rollDoubleInterruptionMin = 87;

        [Header("Glitch timing (доли/множители durationLine)")]
        [Tooltip("Минимальная доля времени первой остановки при одиночном прерывании.")]
        public float shareTimeSingleInterruptionMin = 0.2f;

        [Tooltip("Максимальная доля времени первой остановки при одиночном прерывании.")]
        public float shareTimeSingleInterruptionMax = 0.8f;

        [Tooltip("Минимальная доля времени первой остановки при двойном прерывании.")]
        public float shareTimeDoubleInterruptionFirstStopMin = 0.22f;

        [Tooltip("Максимальная доля времени первой остановки при двойном прерывании.")]
        public float shareTimeDoubleInterruptionFirstStopMax = 0.45f;

        [Tooltip("Минимальный множитель (>1) оставшегося времени для второй остановки.")]
        public float shareTimeDoubleInterruptionSecondStopExtraMin = 1.0005f;

        [Tooltip("Максимальная доля времени второй остановки при двойном прерывании.")]
        public float shareTimeDoubleInterruptionSecondStopMax = 0.9f;

        private void OnValidate()
        {
            cameraSpeed = Mathf.Max(0f, cameraSpeed);
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

    