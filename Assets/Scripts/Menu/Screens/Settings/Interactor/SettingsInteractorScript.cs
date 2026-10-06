using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu.Screens.Settings
{
    public class SettingsInteractorScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SettingsPresenterScript presenter;

        public SettingsSaveInteractorScript Save { get; } = new SettingsSaveInteractorScript();
        public VolumeSettingsInteractorScript Volume { get; } = new VolumeSettingsInteractorScript();
        public KeyBindingInteractorScript KeyBinding { get; } = new KeyBindingInteractorScript();
        public DisplaySettingsInteractorScript Display { get; } = new DisplaySettingsInteractorScript();

        private bool isInitialized;

        private void Awake()
        {
            Volume.OnVolumeChanged += presenter.UpdateVolumeDisplay;
            KeyBinding.OnBindingChanged += presenter.ShowBinding;
        }

        private void Start()
        {
            var resolutionOptions = Display.BuildResolutionOptions();
            Initialize();

            presenter.ShowResolutions(resolutionOptions, Display.ResolutionIndex);
            presenter.SyncDisplayControls(Display.IsFullscreen, Display.Quality, Display.Language);

            isInitialized = true;
        }

        private void Initialize()
        {
            if (Save.TryLoad(out SettingsSaveData data)) ApplyFromSave(data);
            else ResetToDefaults();
        }

        public void IncreaseVolume(VolumeChannel channel)
        {
            Volume.Increase(channel);
            AutoSave();
        }

        public void DecreaseVolume(VolumeChannel channel)
        {
            Volume.Decrease(channel);
            AutoSave();
        }

        public void SaveAll()
        {
            Save.Save(BuildSaveData());
        }

        private void AutoSave()
        {
            if (isInitialized) SaveAll();
        }

        public void BeginRebind(MovementDirection direction)
        {
            KeyBinding.BeginRebind(direction);
            presenter.BeginRebindDisplay(direction);
        }

        public void HandleKeyPressed(Key key)
        {
            switch (KeyBinding.TryAssignKey(key))
            {
                case KeyAssignResult.Assigned:
                    presenter.EndRebindDisplay();
                    AutoSave();
                    break;
                case KeyAssignResult.Duplicate:
                    presenter.PlayBindingError();
                    break;
            }
        }

        public void ChangeResolution(int index)
        {
            Display.ApplyResolution(index);
            AutoSave();
        }

        public void ChangeFullscreen(bool isFullscreen)
        {
            Display.SetFullscreen(isFullscreen);
            AutoSave();
        }

        public void ChangeQuality(int qualityIndex)
        {
            Display.SetQuality(qualityIndex);
            AutoSave();
        }

        public void ChangeLanguage(int languageIndex)
        {
            Display.Language = languageIndex;
            AutoSave();
        }

        private void ApplyFromSave(SettingsSaveData data)
        {
            Volume.SetVolume(VolumeChannel.Master, data.volumeMaster);
            Volume.SetVolume(VolumeChannel.Music, data.volumeMusic);
            Volume.SetVolume(VolumeChannel.SFX, data.volumeSFX);

            KeyBinding.FromSettingsData(data.movementBindsData);

            Display.ResolutionIndex = data.resolution;
            Display.SetFullscreen(data.fullscreen == 1);
            Display.SetQuality(data.quality);
            Display.Language = data.language;
        }

        public void ResetToDefaults()
        {
            Volume.SetVolume(VolumeChannel.Master, 100);
            Volume.SetVolume(VolumeChannel.Music, 100);
            Volume.SetVolume(VolumeChannel.SFX, 100);

            KeyBinding.ResetToDefaults();

            Display.SetFullscreen(true);
            Display.ApplyResolution(Display.CurrentResolutionIndex);
            Display.SetQuality(1);
            Display.Language = 0;

            SaveAll();
        }

        private SettingsSaveData BuildSaveData()
        {
            return new SettingsSaveData
            {
                language = Display.Language,

                volumeMaster = Volume.GetVolume(VolumeChannel.Master),
                volumeMusic = Volume.GetVolume(VolumeChannel.Music),
                volumeSFX = Volume.GetVolume(VolumeChannel.SFX),

                resolution = Display.ResolutionIndex,
                fullscreen = Display.IsFullscreen ? 1 : 0,
                quality = Display.Quality,

                movementBindsData = KeyBinding.ToSettingsData()
            };
        }
    }
}
