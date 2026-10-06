using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace Menu.Screens.Settings
{
    public class KeyBindingInputControllerScript : MonoBehaviour
    {
        [SerializeField] private SettingsInteractorScript settingsInteractor;

        [Header("Buttons")]
        [SerializeField] private Button buttonRight;
        [SerializeField] private Button buttonLeft;
        [SerializeField] private Button buttonTop;

        private bool isCapturing;

        private void Awake()
        {
            buttonRight.onClick.AddListener(() => settingsInteractor.BeginRebind(MovementDirection.Right));
            buttonLeft.onClick.AddListener(() => settingsInteractor.BeginRebind(MovementDirection.Left));
            buttonTop.onClick.AddListener(() => settingsInteractor.BeginRebind(MovementDirection.Top));
        }

        public void SetCapturing(bool capturing)
        {
            isCapturing = capturing;
        }

        private void Update()
        {
            if (!isCapturing || Keyboard.current == null || !Keyboard.current.anyKey.wasPressedThisFrame)
                return;

            foreach (KeyControl control in Keyboard.current.allKeys)
            {
                if (control.wasPressedThisFrame)
                {
                    settingsInteractor.HandleKeyPressed(control.keyCode);
                    break;
                }
            }
        }
    }
}
