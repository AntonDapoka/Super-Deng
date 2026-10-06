using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Menu.Screens.Credits
{
    public class MenuCreditsInteractorScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuCreditsPresenterScript presenter;
        [Header("Settings")]
        [SerializeField] private MenuCreditsSettings settings;

        private MenuCreditsState state = MenuCreditsState.Idle;
        private const int LogoLineIndex = 1;

        private void Awake()
        {
            if (presenter == null || settings == null)
            {
                Debug.LogError("references aren't assigned");
                enabled = false;
                return;
            }

            presenter.Initialize(settings);

            if (!presenter.IsReady) enabled = false;
        }

        public void StartCredits()
        {
            if (state != MenuCreditsState.Idle) return;

            state = MenuCreditsState.Starting;
            StartCoroutine(PlayingCreditsSequence());
        }

        public void EndCredits()
        {
            StopAllCoroutines();
            presenter.StopAllAnimations();
            presenter.StopCameraMoving();

            state = MenuCreditsState.Ending;
            StartCoroutine(EndingCreditsSequence());
        }

        private IEnumerator EndingCreditsSequence()
        {
            presenter.EnsureLogoOn(settings.durationLogoTurnOn);

            yield return Task.WhenAll(
                presenter.ReturnCameraToInitialAsync(),
                presenter.TurnOffAllWordsAsync(settings.durationLine)
            ).WaitAsync();

            state = MenuCreditsState.Idle;
        }

        private IEnumerator PlayingCreditsSequence()
        {
            presenter.TurnLogoOff(settings.durationLogoTurnOff);

            yield return new WaitForSeconds(settings.durationLine);

            for (int i = 0; i < presenter.LinesCount; i++)
            {
                int wordCount = presenter.GetWordCountInLine(i);
                if (wordCount == 0) continue;

                float lineTime = MenuCreditsTimelineCalculatorScript.GetLineDuration(i, settings);

                if (i == LogoLineIndex)
                {
                    presenter.TurnLogoOn(settings.durationLine);
                    yield return new WaitForSeconds(settings.durationLine);
                    presenter.BeginCameraRun();
                    state = MenuCreditsState.Running;
                }

                float timeForWord = lineTime / wordCount;

                presenter.ShowWord(i, 0);

                for (int j = 0; j < wordCount; j++)
                {
                    if (j < wordCount - 1)
                    {
                        yield return new WaitForSeconds(MenuCreditsTimelineCalculatorScript.GetNextWordDelayDuration(i, wordCount, settings));
                        presenter.ShowWord(i, j + 1);
                    }

                    yield return presenter.FadeWordAsync(i, j, timeForWord).WaitAsync();

                    if (i == presenter.LinesCount - 1 && j == wordCount - 1)
                    {
                        yield return new WaitForSeconds(lineTime * settings.durationModifierFinalWord);
                        presenter.BeginCameraDeceleration();
                        state = MenuCreditsState.Decelerating;
                    }
                }
            }
        }
    }
}
