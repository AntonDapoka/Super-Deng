using System.Collections.Generic;
using UnityEngine;

namespace Menu.Screens.Credits
{
    public static class MenuCreditsGlitchTimingScript
    {
        private const float durationSegmentMin = 0.001f;

        public static List<float> BuildGlitchSegmentDurations(float timeTotal, MenuCreditsSettings settings)
        {
            int roll = Random.Range(0, settings.maxGlitchRoll);

            int interruptionCount = 0;
            if (roll >= settings.rollSingleInterruptionMin && roll <= settings.rollDoubleInterruptionMin) interruptionCount = 1;
            else if (roll > settings.rollDoubleInterruptionMin) interruptionCount = 2;

            var boundaries = new List<float>(2);
            if (interruptionCount == 1)
            {
                boundaries.Add(Random.Range(timeTotal * settings.shareTimeSingleInterruptionMin, timeTotal * settings.shareTimeSingleInterruptionMax));
            }
            else if (interruptionCount == 2)
            {
                float boundaryFirst = Random.Range(timeTotal * settings.shareTimeDoubleInterruptionFirstStopMin, timeTotal * settings.shareTimeDoubleInterruptionFirstStopMax);
                float boundarySecond = Random.Range((timeTotal - boundaryFirst) * settings.shareTimeDoubleInterruptionSecondStopExtraMin, timeTotal * settings.shareTimeDoubleInterruptionSecondStopMax);
                boundaries.Add(boundaryFirst);
                boundaries.Add(boundarySecond);
            }

            var segmentDurations = new List<float>(boundaries.Count + 1);
            float previousBoundary = 0f;
            foreach (float boundary in boundaries)
            {
                float clampedBoundary = Mathf.Clamp(boundary, 0f, timeTotal);
                if (clampedBoundary - previousBoundary < durationSegmentMin) continue;

                segmentDurations.Add(clampedBoundary - previousBoundary);
                previousBoundary = clampedBoundary;
            }

            if (timeTotal - previousBoundary >= durationSegmentMin) segmentDurations.Add(timeTotal - previousBoundary);
            if (segmentDurations.Count == 0) segmentDurations.Add(timeTotal);

            return segmentDurations;
        }
    }
}