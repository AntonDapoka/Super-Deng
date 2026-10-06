
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Menu.Screens.Credits
{
    public class MenuCreditsPresenterScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuCreditsReferenceHolderScript referenceHolder;
        [SerializeField] private MenuCreditsCameraManagerScript cameraManager;
        [SerializeField] private MenuLogoNeonFlinkeringScript menuLogo;
        [SerializeField] private MenuCreditsViewScript view;

        private GameObject[][] sortedChildren;
        private int indexLanguage = 0;

        public int LinesCount => sortedChildren.Length;
        public bool IsReady => sortedChildren != null;

        public void Initialize(MenuCreditsSettings settings)
        {
            if (referenceHolder == null || cameraManager == null || menuLogo == null || view == null)
            {
                Debug.LogError("references aren't assigned");
                enabled = false;
                return;
            }

            cameraManager.Initialize(settings);
            view.Initialize(settings);

            GameObject[] parentObjects = referenceHolder.GetParentObjects(indexLanguage);
            sortedChildren = SortChildren(parentObjects);

            view.HideAllWords(sortedChildren);
        }

        public int GetWordCountInLine(int lineIndex)
        {
            return sortedChildren[lineIndex] == null ? 0 : sortedChildren[lineIndex].Length;
        }

        public void ShowWord(int lineIndex, int wordIndex)
        {
            view.ShowWord(sortedChildren[lineIndex][wordIndex]);
        }

        public Task FadeWordAsync(int lineIndex, int wordIndex, float time)
        {
            GameObject word = sortedChildren[lineIndex][wordIndex];

            if (!word.TryGetComponent<TextMeshPro>(out var textMesh))
            {
                Debug.Log("word has no TextMeshPro");
                return Task.CompletedTask;
            }

            return view.FadeWordAsync(textMesh, time, Color.gray, Color.white);
        }

        public Task TurnOffAllWordsAsync(float timeForLine)
        {
            return view.TurnOffWordsAsync(sortedChildren, timeForLine);
        }

        public void StopAllAnimations()
        {
            view.StopAllAnimations();
        }

        public void TurnLogoOff(float duration)
        {
            menuLogo.LogoTurningOnAndOff(duration, false, true, false, false);
        }

        public void TurnLogoOn(float duration)
        {
            menuLogo.LogoTurningOnAndOff(duration, true, true, true, false);
        }

        public void EnsureLogoOn(float duration)
        {
            if (!menuLogo.isTurnOn) TurnLogoOn(duration);
        }

        public void BeginCameraRun()
        {
            cameraManager.BeginRun();
        }

        public void BeginCameraDeceleration()
        {
            cameraManager.BeginDeceleration();
        }

        public void StopCameraMoving()
        {
            cameraManager.StopMoving();
        }

        public Task ReturnCameraToInitialAsync()
        {
            return cameraManager.ReturnToInitialAsync();
        }

        private static GameObject[][] SortChildren(GameObject[] parentObjects)
        {
            var sorted = new GameObject[parentObjects.Length][];

            for (int i = 0; i < parentObjects.Length; i++)
            {
                if (parentObjects[i] == null) continue;

                sorted[i] = parentObjects[i].transform
                    .Cast<Transform>()
                    .OrderBy(child => child.position.x)
                    .Select(child => child.gameObject)
                    .ToArray();
            }
            return sorted;
        }

        public void SetLanguageIndex(int languageIndex)
        {
            indexLanguage = languageIndex;
        }
    }
}
