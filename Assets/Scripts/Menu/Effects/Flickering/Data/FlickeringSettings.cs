using UnityEngine;

namespace Menu.Effects.Flickering
{
    [CreateAssetMenu(fileName = "FlickeringSettings", menuName = "Menu/Effects/Flickering Settings")]
    public class FlickeringSettings : ScriptableObject
    {
        [Header("Color Curves")]
        [SerializeField] private AnimationCurve colorCurveTurnOn = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private AnimationCurve colorCurveTurnOff = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Blink Interruption")]
        [SerializeField, Range(0f, 1f), Tooltip("Chance that the animation is interrupted midway and restarts (neon blinking).")]
        private float blinkChance = 0.5f;
        [SerializeField, Tooltip("Interruption point range as a fraction of duration when turning on.")]
        private Vector2 interruptionTimeRangeTurnOn = new Vector2(0.4f, 0.8f);
        [SerializeField, Tooltip("Interruption point range as a fraction of duration when turning off.")]
        private Vector2 interruptionTimeRangeTurnOff = new Vector2(0.2f, 0.6f);
        [SerializeField, Range(0f, 1f), Tooltip("Alpha multiplier applied to the initial color when the animation restarts after a blink.")]
        private float blinkRestartAlphaFactor = 1f;

        public AnimationCurve ColorCurveTurnOn => colorCurveTurnOn;
        public AnimationCurve ColorCurveTurnOff => colorCurveTurnOff;
        public float BlinkChance => blinkChance;
        public Vector2 InterruptionTimeRangeTurnOn => interruptionTimeRangeTurnOn;
        public Vector2 InterruptionTimeRangeTurnOff => interruptionTimeRangeTurnOff;
        public float BlinkRestartAlphaFactor => blinkRestartAlphaFactor;
    }
}
