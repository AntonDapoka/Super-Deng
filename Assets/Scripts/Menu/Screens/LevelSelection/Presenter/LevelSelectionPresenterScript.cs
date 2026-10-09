using Menu.Effects.Flickering;
using Menu.Effects.Flickering.Logo;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private MenuLogoViewScript logoView;

        [Header("Level Selection Icons")]
        [SerializeField] private Transform[] points;
        [SerializeField] private LevelIconScript[] icons;
        [SerializeField] private List<LevelIconScript> iconsAccessible;
        private Transform[] iconsTransform;
        private int[] levelsID;
        
        [Header("UI")]       
        [SerializeField] private Button buttonRight;
        [SerializeField] private Button buttonLeft;
        [SerializeField] private float durationDelayButton = 0.5f;

        [Header("Flickering")] 
        [SerializeField] private FlickeringPresenterScript flickeringPresenter;
        [SerializeField] private FlickeringSettings flickeringSettings;

        private bool isButtonRightActive;
        private bool isButtonLeftActive;
        private int indexCurrent;
        private int numberOfLevels;

        public void Initialize(int[] levelsID, int levelIndexInitial)
        {
            indexCurrent = levelIndexInitial;
            this.levelsID = levelsID;
            numberOfLevels = levelsID.Length;
            isButtonRightActive = false;
            isButtonLeftActive = false;
            view.ChangeButtonStateInstant(buttonRight, false);
            view.ChangeButtonStateInstant(buttonLeft, false);

            int centerSlot = points.Length / 2;

            foreach (LevelIconScript levelIcon in icons)
            {
                for (int i = 0; i < levelsID.Length; i++)
                {
                    if (levelIcon.GetLevelID() == levelsID[i])
                        iconsAccessible.Add(levelIcon);
                    if (levelIcon.GetLevelID() == indexCurrent) //add flag
                        logoView.SetLevelIcon(levelIcon.gameObject);
                }
            }

            foreach (LevelIconScript levelIconAccessible in iconsAccessible)
            {
                int slot = GetSlotByOffset(centerSlot, levelIconAccessible.GetLevelID() - levelIndexInitial);
                levelIconAccessible.gameObject.transform.position = points[slot].position;
                levelIconAccessible.gameObject.SetActive(slot == centerSlot);
            }

            iconsTransform = new Transform[iconsAccessible.Count];
            for (int i = 0; i < iconsAccessible.Count; i++)
                iconsTransform[i] = iconsAccessible[i].gameObject.transform;
        }

        private int GetSlotByOffset(int centerSlot, int offset)
        {
            return Mathf.Clamp(centerSlot + offset, 0, points.Length - 1);
        }

        public void ShowButtons()
        {
            if (!isButtonRightActive)
            {
                view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, true);
                isButtonRightActive = true;
            }
            if (!isButtonLeftActive)
            {
                view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter,  true);
                isButtonLeftActive = true;
            }
        }

        public async Task ShowButtonsWithDelay()
        {
            await this.RunAsync(WaitingShowButtons());
            ShowButtons();
        }

        private IEnumerator WaitingShowButtons()
        {
            yield return new WaitForSeconds(durationDelayButton);
        }

        public void HideButtons()
        {
            if (isButtonRightActive)
            {
                view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, false);
                isButtonRightActive = false;
            }
            if (isButtonLeftActive)
            {
                view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter,  false);
                isButtonLeftActive = false;
            }
        }

        public GameObject GetCurrentLevelIcon()
        {
            return icons[indexCurrent].gameObject;
        }

        public void ShowLevelIcons()
        {
            view.ShowLevelIcons(iconsTransform, indexCurrent);
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
            logoView.SetLevelIcon(icons[indexCurrent].gameObject);
            blockWallManager.TurnOnBlockWall();
            if (!isButtonLeftActive)
            {
                view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter, true);
                isButtonLeftActive = true;
            }
            else if (!isButtonRightActive)
            {
                view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, true);
                isButtonRightActive = true;
            }
            await view.MoveLevelIcons(iconsTransform, points, indexNew, indexCurrent);
            if (indexNew == 0)
            {
                view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter, false);
                isButtonLeftActive = false;
            }
            else if (indexNew == numberOfLevels-1)
            {
                view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, false);
                isButtonRightActive = false;
            }
            
            blockWallManager.TurnOffBlockWall();
        }
    }
}