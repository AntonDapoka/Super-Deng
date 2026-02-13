using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "ScriptableObjects/Key Bindings", order = 60)]
public class KeyBindingDataScript : ScriptableObject
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveTop = KeyCode.W;
}