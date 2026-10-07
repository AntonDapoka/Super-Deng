using System.Collections.Generic;
using UnityEngine;

namespace Menu.Effects.Flickering
{
    public readonly struct FlickeringPhaseScript
    {
        public Color StartColor { get; }
        public AnimationCurve Curve { get; }
        public float Duration { get; }

        public FlickeringPhaseScript(Color startColor, AnimationCurve curve, float duration)
        {
            StartColor = startColor;
            Curve = curve;
            Duration = duration;
        }
    }

    public sealed class FlickeringPlanScript
    {
        public IReadOnlyList<FlickeringPhaseScript> Phases { get; }
        public Color TargetColor { get; }
        public bool DeactivateTargetAfter { get; }

        public FlickeringPlanScript(IReadOnlyList<FlickeringPhaseScript> phases, Color targetColor, bool deactivateTargetAfter)
        {
            Phases = phases;
            TargetColor = targetColor;
            DeactivateTargetAfter = deactivateTargetAfter;
        }
    }
}
