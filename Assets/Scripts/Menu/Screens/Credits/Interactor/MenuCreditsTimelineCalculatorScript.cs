using UnityEngine;

namespace Menu.Screens.Credits
{
    public static class MenuCreditsTimelineCalculatorScript
    {
        public static float GetLineDuration(int lineIndex, MenuCreditsSettings settings)
        {
            float durationLine = settings.durationLine;

            if (settings.lineTimeModifiers == null) return durationLine;

            foreach (var modifier in settings.lineTimeModifiers)
                if (modifier != null && modifier.lineIndex == lineIndex)
                    durationLine *= modifier.timeModifier;

            return durationLine;
        }

        public static float GetNextWordDelayDuration(int lineIndex, int wordsInLine, MenuCreditsSettings settings)
        {
            float timeForWord = GetLineDuration(lineIndex, settings) / Mathf.Max(1, wordsInLine);

            float delayMin = lineIndex == 0 ? settings.delayFirstLineNextWordMin : settings.delayOtherLinesNextWordMin;
            float delayMax = lineIndex == 0 ? settings.delayFirstLineNextWordMax : settings.delayOtherLinesNextWordMax;

            return Random.Range(delayMin, delayMax) * timeForWord;
        }
    }
}