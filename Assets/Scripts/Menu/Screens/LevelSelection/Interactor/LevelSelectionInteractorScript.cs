using System;
using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionInteractorScript : MonoBehaviour
    {
        private int idLevelCurrent = 0;
        [Header("References")]
        [SerializeField] private LevelSelectionPresenterScript presenter;

        public LevelSelectionSaveInteractorScript Save { get; } = new();
        
        private void Start()
        {
            //Initialize();

            //presenter.Show
        }

        public void SwitchToRightLevel()
        {
            
        }

        public void SwitchToLeftLevel()
        {
            
        }

        public void PlaySelectedLevel()
        {
            
        }

        private void Initialize()
        {
            if (Save.TryLoad(out GameSaveData data)) ApplyFromSave(data);
            else ResetToDefaults();
        }

        private void ApplyFromSave(GameSaveData data)
        {
            //throw new NotImplementedException();
        }

        private void ResetToDefaults()
        {
            //throw new NotImplementedException();
        }
   }
}