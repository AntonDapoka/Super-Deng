using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveDataInitializerScript
{
    private GameSaveData data;
    private List<LevelSaveData> levels;
    private List<LevelSettings> levelSettings;

    public GameSaveData GetNewGameSaveData()
    {
        data = new();
        levels = new();
        for (int i = 0; i < levelSettings.Count; i++)
        {
            LevelSaveData levelData = new();
            levels.Add(levelData);
        }
        return data;
    }
}
