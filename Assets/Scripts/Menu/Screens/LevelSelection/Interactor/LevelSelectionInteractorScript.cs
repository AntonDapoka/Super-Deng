using System.Collections.Generic;
using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionInteractorScript : MonoBehaviour
    {
        [SerializeField] private int numberOfLevels = 0;
        [SerializeField] private int idLevelCurrent = 0;
        private const int IdLevelinitial = 2;
        
        [Header("References")]
        [SerializeField] private LevelSelectionPresenterScript presenter;

        public LevelSelectionSaveInteractorScript Save { get; } = new();
        
        private void Start()
        {
            Initialize();
            idLevelCurrent = IdLevelinitial;
            presenter.Initialize(IdLevelinitial);

            ResetToDefaults();
        }

        public void SwitchToRightLevel()
        {
            if (idLevelCurrent < numberOfLevels - 1)
            {
                int indexNew = idLevelCurrent + 1;
                _ = presenter.ChangeLevelIcons(indexNew, idLevelCurrent);
                idLevelCurrent = indexNew;
            }
        }

        public void SwitchToLeftLevel()
        {
            if (idLevelCurrent > 0)
            {
                int indexNew = idLevelCurrent - 1;
                _ = presenter.ChangeLevelIcons(indexNew, idLevelCurrent);
                idLevelCurrent = indexNew;
            }
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
            numberOfLevels = data.Levels.Count;
        }

        private void ResetToDefaults()
        {
            //throw new NotImplementedException();

            SaveAll();
        }

        public void SaveAll()
        {
            Save.Save(BuildSaveData());
        }

        private GameSaveData BuildSaveData()
        {
            return new GameSaveData
            {
                Levels = GetNewLevels(),
            };
        }

        private List<LevelSaveData> GetNewLevels()
        {
            List<LevelSaveData> levels = new();
            LevelSaveData level0 = new()
            {
                levelId = 0
            };
            levels.Add(level0);
            LevelSaveData level1 = new()
            {
                levelId = 1
            };
            levels.Add(level1);
            LevelSaveData level2 = new()
            {
                levelId = 2
            };
            levels.Add(level2);
            LevelSaveData level3 = new()
            {
                levelId = 3
            };
            levels.Add(level3);
            LevelSaveData level4 = new()
            {
                levelId = 4
            };
            levels.Add(level4);
            LevelSaveData level5 = new()
            {
                levelId = 5
            };
            levels.Add(level5);
            LevelSaveData level6 = new()
            {
                levelId = 6
            };
            levels.Add(level6);
            return levels;
        }
   }
}