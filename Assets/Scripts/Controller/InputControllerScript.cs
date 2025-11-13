using UnityEngine;

public class InputControllerScript : MonoBehaviour
{
    [SerializeField] private MonoBehaviour inputHandler;
    private IInputHandlerScript InputHandler;

    private void Awake()
    {
        InputHandler = inputHandler as IInputHandlerScript;
        if (InputHandler == null)
            Debug.LogError("InputControllerScript: inputHandlerBehaviour do NOT realise IInputHandler!");
    }

    private void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
                InputHandler?.HandleInput(key);
        }
    }
}