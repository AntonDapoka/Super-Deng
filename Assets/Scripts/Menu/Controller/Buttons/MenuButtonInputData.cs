using UnityEngine;

namespace Menu.Controller.Buttons
{
    public struct MenuButtonInputData
    {
        public bool IsClick;
        public bool IsPointerDown;
        public bool IsPointerUp;
        public bool IsKeyboard;
        public KeyCode Key;
    }
}