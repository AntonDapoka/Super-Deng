using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializerScript : MonoBehaviour
{
    [SerializeField] private FieldInitializerScript fieldInitializer;
    [SerializeField] private BackgroundInitializerScript backgroundInitializer;
    [SerializeField] private PlayerInitializerScript playerInitializer;
    //[SerializeField] private InputControllerInitializerScript inputControllerInitializer; ????

    private void Start()
    {
        fieldInitializer.InitializeField();
        playerInitializer.InitializePlayer();
    }
}
