using UnityEngine;
using System.Collections.Generic;

public class MenuKeyboardInputControllerScript : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] interactors;
    private readonly List<IMenuKeyboardInputInteractorScript> Interactors = new();

    private readonly Queue<KeyCode> buffer = new();
    private const int bufferSize = 20;

    private void Awake()
    {
        foreach (var behaviour in interactors)
        {
            if (behaviour is IMenuKeyboardInputInteractorScript interactor)
            {
                Interactors.Add(interactor);
            }
            else
            {
                Debug.LogError($"{behaviour.name} does not implement IMenuKeyboardInputInteractorScript");
            }
        }
    }

    private void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                AddKeyToBuffer(key);
                NotifyInteractors();
            }
        }
    }

    private void AddKeyToBuffer(KeyCode key)
    {
        if (buffer.Count >= bufferSize) buffer.Dequeue();

        buffer.Enqueue(key);
    }

    private void NotifyInteractors()
    {
        KeyCode[] snapshot = buffer.ToArray();

        foreach (var interactor in Interactors)
        {
            interactor.HandleKeyboardBuffer(snapshot);
        }
    }
}