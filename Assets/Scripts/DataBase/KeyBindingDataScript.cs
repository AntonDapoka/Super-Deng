using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "Config/Key Bindings")]
public class KeyBindingDataScript : ScriptableObject
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveTop = KeyCode.W;
}