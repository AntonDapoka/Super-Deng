using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
using System.IO;

public class SettingsScript : MonoBehaviour
{
    private IDataServiceScript dataService = new JsonDataServiceScript();
    public SettingsSaveData saveData = new SettingsSaveData();
    private bool isEncrypted;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle toggleFullScreen;
    [SerializeField] private TMP_Dropdown dropDownResolution;
    [SerializeField] private TMP_Dropdown dropDownLanguage;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private MovementButtonsChanger MBC;
    [Header("MasterVolumeSettings")]
    [SerializeField] private TextMeshProUGUI textMasterVolume;
    [SerializeField] private Button buttonIncreaseMaster;
    [SerializeField] private Button buttonDecreaseMaster;
    [SerializeField] private string volumeParameterMaster = "MasterVolume";
    private float currentMasterVolume = 100;
    [Header("MusicVolumeSettings")]
    [SerializeField] private TextMeshProUGUI textMusicVolume;
    [SerializeField] private Button buttonIncreaseMusic;    
    [SerializeField] private Button buttonDecreaseMusic;
    [SerializeField] private string volumeParameterMusic = "MusicVolume";
    private float currentMusicVolume = 100;
    [Header("SFXVolumeSettings")]
    [SerializeField] private TextMeshProUGUI textSFXVolume;
    [SerializeField] private Button buttonIncreaseSFX;
    [SerializeField] private Button buttonDecreaseSFX;
    [SerializeField] private string volumeParameterSFX = "SFXVolume";
    private float currentSFXVolume = 100;

    private const int step = 5;        
    private const float minValue = 0;    
    private const int maxValue = 100;
    private int easterEggCounter = 0;
    public int parameter;
    private Resolution[] resolutions;

    private string relativePath = "/settings-savefile.json";

    private void Awake()
    {
        string path = Application.persistentDataPath + relativePath;
        Debug.Log(path);
        if (!File.Exists(path))
        {
            SetFirstSaveFile();
        }
    }

    private void Start()
    {
        SetResolution();

        buttonIncreaseMaster.onClick.AddListener(() => IncreaseValue(ref currentMasterVolume, volumeParameterMaster, textMasterVolume, false));
        buttonDecreaseMaster.onClick.AddListener(() => DecreaseValue(ref currentMasterVolume, volumeParameterMaster, textMasterVolume, false));
        UpdateText(currentMasterVolume,volumeParameterMaster, textMasterVolume);

        buttonIncreaseMusic.onClick.AddListener(() => IncreaseValue(ref currentMusicVolume, volumeParameterMusic, textMusicVolume, true));
        buttonDecreaseMusic.onClick.AddListener(() => DecreaseValue(ref currentMusicVolume, volumeParameterMusic, textMusicVolume, true));
        UpdateText(currentMusicVolume,volumeParameterMusic, textMusicVolume);

        buttonIncreaseSFX.onClick.AddListener(() => IncreaseValue(ref currentSFXVolume, volumeParameterSFX, textSFXVolume, false));
        buttonDecreaseSFX.onClick.AddListener(() => DecreaseValue(ref currentSFXVolume, volumeParameterSFX, textSFXVolume, false));
        UpdateText(currentSFXVolume, volumeParameterSFX, textSFXVolume);

        LoadSettings();
        MBC.SetSettings(saveData.movementBindsData);
    }

