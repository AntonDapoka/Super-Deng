using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Menu.Screens.Credits
{
    public class MenuCreditsViewScript : MonoBehaviour
    {
        private MenuCreditsSettings settings;
        private readonly Dictionary<TextMeshPro, int> fadeGenerations = new Dictionary<TextMeshPro, int>();

        public void Initialize(MenuCreditsSettings settings)
        {
            this.settings = settings;
        }

        public void HideAllWords(GameObject[][] lines)
        {
            foreach (var line in lines)
            {
                if (line == null) continue;

                foreach (var word in line)
                    HideWord(word);
            }
        }

        public void ShowWord(GameObject word)
        {
            SetWordGray(word);
            word.SetActive(true);
        }

        public void HideWord(GameObject word)
        {
            SetWordGray(word);
            word.SetActive(false);
        }

        public Task FadeWordAsync(TextMeshPro text, float time, Color initialColor, Color targetColor)
        {
            int generation = InvalidateFade(text);
            return this.RunAsync(ChangingColorSmoothly(text, time, initialColor, targetColor, generation));
        }

        public async Task TurnOffWordsAsync(GameObject[][] lines, float timeForLine)
        {
            float fadeDuration = timeForLine * settings.durationModifierWordsFadeOut;

            foreach (var line in lines)
            {
                if (line == null) continue;

                foreach (var child in line)
                    if (child != null && child.activeSelf && child.TryGetComponent<TextMeshPro>(out var textMesh))
                        _ = FadeWordAsync(textMesh, fadeDuration, Color.white, Color.clear);
            }

            await this.RunAsync(WaitForLineRoutine(timeForLine));
        }

        public void StopAllAnimations()
        {
            foreach (var text in new List<TextMeshPro>(fadeGenerations.Keys))
                fadeGenerations[text]++;
        }

        private static void SetWordGray(GameObject word)
        {
            if (word.TryGetComponent<TextMeshPro>(out var textMesh)) textMesh.color = Color.gray;
        }

        private int InvalidateFade(TextMeshPro text)
        {
            fadeGenerations.TryGetValue(text, out int generation);
            fadeGenerations[text] = generation + 1;
            return generation + 1;
        }

        private bool IsCurrentFade(TextMeshPro text, int generation)
        {
            return fadeGenerations.TryGetValue(text, out int current) && current == generation;
        }

        private IEnumerator WaitForLineRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
        }

        private IEnumerator ChangingColorSmoothly(TextMeshPro text, float time, Color initialColor, Color targetColor, int generation)
        {
            if (time <= 0f)
            {
                if (IsCurrentFade(text, generation)) text.color = targetColor;
                yield break;
            }

            foreach (float segmentDuration in MenuCreditsGlitchTimingScript.BuildGlitchSegmentDurations(time, settings))
            {
                float elapsedTime = 0f;
                while (elapsedTime < segmentDuration)
                {
                    if (!IsCurrentFade(text, generation)) yield break;

                    float curveValue = settings.curveColorChangeTurnOn.Evaluate(Mathf.Clamp01(elapsedTime / segmentDuration));
                    text.color = Color.Lerp(initialColor, targetColor, curveValue);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            if (IsCurrentFade(text, generation)) text.color = targetColor;
        }
    }
}