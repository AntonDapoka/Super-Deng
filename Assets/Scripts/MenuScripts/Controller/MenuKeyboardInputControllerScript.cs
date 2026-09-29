using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MenuKeyboardInputControllerScript : MonoBehaviour
{
    [SerializeField] private MenuKeyboardInputInteractorScript[] interactors;

    private readonly Queue<Key> buffer = new();
    private const int bufferSize = 20;

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        foreach (var key in keyboard.allKeys)
        {
            if (key != null && key.wasPressedThisFrame)
            {
                AddKeyToBuffer(key.keyCode);
                NotifyInteractors();
            }
        }
    }

    private void AddKeyToBuffer(Key key)
    {
        if (buffer.Count >= bufferSize) buffer.Dequeue();

        buffer.Enqueue(key);
    }

    private void NotifyInteractors()
    {
        foreach (var interactor in interactors) interactor.HandleKeyboardBuffer(buffer.ToArray());
    }
}