using System.Collections.Generic;
using UnityEngine;

namespace Menu.Screens.Credits
{
    public static class MenuCreditsGlitchTimingScript
    {
        private const float MinSegmentDuration = 0.001f;

        public static List<float> BuildGlitchSegmentDurations(float totalTime, MenuCreditsSettings settings)
        {
            int roll = Random.Range(0, settings.maxGlitchRoll);

            int interruptionCount = 0;
            if (roll >= settings.rollSingleInterruptionMin && roll <= settings.rollDoubleInterruptionMin) interruptionCount = 1;
            else if (roll > settings.rollDoubleInterruptionMin) interruptionCount = 2;

            var boundaries = new List<float>(2);
            if (interruptionCount == 1)
            {
                boundaries.Add(Random.Range(totalTime * settings.shareTimeSingleInterruptionMin, totalTime * settings.shareTimeSingleInterruptionMax));
            }
            else if (interruptionCount == 2)
            {
                float firstBoundary = Random.Range(totalTime * settings.shareTimeDoubleInterruptionFirstStopMin, totalTime * settings.shareTimeDoubleInterruptionFirstStopMax);
                float secondBoundary = Random.Range((totalTime - firstBoundary) * settings.shareTimeDoubleInterruptionSecondStopExtraMin, totalTime * settings.shareTimeDoubleInterruptionSecondStopMax);
                boundaries.Add(firstBoundary);
                boundaries.Add(secondBoundary);
            }

            var segmentDurations = new List<float>(boundaries.Count + 1);
            float previousBoundary = 0f;
            foreach (float boundary in boundaries)
            {
                float clampedBoundary = Mathf.Clamp(boundary, 0f, totalTime);
                if (clampedBoundary - previousBoundary < MinSegmentDuration) continue;

                segmentDurations.Add(clampedBoundary - previousBoundary);
                previousBoundary = clampedBoundary;
            }

            if (totalTime - previousBoundary >= MinSegmentDuration) segmentDurations.Add(totalTime - previousBoundary);
            if (segmentDurations.Count == 0) segmentDurations.Add(totalTime);

            return segmentDurations;
        }
    }
}