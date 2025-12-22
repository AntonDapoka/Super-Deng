using UnityEngine;
using System.Collections.Generic;

public class MenuKeyboardInputControllerScript : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] inputInteractor;

    private readonly List<IMenuKeyboardInputInteractorScript> Interactors = new();

    private readonly Queue<char> buffer = new Queue<char>(20);
    private const int BufferSize = 20;

    private void Awake()
    {
        foreach (var behaviour in inputInteractor)
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
        var input = Input.inputString;
        if (string.IsNullOrEmpty(input))
            return;

        foreach (char c in input)
        {
            AddCharToBuffer(c);
        }

        NotifyInteractors();
    }

    private void AddCharToBuffer(char character)
    {
        if (buffer.Count >= BufferSize) buffer.Dequeue();

        buffer.Enqueue(character);
    }

    private void NotifyInteractors()
    {
        char[] snapshot = buffer.ToArray();

        foreach (var interactor in Interactors)
        {
            interactor.HandleKeyboardBuffer(snapshot);
        }
    }
}
