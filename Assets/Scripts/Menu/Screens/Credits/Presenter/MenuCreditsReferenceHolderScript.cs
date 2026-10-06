using UnityEngine;

namespace Menu.Screens.Credits
{
    public class MenuCreditsReferenceHolderScript : MonoBehaviour
    {
        [Header("Lines (English)")]
        [SerializeField] private GameObject[] parentObjectsEnglish;

        [Header("Lines (Spanish)")]
        [SerializeField] private GameObject[] parentObjectsSpanish;

        [Header("Lines (Chinese Simplified)")]
        [SerializeField] private GameObject[] parentObjectsChineseSimplified;

        [Header("Lines (Chinese Traditional)")]
        [SerializeField] private GameObject[] parentObjectsChineseTraditional;

        [Header("Lines (Russian)")]
        [SerializeField] private GameObject[] parentObjectsRussian;

        [Header("Lines (Italian)")]
        [SerializeField] private GameObject[] parentObjectsItalian;

        [Header("Lines (French)")]
        [SerializeField] private GameObject[] parentObjectsFrench;

        /// (0 - eng, 1 - spa, 2 - zho, 3 - chi, 4 - rus, 5 - ita, 6 - fra).

        public GameObject[] GetParentObjects(int languageIndex)
        {
            GameObject[] lines = SelectLines(languageIndex);

            if (lines == null || lines.Length == 0)
                lines = parentObjectsEnglish;

            return lines ?? new GameObject[0];
        }

        private GameObject[] SelectLines(int languageIndex)
        {
            switch (languageIndex)
            {
                case 1: return parentObjectsSpanish;
                case 2: return parentObjectsChineseSimplified;
                case 3: return parentObjectsChineseTraditional;
                case 4: return parentObjectsRussian;
                case 5: return parentObjectsItalian;
                case 6: return parentObjectsFrench;
                default: return parentObjectsEnglish;
            }
        }
    }
}
