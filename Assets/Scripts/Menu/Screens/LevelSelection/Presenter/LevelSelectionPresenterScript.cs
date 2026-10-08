using Menu.Effects.Flickering;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionPresenterScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelSelectionViewScript view;
        [SerializeField] private MenuBlockWallManagerScript blockWallManager; 

        [Header("Level Selection Icons")]
        [SerializeField] private Transform[] points;
        [SerializeField] private LevelIconScript[] icons;
        private Transform[] iconsTransform;
        
        [Header("UI")]       
        [SerializeField] private Button buttonRight;
        [SerializeField] private Button buttonLeft;

        [Header("Flickering")] 
        [SerializeField] private FlickeringPresenterScript flickeringPresenter;
        [SerializeField] private FlickeringSettings flickeringSettings;

        private bool isButtonRightActive;
        private bool isButtonLeftActive;
        private int indexCurrent;

        private void Awake()
        {
            iconsTransform = new Transform[icons.Length];
            for (int i = 0; i < icons.Length; i++)
                iconsTransform[i] = icons[i].gameObject.transform;
        }

        public void Initialize(int levelIndexInitial)
        {
            indexCurrent = levelIndexInitial;
            isButtonRightActive = false;
            isButtonRightActive = false;
            view.ChangeButtonStateInstant(buttonRight, false);
            view.ChangeButtonStateInstant(buttonLeft, false);
            

            foreach (LevelIconScript levelIcon in icons)
            {
                int id = levelIcon.GetLevelID();
                switch (id)
                {
                    case int n when n < levelIndexInitial-1:
                        levelIcon.gameObject.transform.position = points[0].position;
                                        levelIcon.gameObject.SetActive(false);
                        break;
                    case int n when n == levelIndexInitial-1:
                        levelIcon.gameObject.transform.position = points[1].position;
                                        levelIcon.gameObject.SetActive(false);
                        break;
                    case int n when n == levelIndexInitial:
                        levelIcon.gameObject.transform.position = points[2].position;
                                        levelIcon.gameObject.SetActive(true);
                        break;
                    case int n when n == levelIndexInitial+1:
                        levelIcon.gameObject.transform.position = points[3].position;
                                        levelIcon.gameObject.SetActive(false);
                        break;
                    case int n when n > levelIndexInitial+1:
                        levelIcon.gameObject.transform.position = points[4].position;
                                        levelIcon.gameObject.SetActive(false);
                        break;
                    default:
                        Debug.Log("Maybe another day");
                        break;
                }
            }
        }

        public void ShowButtons()
        {
            view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, true);
            isButtonRightActive = true;
            view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter,  true);
            isButtonLeftActive = true;
        }

        public void HideButtons()
        {
            view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, false);
            isButtonRightActive = false;
            view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter, false);
            isButtonRightActive = false;
        }

        public void ShowLevelIcons()
        {
            view.ShowLevelIcons(iconsTransform);
        }

        public void HideLevelIcons()
        {
            view.HideLevelIcons(iconsTransform);
        }

        public void HideSideLevelIcons()
        {
            view.HideSideLevelIcons(iconsTransform, indexCurrent);
        }

        public async Task ChangeLevelIcons(int indexNew, int indexCurrent)
        {
            this.indexCurrent = indexNew;
            blockWallManager.TurnOnBlockWall();
            await view.MoveLevelIcons(iconsTransform, points, indexNew, indexCurrent);
            blockWallManager.TurnOffBlockWall();
        }
    }
}