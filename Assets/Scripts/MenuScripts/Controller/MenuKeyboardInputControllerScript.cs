using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MenuKeyboardInputControllerScript : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] interactors;
    private readonly List<IMenuKeyboardInputInteractorScript> Interactors = new();

    private readonly Queue<Key> buffer = new();
    private const int bufferSize = 20;

    private void Awake()
    {
        foreach (var behaviour in interactors)
        {
            if (behaviour is IMenuKeyboardInputInteractorScript interactor) Interactors.Add(interactor);
            else Debug.LogError($"{behaviour.name} does not implement interface");
        }
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        foreach (var key in keyboard.allKeys)
        {
            if (key.wasPressedThisFrame)
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
        foreach (var interactor in Interactors) interactor.HandleKeyboardBuffer(buffer.ToArray());
    }
}