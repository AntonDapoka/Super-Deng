using UnityEngine;

public class InputKeyBoardControllerScript : MonoBehaviour, IInputSourceScript
{
    [SerializeField] private InputHandlerScript inputHandler;
    public InputHandlerScript InputHandler => inputHandler;

    // List of standard keyboard keys you want to support
    private static readonly KeyCode[] KeyboardKeys =
    {
        // Letters
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
        KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };

    //InputHandlerScript IInputSourceScript.InputHandler => throw new System.NotImplementedException();

    private void Awake()
    {
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