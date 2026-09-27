using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonInitializerScript : MonoBehaviour
{
    [SerializeField] private MenuButtonBinding[] bindings;

    private void Awake()
    {
        foreach (var binding in bindings)
            binding.Button.Initialize(binding.Interactor);
    }
}

[Serializable]
public class MenuButtonBinding
{
    [SerializeField] private MenuButtonScript button;
    [SerializeField] private MonoBehaviour commandProvider;

    public MenuButtonScript Button => button;
    public Command Command => commandProvider as ICommand;
}