using UnityEngine;
using UnityEngine.UI;

namespace Menu.Controller.Buttons
{
    public class MenuButtonScript : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Command command;

        private void Start()
        {
            if (button != null && command != null) button.onClick.AddListener(command.Execute);
        }

        private void OnDestroy()
        {
            if (button != null && command != null) button.onClick.RemoveListener(command.Execute);
        }
    }
}