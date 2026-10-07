using UnityEngine;

namespace Menu.Effects.Flickering
{
    public readonly struct FlickeringRequestScript
    {
        public Component Target { get; }
        public FlickeringSettings Settings { get; }
        public Color InitialColor { get; }
        public Color TargetColor { get; }
        public float Duration { get; }
        public bool IsTurningOn { get; }
        public bool IsBlinking { get; }
        public bool DeactivateTargetAfter { get; }

        public FlickeringRequestScript(Component target, FlickeringSettings settings, Color initialColor, Color targetColor, float duration, bool isTurningOn, bool isBlinking, bool deactivateTargetAfter = false)
        {
            Target = target;
            Settings = settings;
            InitialColor = initialColor;
            TargetColor = targetColor;
            Duration = duration;
            IsTurningOn = isTurningOn;
            IsBlinking = isBlinking;
            DeactivateTargetAfter = deactivateTargetAfter;
        }
    }
}
