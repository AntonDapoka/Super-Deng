using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menu.Screens.Settings
{
    public class KeyBindingViewScript : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button buttonRight;
        [SerializeField] private Button buttonLeft;
        [SerializeField] private Button buttonTop;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI textRight;
        [SerializeField] private TextMeshProUGUI textLeft;
        [SerializeField] private TextMeshProUGUI textTop;

        [Header("Images")]
        [SerializeField] private Image imageRight;
        [SerializeField] private Image imageLeft;
        [SerializeField] private Image imageTop;

        [Header("Sounds")]
        [SerializeField] private AudioSource errorSound;

        public void ShowCapturingState(MovementDirection direction)
        {
            GetText(direction).text = "Press";
            GetImage(direction).enabled = false;
        }

        public void ShowBinding(MovementDirection direction, Key key)
        {
            GetText(direction).text = key.ToString();
            GetImage(direction).enabled = true;
        }

        public void SetAllInteractable(bool interactable)
        {
            buttonRight.interactable = interactable;
            buttonLeft.interactable = interactable;
            buttonTop.interactable = interactable;
        }

        public void SetAllInteractableExcept(MovementDirection direction, bool interactable)
        {
            buttonRight.interactable = interactable || direction == MovementDirection.Right;
            buttonLeft.interactable = interactable || direction == MovementDirection.Left;
            buttonTop.interactable = interactable || direction == MovementDirection.Top;
        }

        public void PlayErrorSound()
        {
            errorSound.Play();
        }

        private TextMeshProUGUI GetText(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Right: return textRight;
                case MovementDirection.Left: return textLeft;
                default: return textTop;
            }
        }

        private Image GetImage(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Right: return imageRight;
                case MovementDirection.Left: return imageLeft;
                default: return imageTop;
            }
        }
    }
}
