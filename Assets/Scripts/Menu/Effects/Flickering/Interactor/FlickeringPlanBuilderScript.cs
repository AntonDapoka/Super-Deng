using System.Collections.Generic;
using UnityEngine;

namespace Menu.Effects.Flickering
{
    public class FlickeringPlanBuilderScript
    {
        public FlickeringPlanScript Build(in FlickeringRequestScript request)
        {
            FlickeringSettings settings = request.Settings;
            AnimationCurve curve = request.IsTurningOn ? settings.ColorCurveTurnOn : settings.ColorCurveTurnOff;
            List<FlickeringPhaseScript> phases = new(capacity: 2);

            Color restartColor = request.InitialColor;
            float remainingDuration = request.Duration;

            if (request.IsBlinking && Random.value < settings.BlinkChance)
            {
                Vector2 interruptionRange = request.IsTurningOn
                    ? settings.InterruptionTimeRangeTurnOn
                    : settings.InterruptionTimeRangeTurnOff;

                float interruptionDuration = request.Duration * Random.Range(interruptionRange.x, interruptionRange.y);
                phases.Add(new FlickeringPhaseScript(request.InitialColor, curve, interruptionDuration));

                restartColor.a *= settings.BlinkRestartAlphaFactor;
                remainingDuration -= interruptionDuration;
            }

            phases.Add(new FlickeringPhaseScript(restartColor, curve, remainingDuration));

            return new FlickeringPlanScript(phases, request.TargetColor, request.DeactivateTargetAfter);
        }
    }
}
