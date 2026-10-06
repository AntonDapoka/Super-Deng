using System;
using UnityEngine.InputSystem;

namespace Menu.Screens.Settings
{
    public class KeyBindingInteractorScript
    {
        public event Action<MovementDirection, Key> OnBindingChanged;

        private Key rightKey = Key.D;
        private Key leftKey = Key.A;
        private Key topKey = Key.W;
        private MovementDirection? pendingDirection;

        public bool IsRebinding => pendingDirection.HasValue;

        public void BeginRebind(MovementDirection direction)
        {
            pendingDirection = direction;
        }

        public void CancelRebind()
        {
            pendingDirection = null;
        }

        public KeyAssignResult TryAssignKey(Key key)
        {
            if (!pendingDirection.HasValue || !IsValidKey(key)) return KeyAssignResult.InvalidKey;

            if (IsKeyAlreadyAssigned(key)) return KeyAssignResult.Duplicate;

            MovementDirection direction = pendingDirection.Value;
            SetKey(direction, key);
            pendingDirection = null;
            OnBindingChanged?.Invoke(direction, key);
            return KeyAssignResult.Assigned;
        }

        public Key GetKey(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Right: return rightKey;
                case MovementDirection.Left: return leftKey;
                default: return topKey;
            }
        }

        public void ResetToDefaults()
        {
            SetKey(MovementDirection.Right, Key.D);
            SetKey(MovementDirection.Left, Key.A);
            SetKey(MovementDirection.Top, Key.W);
        }

        public MovementBindsSettingsData ToSettingsData()
        {
            return new MovementBindsSettingsData
            {
                right = rightKey.ToString(),
                left = leftKey.ToString(),
                top = topKey.ToString()
            };
        }

        public void FromSettingsData(MovementBindsSettingsData data)
        {
            if (data == null)
            {
                ResetToDefaults();
                return;
            }

            SetKey(MovementDirection.Right, ParseKey(data.right, rightKey));
            SetKey(MovementDirection.Left, ParseKey(data.left, leftKey));
            SetKey(MovementDirection.Top, ParseKey(data.top, topKey));
        }

        private void SetKey(MovementDirection direction, Key key)
        {
            switch (direction)
            {
                case MovementDirection.Right: rightKey = key; break;
                case MovementDirection.Left: leftKey = key; break;
                default: topKey = key; break;
            }

            OnBindingChanged?.Invoke(direction, key);
        }

        private bool IsKeyAlreadyAssigned(Key key)
        {
            if (pendingDirection == MovementDirection.Right) return key == leftKey || key == topKey;
            else if (pendingDirection == MovementDirection.Left) return key == rightKey || key == topKey;
            else return key == rightKey || key == leftKey;
        }

        private static bool IsValidKey(Key key)
        {
            return (key >= Key.A && key <= Key.Z) ||
                (key >= Key.Digit0 && key <= Key.Digit9) ||
                (key >= Key.Numpad0 && key <= Key.Numpad9) ||
                key == Key.LeftArrow ||
                key == Key.RightArrow ||
                key == Key.UpArrow ||
                key == Key.DownArrow;
        }

        private static Key ParseKey(string name, Key fallback)
        {
            return Enum.TryParse(name, out Key key) ? key : fallback;
        }
    }
}
