using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FaceStateScript))]
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

    [Space]

    [Header("Sides of the Face")]
    public GameObject side1;
    public GameObject side2;
    public GameObject side3;

    public Dictionary<string, GameObject> sides;

    [HideInInspector] public FaceScript FS1;
    [HideInInspector] public FaceScript FS2;
    [HideInInspector] public FaceScript FS3;

    [Space]
    // ��� ������ ���� ��������� Presenter

    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;

    [Space]
    [Header("Scene ScriptManagers")]
    [SerializeField] private FaceStateScript faceState; 
    [SerializeField] private FacePresenterScript facePresenter;
    [SerializeField] private FaceArrayScript faceArray;

    public bool IsTurnOn { get => isTurnOn; set => isTurnOn = value; }
    public int PathObjectCount { get => pathObjectCount; set => pathObjectCount = value; }
    public FaceStateScript FaceState => faceState; //interface
    public FaceArrayScript FaceArray => faceArray; //interface
    public FacePresenterScript FacePresenter => facePresenter; //interface

    private void Awake()
    {
        faceState = GetComponent<FaceStateScript>();
        if (glowingPart != null)
            rend = glowingPart.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (faceArray == null)
        {
            Debug.LogWarning($"{name}: FaceArray not assigned!");
            return;
        }

        pathObjectCount = FaceState.Get(FaceProperty.HavePlayer) ? 0 : -1;

        //Add code to specify the direction for the Player Face

        GameObject[] closestObjects = FindClosestObjectsFromArray(FaceArray.GetAllFaces(), 3);
        if (closestObjects.Length < 3)
        {
            Debug.LogWarning($"{name}: Not enough nearby faces found!");
            return;
        }

        side1 = closestObjects[0];
        side2 = closestObjects[1];
        side3 = closestObjects[2];

        FS1 = side1.GetComponent<FaceScript>();
        FS2 = side2.GetComponent<FaceScript>();
        FS3 = side3.GetComponent<FaceScript>();

        isTurnOn = true;
    }

    GameObject[] FindClosestObjectsFromArray(GameObject[] objectsArray, int count)
    {
        var sortedObjects = objectsArray
            .OrderBy(obj => Vector3.Distance(this.transform.position, obj.transform.position))
            .Skip(1) // We skip the first object (because the closest one is the face itself)
            .Take(count)
            .ToArray();

        return sortedObjects;
    }
}