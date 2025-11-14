using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceScript : MonoBehaviour, IFaceScript
{
    // Mandatory Disclaimer: "Right," "Left," and "Top" are for a triangle with its base pointing DOWN!!!
    // Among other names, "Right" may be written as "Side2," "Left" as "Side1," and "Top" as "Side3."
    /* Triangle orientation:
      
                      /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
    */
    [Header("State")]
    [SerializeField] private bool isTurnOn = false;
    [SerializeField] private int pathObjectCount = -1;
    [SerializeField] private FaceStateScript faceState;

    [Space]

    [Header("Sides of the Face")]
    public GameObject side1;
    public GameObject side2;
    public GameObject side3;

    [Space]

    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;

    public bool IsTurnOn { get => isTurnOn; set => isTurnOn = value; }
    public int PathObjectCount { get => pathObjectCount; set => pathObjectCount = value; }
    public FaceStateScript FaceState => faceState; //interface

    private void Awake()
    {
        faceState = GetComponent<FaceStateScript>();
        if (glowingPart != null)
            rend = glowingPart.GetComponent<MeshRenderer>();
    }

    public void Initialize(GameObject[] closestObjects, bool havePlayer)
    {
        faceState.Set(FaceProperty.HavePlayer, havePlayer);
        pathObjectCount = havePlayer ? 0 : -1;
        //Add code to specify the direction for the Player Face

        if (closestObjects.Length < 3)
        {
            Debug.LogWarning($"{name}: Not enough nearby faces found!");
            return;
        }

        side1 = closestObjects[0];
        side2 = closestObjects[1];
        side3 = closestObjects[2];

        isTurnOn = true;
    }
}