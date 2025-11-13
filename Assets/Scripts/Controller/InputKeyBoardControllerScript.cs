using UnityEngine;

public class InputControllerScript : MonoBehaviour
{
    [SerializeField] private MonoBehaviour inputHandler;
    private IInputHandlerScript InputHandler;

    // List of standard keyboard keys you want to support
    private static readonly KeyCode[] KeyboardKeys =
    {
        // Letters
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
        KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };

    private void Awake()
    {
        InputHandler = inputHandler as IInputHandlerScript;
        if (InputHandler == null)
            Debug.LogError("InputControllerScript: inputHandlerBehaviour does NOT implement IInputHandlerScript!");
    }

    private void Update()
    {
        foreach (KeyCode key in KeyboardKeys)
        {
            if (Input.GetKeyDown(key))
                InputHandler?.HandleInput(key);
        }
    }
}