    private void SetResolution()
    {
        dropDownResolution.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        easterEggCounter = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height; // + " " + resolutions[i].refreshRateRatio + "Hz"
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        dropDownResolution.AddOptions(options);
        dropDownResolution.RefreshShownValue();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        //QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SaveSettings()
    {
        FillSettingsSaveData();

        SerializeJson();
    }

    private void FillSettingsSaveData()
    {
        saveData.language = dropDownLanguage.value;

        saveData.volumeMaster = currentMasterVolume;
        saveData.volumeMusic = currentMusicVolume;
        saveData.volumeSFX = currentSFXVolume;

        saveData.resolution = dropDownResolution.value;
        //saveData.aspectRatio = dropDownResolution.value;
        saveData.fullscreen = System.Convert.ToInt32(Screen.fullScreen);
        saveData.quality = qualityDropdown.value;

        saveData.movementBindsData = MBC.GetSettings();

        ///saveData.difficulty = ???;
        ///saveData.skipCutScenes = ???;
        ///saveData.comments = ???;
        ///saveData.gamepadRumble = ???;

    }

    private void SerializeJson()
    {
        long s = DateTime.Now.Ticks;
        long f = 0;
        if (dataService.SaveData("/settings-savefile.json", saveData, isEncrypted))
        {
            f = DateTime.Now.Ticks - s;
            Debug.Log($"Save Time {(f / 100000f):N4}ms");
            Debug.Log(Application.persistentDataPath + "/settings-savefile.json");
        }
        else
        {
            Debug.LogError("Cant save file bro");
        }
    }

    public void LoadSettings()
    {
        DeserializeJson();
        ReadSettingsSaveData();
    }

    private void DeserializeJson()
    {
        long s = DateTime.Now.Ticks;
        long f = 0;
        try
        {
            saveData = dataService.LoadData<SettingsSaveData>("/settings-savefile.json", isEncrypted);
            f = DateTime.Now.Ticks - s;
            Debug.Log($"Load Time {(f / 100000f):N4}ms");
        }
        catch
        {
            Debug.LogError("Cant load file bro");
        }
    }

    private void ReadSettingsSaveData()
    {
        dropDownLanguage.value = saveData.language;

        SetVolume(saveData.volumeMaster, volumeParameterMaster, textMasterVolume);
        SetVolume(saveData.volumeMusic, volumeParameterMusic, textMusicVolume);
        SetVolume(saveData.volumeSFX, volumeParameterSFX, textSFXVolume);

        dropDownResolution.value = saveData.resolution;
        bool fullscreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        Screen.fullScreen = fullscreen;
        toggleFullScreen.isOn = fullscreen;
        //saveData.aspectRatio = dropDownResolution.value;
        qualityDropdown.value = saveData.quality;

        MBC.SetSettings(saveData.movementBindsData);

        ///saveData.difficulty = ???;
        ///saveData.skipCutScenes = ???;
        ///saveData.comments = ???;
        ///saveData.gamepadRumble = ???;

    }

    private void SetVolume(float savedVolume, string volumeParameter, TextMeshProUGUI volumeText)
    {
        float actualVolume = savedVolume == 0 ? 0.00001f : savedVolume;
        audioMixer.SetFloat(volumeParameter, DecibelConvert(actualVolume));
        volumeText.text = savedVolume == 0 ? "0" : savedVolume.ToString();
    }

    private float DecibelConvert(float volumeValue)
    {
        var value = Mathf.Log10(volumeValue) * parameter - 45;
        return value;
    }



    public void IncreaseValue(ref float currentVolume, string volumeParameter, TextMeshProUGUI text, bool isEasterEgg)
    {
        currentVolume = Mathf.Clamp(currentVolume + step, minValue, maxValue);

        if (isEasterEgg && currentVolume == 100)
        {
            easterEggCounter += 1;
            if (easterEggCounter >= 15)
            {
                currentVolume = 999;
            }
        }
        UpdateText(currentVolume, volumeParameter, text);
    }

    public void DecreaseValue(ref float currentVolume, string volumeParameter, TextMeshProUGUI text, bool isEasterEgg)
    {
        if (isEasterEgg) easterEggCounter = 0;
        currentVolume = Mathf.Clamp(currentVolume - step, minValue, maxValue);
        if (currentVolume == 0)
        {
            audioMixer.SetFloat(volumeParameter, DecibelConvert(0.00001f));
            text.text = "0";
        }
        else
            UpdateText(currentVolume, volumeParameter, text);
    }

    public void UpdateText(float currentVolume, string volumeParameter, TextMeshProUGUI text)
    {
        audioMixer.SetFloat(volumeParameter, DecibelConvert(currentVolume));
        text.text = currentVolume.ToString();
    }

    private void SetFirstSaveFile()
    {
        qualityDropdown.value = 1;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                dropDownResolution.value = i;
        }
        Screen.fullScreen = false;
        toggleFullScreen.isOn = false;


        audioMixer.SetFloat(volumeParameterMaster, DecibelConvert(100));
        textMasterVolume.text = "100";
        currentMasterVolume = 100;


        audioMixer.SetFloat(volumeParameterMusic, DecibelConvert(100));
        textMusicVolume.text = "100";
        currentMusicVolume = 100;


        audioMixer.SetFloat(volumeParameterSFX, DecibelConvert(100));
        textSFXVolume.text = "100";
        currentSFXVolume = 100;

        SaveSettings();
    }
}

[System.Serializable]
public class SettingsSaveData
{
    public int language;

    public float volumeMaster;
    public float volumeMusic;
    public float volumeSFX;

    public int resolution;
    public int aspectRatio;
    public int fullscreen;
    public int quality;

    public MovementBindsSettingsData movementBindsData;

    public string difficulty;

    public bool skipCutScenes;
    public bool comments;
    public bool gamepadRumble;
}