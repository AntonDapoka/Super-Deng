using System.IO;
using UnityEngine;

namespace Menu.Screens.Settings
{
    public class SettingsSaveInteractorScript
    {
        private const string FileName = "/settings-savefile.json";
        private const bool IsEncrypted = false;
        private readonly IDataServiceScript dataService = new JsonDataServiceScript();

        public bool SaveFileExists => File.Exists(Application.persistentDataPath + FileName);

        public bool TryLoad(out SettingsSaveData data)
        {
            data = null;
            try
            {
                data = dataService.LoadData<SettingsSaveData>(FileName, IsEncrypted);
                return data != null;
            }
            catch
            {
                Debug.LogError("can't load settings file");
                return false;
            }
        }

        public void Save(SettingsSaveData data)
        {
            if (!dataService.SaveData(FileName, data, IsEncrypted)) Debug.LogError("can't save settings file");
        }
    }
}
