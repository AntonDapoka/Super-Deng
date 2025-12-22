using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonControllerInitializerScript : MonoBehaviour
{
    [SerializeField] private MenuButtonControllerScript[] controllers;

    [SerializeField] private MenuButtonInteractorScript[] interactors;

    private void Start()
    {
        foreach (var controller in controllers)
        {
            //controller.Initialize(Interactor);
        }
    }
}
