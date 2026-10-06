using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Menu.Screens.Credits
{
    public class MenuCreditsInteractorScript : MonoBehaviour
    {
        [Header("Lines")]
        [SerializeField] private GameObject[] parentObjects;

        [Header("References")]
        [SerializeField] private MenuCreditsCameraManagerScript cameraManager;
        [SerializeField] private MenuLogoNeonFlinkeringScript menuLogo;
        [SerializeField] private MenuCreditsViewScript view;

        [Header("Settings")]
        [SerializeField] private MenuCreditsSettings settings;

        private MenuCreditsState state = MenuCreditsState.Idle;
        private GameObject[][] sortedChildren;
        private const int LogoLineIndex = 1;

        private void Awake()
        {
            if (cameraManager == null || view == null || menuLogo == null || settings == null)
            {
                Debug.LogError("references aren't assigned");
                enabled = false;
                return;
            }

            cameraManager.Initialize(settings);
            view.Initialize(settings);

            sortedChildren = new GameObject[parentObjects.Length][];

            for (int i = 0; i < parentObjects.Length; i++)
            {
                if (parentObjects[i] == null) continue;

                sortedChildren[i] = parentObjects[i].transform
                    .Cast<Transform>()
                    .OrderBy(child => child.position.x)
                    .Select(child => child.gameObject)
                    .ToArray();
            }

            view.HideAllWords(sortedChildren);
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
            view.StopAllAnimations();
            cameraManager.StopMoving();

            state = MenuCreditsState.Ending;
            StartCoroutine(EndingCreditsSequence());
        }

        private IEnumerator EndingCreditsSequence()
        {
            if (!menuLogo.isTurnOn) menuLogo.LogoTurningOnAndOff(settings.durationLogoTurnOn, true, true, true, false);

            yield return Task.WhenAll(
                cameraManager.ReturnToInitialAsync(),
                view.TurnOffWordsAsync(sortedChildren, settings.durationLine)
            ).WaitAsync();

            state = MenuCreditsState.Idle;
        }

        private IEnumerator PlayingCreditsSequence()
        {
            menuLogo.LogoTurningOnAndOff(settings.durationLogoTurnOff, false, true, false, false);

            yield return new WaitForSeconds(settings.durationLine);

            for (int i = 0; i < sortedChildren.Length; i++)
            {
                if (sortedChildren[i] == null || sortedChildren[i].Length == 0) continue;

                float lineTime = MenuCreditsTimelineCalculatorScript.GetLineDuration(i, settings);

                if (i == LogoLineIndex)
                {
                    menuLogo.LogoTurningOnAndOff(settings.durationLine, true, true, true, false);
                    yield return new WaitForSeconds(settings.durationLine);
                    cameraManager.BeginRun();
                    state = MenuCreditsState.Running;
                }

                float timeForWord = lineTime / sortedChildren[i].Length;

                view.ShowWord(sortedChildren[i][0]);

                for (int j = 0; j < sortedChildren[i].Length; j++)
                {
                    if (j < sortedChildren[i].Length - 1)
                    {
                        yield return new WaitForSeconds(MenuCreditsTimelineCalculatorScript.GetNextWordDelayDuration(i, sortedChildren[i].Length, settings));
                        view.ShowWord(sortedChildren[i][j + 1]);
                    }

                    if (!sortedChildren[i][j].TryGetComponent<TextMeshPro>(out var textMesh))
                    {
                        Debug.LogWarning($"word '{sortedChildren[i][j].name}' has no TextMeshPro");
                        continue;
                    }

                    yield return view.FadeWordAsync(textMesh, timeForWord, Color.gray, Color.white).WaitAsync();

                    if (i == sortedChildren.Length - 1 && j == sortedChildren[i].Length - 1)
                    {
                        yield return new WaitForSeconds(lineTime * settings.durationModifierFinalWord);
                        cameraManager.BeginDeceleration();
                        state = MenuCreditsState.Decelerating;
                    }
                }
            }
        }
    }
}