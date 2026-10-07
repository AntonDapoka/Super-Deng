using System.IO;
using UnityEngine;

namespace Menu.Screens.LevelSelection
{
    public class LevelSelectionSaveInteractorScript
    {
        private const string FileName = "/game-savefile.json";
        private const bool IsEncrypted = false;
        private readonly IDataServiceScript dataService = new JsonDataServiceScript();

        public bool SaveFileExists => File.Exists(Application.persistentDataPath + FileName);

        public bool TryLoad(out GameSaveData data)
        {
            data = null;
            try
            {
                data = dataService.LoadData<GameSaveData>(FileName, IsEncrypted);
                return data != null;
            }
            catch
            {
                Debug.LogError("can't load settings file");
                return false;
            }
        }

        public void Save(GameSaveData data)
        {
            if (!dataService.SaveData(FileName, data, IsEncrypted)) Debug.LogError("can't save settings file");
        }
    }
}
