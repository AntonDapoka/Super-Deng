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

        private InputAction captureAction;

        private void Awake()
        {
            captureAction = new InputAction(name: "RebindCapture", type: InputActionType.Button);
            AddCaptureBindings();
            captureAction.performed += OnCapturePerformed;

            buttonRight.onClick.AddListener(() => BeginRebind(MovementDirection.Right));
            buttonLeft.onClick.AddListener(() => BeginRebind(MovementDirection.Left));
            buttonTop.onClick.AddListener(() => BeginRebind(MovementDirection.Top));
        }

        private void OnDestroy()
        {
            captureAction.performed -= OnCapturePerformed;
            captureAction.Dispose();
        }

        private void BeginRebind(MovementDirection direction)
        {
            settingsInteractor.BeginRebind(direction);
            captureAction.Enable();
        }

        private void OnCapturePerformed(InputAction.CallbackContext context)
        {
            if (!(context.control is KeyControl keyControl)) return;

            settingsInteractor.HandleKeyPressed(keyControl.keyCode);

            if (!settingsInteractor.KeyBinding.IsRebinding)
                captureAction.Disable();
        }

        private void AddCaptureBindings()
        {
            for (int i = 0; i < 26; i++)
                captureAction.AddBinding($"<Keyboard>/{(char)('a' + i)}");

            for (int i = 0; i < 10; i++)
            {
                captureAction.AddBinding($"<Keyboard>/{(char)('0' + i)}");
                captureAction.AddBinding($"<Keyboard>/numpad{i}");
            }

            captureAction.AddBinding("<Keyboard>/leftArrow");
            captureAction.AddBinding("<Keyboard>/rightArrow");
            captureAction.AddBinding("<Keyboard>/upArrow");
            captureAction.AddBinding("<Keyboard>/downArrow");
        }
    }
}
