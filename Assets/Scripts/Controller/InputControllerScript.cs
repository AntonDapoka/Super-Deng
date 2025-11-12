using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllerScript : MonoBehaviour
{
    [SerializeField] private PlayerInteractorScript interactor;

    [Header("Key Bindings")]
    private KeyCode[] keys;
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;


    private void Start()
    {
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));

        keys = new[] { keyLeft, keyTop, keyRight };
    }

    private void Update()
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
                interactor.HandleInput(key);
        }
    }
}
