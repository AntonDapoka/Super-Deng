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
        private int idCurrent;
        private int numberCurrent;
        private int numberOfLevels;

        public void Initialize(int[] levelsID, int levelIDInitial)
        {
            this.levelsID = levelsID;
            numberOfLevels = levelsID.Length;
            idCurrent = levelIDInitial;
            numberCurrent = System.Array.IndexOf(levelsID, levelIDInitial);
            if (numberCurrent < 0)
            {
                numberCurrent = 0;
                idCurrent = levelsID[0];
            }
            isButtonRightActive = false;
            isButtonLeftActive = false;
            view.ChangeButtonStateInstant(buttonRight, false);
            view.ChangeButtonStateInstant(buttonLeft, false);

            int centerSlot = points.Length / 2;

            iconsAccessible.Clear();
            foreach (LevelIconScript levelIcon in icons)
            {
                for (int i = 0; i < levelsID.Length; i++)
                {
                    if (levelIcon.GetLevelID() == levelsID[i])
                        iconsAccessible.Add(levelIcon);
                    if (levelIcon.GetLevelID() == idCurrent)
                        logoView.SetLevelIcon(levelIcon.gameObject);
                }
            }

            for (int i = 0; i < iconsAccessible.Count; i++)
            {
                int slot = GetSlotByOffset(centerSlot, i - numberCurrent);
                iconsAccessible[i].gameObject.transform.position = points[slot].position;
                iconsAccessible[i].gameObject.SetActive(slot == centerSlot);
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
            return iconsAccessible[numberCurrent].gameObject;
        }

        public void ShowLevelIcons()
        {
            view.ShowLevelIcons(iconsTransform, numberCurrent);
        }

        public void HideLevelIcons()
        {
            view.HideLevelIcons(iconsTransform);
        }

        public void HideSideLevelIcons()
        {
            view.HideSideLevelIcons(iconsTransform, numberCurrent);
        }

        public async Task ChangeLevelIcons(int numberNew, int numberCurrent)
        {
            this.numberCurrent = numberNew;
            idCurrent = levelsID[numberNew];
            logoView.SetLevelIcon(iconsAccessible[this.numberCurrent].gameObject);
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
            await view.MoveLevelIcons(iconsTransform, points, numberNew, numberCurrent);
            if (numberNew == 0)
            {
                view.ChangeButtonState(buttonLeft, flickeringSettings, flickeringPresenter, false);
                isButtonLeftActive = false;
            }
            else if (numberNew == numberOfLevels-1)
            {
                view.ChangeButtonState(buttonRight, flickeringSettings, flickeringPresenter, false);
                isButtonRightActive = false;
            }
            
            blockWallManager.TurnOffBlockWall();
        }
    }
}