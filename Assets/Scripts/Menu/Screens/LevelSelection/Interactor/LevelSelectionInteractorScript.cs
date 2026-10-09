using System.Collections.Generic;
using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionInteractorScript : MonoBehaviour
    {
        [SerializeField] private int idLevelCurrent = 0;
        [SerializeField] private int numberLevelCurrent = 0;
        private int levelIDInitial;
        private int[] levelsIDAccessible;
        
        [Header("References")]
        [SerializeField] private LevelSelectionPresenterScript presenter;
        [SerializeField] private MenuSceneLoaderScript sceneLoader;

        public LevelSelectionSaveInteractorScript Save { get; } = new();
        
        private void Start()
        {
            ResetToDefaults(); //REMOVE
            Initialize();
            idLevelCurrent = levelIDInitial;

            numberLevelCurrent = System.Array.IndexOf(levelsIDAccessible, levelIDInitial);
            if (numberLevelCurrent < 0) numberLevelCurrent = 0;
            presenter.Initialize(levelsIDAccessible, levelIDInitial);


        }

        private void Initialize()
        {
            if (Save.TryLoad(out GameSaveData data)) ApplyFromSave(data); ///!!!!!!!!!!
            else ResetToDefaults();
        }

        public void SwitchToRightLevel()
        {
            if (numberLevelCurrent < levelsIDAccessible.Length - 1)
            {
                int numberNew = numberLevelCurrent + 1;
                _ = presenter.ChangeLevelIcons(numberNew, numberLevelCurrent);
                numberLevelCurrent = numberNew;
                idLevelCurrent = levelsIDAccessible[numberLevelCurrent];
            }
        }

        public void SwitchToLeftLevel()
        {
            if (numberLevelCurrent > 0)
            {
                int numberNew = numberLevelCurrent - 1;
                _ = presenter.ChangeLevelIcons(numberNew, numberLevelCurrent);
                numberLevelCurrent = numberNew;
                idLevelCurrent = levelsIDAccessible[numberLevelCurrent];
            }
        }

        public void PlaySelectedLevel()
        {
            //sceneLoader
        }

        private void ApplyFromSave(GameSaveData data)
        {
            levelsIDAccessible = GetAccessibleLevelsID(data);
        }

        private int[] GetAccessibleLevelsID(GameSaveData data)
        {
            List<int> levelsID = new();
            foreach (LevelSaveData levelData in data.Levels)
            {
                if (levelData.isLevelAccessible || levelData.isLevelIconInitial)
                    levelsID.Add(levelData.levelId);
                if (levelData.isLevelIconInitial)
                    levelIDInitial = levelData.levelId;
            }

            return levelsID.ToArray();
        }

        private void ResetToDefaults()
        {
            //!!!!!!!!!!!

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
                levelId = 0,
                isLevelAccessible = true,
                isLevelIconInitial = true
            };
            levels.Add(level0);
            LevelSaveData level1 = new()
            {
                levelId = 1,
                isLevelAccessible = false,
                isLevelIconInitial = false
            };
            levels.Add(level1);
            LevelSaveData level2 = new()
            {
                levelId = 2,
                isLevelAccessible = false,
                isLevelIconInitial = false
            };
            levels.Add(level2);
            LevelSaveData level3 = new()
            {
                levelId = 3,
                isLevelAccessible = false,
                isLevelIconInitial = false
            };
            levels.Add(level3);
            LevelSaveData level4 = new()
            {
                levelId = 4,
                isLevelAccessible = false,
                isLevelIconInitial = false
            };
            levels.Add(level4);
            LevelSaveData level5 = new()
            {
                levelId = 5,
                isLevelAccessible = false,
                isLevelIconInitial = false
            };
            levels.Add(level5);
            LevelSaveData level6 = new()
            {
                levelId = 6,
                isLevelAccessible = true,
                isLevelIconInitial = false
            };
            levels.Add(level6);
            return levels;
        }
   }
}