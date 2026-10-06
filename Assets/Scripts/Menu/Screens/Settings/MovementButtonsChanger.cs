using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Menu.Screens.Settings
{
    public class MovementButtonsChanger : MonoBehaviour
    {
        [SerializeField] private Button buttonRight;
        [SerializeField] private Button buttonLeft;
        [SerializeField] private Button buttonTop;

        [SerializeField] private TextMeshProUGUI buttonRightText;
        [SerializeField] private TextMeshProUGUI buttonLeftText;
        [SerializeField] private TextMeshProUGUI buttonTopText;

        [SerializeField] private Image buttonRightImage;
        [SerializeField] private Image buttonLeftImage;
        [SerializeField] private Image buttonTopImage;

        [SerializeField] private AudioSource errorSound;

        private int currentButtonIndex = -1;

        private Key rightKey = Key.D;
        private Key leftKey = Key.A;
        private Key topKey = Key.W;

        private void Start()
        {
            buttonRightText.text = rightKey.ToString();
            buttonLeftText.text = leftKey.ToString();
            buttonTopText.text = topKey.ToString();

            buttonRight.onClick.AddListener(() => OnButtonClick(0));
            buttonLeft.onClick.AddListener(() => OnButtonClick(1));
            buttonTop.onClick.AddListener(() => OnButtonClick(2));
        }

        private void Update()
        {
            if (currentButtonIndex != -1 && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                foreach (Key key in System.Enum.GetValues(typeof(Key)))
                {
                    KeyControl control = Keyboard.current[key];
                    if (control != null && control.wasPressedThisFrame && IsValidKey(key))
                    {
                        if (!IsKeyAlreadyAssigned(key))
                        {
                            UpdateButtonTextAndImage(currentButtonIndex, key);
                            currentButtonIndex = -1;
                            SetButtonsInteractable(true);
                        }
                        else errorSound.Play();
                        break;
                    }
                }
            }
        }

        private void OnButtonClick(int index)
        {
            if (currentButtonIndex == -1)
            {
                SetButtonsInteractable(false);

                if (index == 0)
                {
                    buttonRightText.text = "Press";
                    buttonRightImage.enabled = false;
                }
                else if (index == 1)
                {
                    buttonLeftText.text = "Press";
                    buttonLeftImage.enabled = false;
                }
                else if (index == 2)
                {
                    buttonTopText.text = "Press";
                    buttonTopImage.enabled = false;
                }
                currentButtonIndex = index;
            }
        }

        private void UpdateButtonTextAndImage(int index, Key newKey)
        {
            if (index == 0)
            {
                rightKey = newKey;
                buttonRightText.text = newKey.ToString();
                buttonRightImage.enabled = true;
            }
            else if (index == 1)
            {
                leftKey = newKey;
                buttonLeftText.text = newKey.ToString();
                buttonLeftImage.enabled = true;
            }
            else if (index == 2)
            {
                topKey = newKey;
                buttonTopText.text = newKey.ToString();
                buttonTopImage.enabled = true;
            }
        }

        private bool IsValidKey(Key key)
        {
            return (key >= Key.A && key <= Key.Z) ||
                (key >= Key.Digit0 && key <= Key.Digit9) ||
                (key >= Key.Numpad0 && key <= Key.Numpad9) ||
                key == Key.LeftArrow ||
                key == Key.RightArrow ||
                key == Key.UpArrow ||
                key == Key.DownArrow;
        }

        private bool IsKeyAlreadyAssigned(Key key)
        {
            if (currentButtonIndex == 0) return key == leftKey || key == topKey;
            else if (currentButtonIndex == 1) return key == rightKey || key == topKey;
            else return key == rightKey || key == leftKey;
        }

        private void SetButtonsInteractable(bool interactable)
        {
            buttonRight.interactable = interactable || currentButtonIndex == 0;
            buttonLeft.interactable = interactable || currentButtonIndex == 1;
            buttonTop.interactable = interactable || currentButtonIndex == 2;
        }

        public MovementBindsSettingsData GetSettings()
        {
            return new MovementBindsSettingsData
            {
                right = rightKey.ToString(),
                left = leftKey.ToString(),
                top = topKey.ToString()
            };
        }

        public void SetSettings(MovementBindsSettingsData movementData)
        {
            UpdateButtonTextAndImage(0, ParseKey(movementData.right, rightKey));
            UpdateButtonTextAndImage(1, ParseKey(movementData.left, leftKey));
            UpdateButtonTextAndImage(2, ParseKey(movementData.top, topKey));
        }

        private static Key ParseKey(string name, Key fallback)
        {
            return System.Enum.TryParse(name, out Key key) ? key : fallback;
        }
    }

    [System.Serializable]
    public class MovementBindsSettingsData
    {
        public string right;
        public string left;
        public string top;
    }
}
