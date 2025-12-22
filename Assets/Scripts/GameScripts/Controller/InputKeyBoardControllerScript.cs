using UnityEngine;

public class InputKeyBoardControllerScript : MonoBehaviour, IInputSourceScript
{
    [SerializeField] private InputHandlerScript[] inputHandlers;
    public InputHandlerScript[] InputHandler => inputHandlers;

    private static readonly KeyCode[] KeyboardKeys =
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
        KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };

    private void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        foreach (KeyCode key in KeyboardKeys)
        {
            if (Input.GetKeyDown(key))
            {
                foreach (InputHandlerScript handler in inputHandlers)
                {
                    handler?.HandleInput(key);
                }
            }
        }
    }
}