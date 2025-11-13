using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Обязательное уведомление: "Правые", "Левые" и "Верхние" указаны для треугольника с основанием, направленным ВНИЗ!!!
// Обратите внимание, что "Правая" сторона раньше носила название "BlueSide", "Левая" - "OrangeSide", а "Верхняя" - "GreenSide"
// Помимо прочих наименований, "Правая" сторона может записываться как "Side2", "Левая" - "Side1", а "Верхняя" - "Side3"
public class FaceScript : MonoBehaviour, IFaceScript
{
    /*                /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
    */
    public bool isTurnOn = false;
    //public GameObject player;
    public int pathObjectCount = -1;
    //private PlayerScript PS;

    [Space]

    [Header("Sides of the Face")]
    public GameObject side1; // OrangeSide == Side1
    public GameObject side2; // BlueSide == Side2
    public GameObject side3; // GreenSide == Side3


    public Dictionary<string, GameObject> sides;

    [HideInInspector] public FaceScript FS1;
    [HideInInspector] public FaceScript FS2;
    [HideInInspector] public FaceScript FS3;

    [Space]
    /*
    [Header("Materials")]
    [FormerlySerializedAs("materialWhite")]
    [SerializeField] private Material materialBasicFace;
    [FormerlySerializedAs("materialRed")]
    public Material materialKillerFace;
    [FormerlySerializedAs("materialLightBlue")]
    [SerializeField] private Material materialPlayerFace;
    [FormerlySerializedAs("heheheheh")]
    [SerializeField] private Material materialSecretFace;
    [FormerlySerializedAs("materialBlue")]
    public Material materialRightFace;
    [FormerlySerializedAs("materialOrange")]
    public Material materialLeftFace;
    [FormerlySerializedAs("materialGreen")]
    public Material materialTopFace;
    */
    // Тут должен быть матриалсеттер или его интерфейс
    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;
    //private Animator animator;



    /*
    
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private BeatController BC;
    [SerializeField] private SoundScript SS;
    [SerializeField] private BonusSpawnerScript BSS;
    [SerializeField] private PathCounterScript PCS;
    [SerializeField] private PortalSpawnerScript PSS;
    [SerializeField] private NavigationHintScript NHS;
    [SerializeField] private KillYourselfScript KYSS;*/

    [Space]
    [Header("Questions")]
    //public bool havePlayer = false;

    /*
    private bool transferInProgress = false;

    public bool isKilling = false;
    public bool isBlinking = false;
    public bool isColored = false;
    public bool isBlocked = false;
    public bool isPortal = false;
    public bool isBonus = false;
    public bool isTutorial = false;
    public bool isUpsideDown = false;
    [HideInInspector] public bool isLeft = false;
    [HideInInspector] public bool isRight = false;
    [HideInInspector] public bool isTop = false;*/

    [Space]
    [Header("Scene ScriptManagers")]
    [SerializeField] private FaceStateScript faceState; //РЕАЛИЗОВАТЬ
    [SerializeField] private FacePresenterScript facePresenter;
    [SerializeField] private FaceArrayScript faceArray;

    public FaceStateScript FaceState => faceState; //interface
    public FaceArrayScript FaceArray => faceArray; //interface
    public FacePresenterScript FacePresenter => facePresenter; //interface

    public void Initialize()
    {
        rend = glowingPart.GetComponent<MeshRenderer>();

        pathObjectCount = FaceState.HavePlayer ? 0 : -1;

        //if (!FaceState.HavePlayer)
        {
            GameObject[] closestObjects = FindClosestObjectsFromArray(FaceArray.GetAllFaces(), 3);
            side1 = closestObjects[0];
            side2 = closestObjects[1];
            side3 = closestObjects[2];
        }

        FS1 = side1.GetComponent<FaceScript>();
        FS2 = side2.GetComponent<FaceScript>();
        FS3 = side3.GetComponent<FaceScript>();
    }
    
    private void Start()
    {
        //sides = new Dictionary<string, GameObject>();

        //FacePresenter //SomethingSomething
        /*animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }*/
    }

    /*
    private void SetSideMaterial(FaceScript face, Material material)
    {
        if (!face.isKilling) face.rend.material = material;
    }*/
    /*
    private void Update()
    {
        if (havePlayer && isBonus && BSS != null)
        {
            HandleBonus();
        }

        if (havePlayer && isPortal && PSS != null)
        {
            HandlePortal();
        }
    }*/

    /*
    public void HandleInput(KeyCode keyCode, string direction)
    {
        if (isTurnOn && havePlayer && !transferInProgress && BC.canPress)
        {
            string direction = "";// GetDirectionFromInput(keyCode);
            if (!string.IsNullOrEmpty(direction))
            {
                StartTransfer(GetGameObject($"{direction}Side"), direction);
                BC.isAlreadyPressed = true;
                BC.isAlreadyPressedIsAlreadyPressed = false;
                SS.TurnOnSoundStep();
            }
        }
    }*/
    /*
    private string GetDirectionFromInput()
    {
        if (Input.GetKeyDown(keyLeft) && !GetGameObject("LeftSide").GetComponent<FaceScript>().isBlocked)
        {
            return "Left";
        }
        else if (Input.GetKeyDown(keyTop) && !GetGameObject("TopSide").GetComponent<FaceScript>().isBlocked)
        {
            return "Top";
        }
        else if (Input.GetKeyDown(keyRight) && !GetGameObject("RightSide").GetComponent<FaceScript>().isBlocked)
        {
            return "Right";
        }
        return "";
    }*/
    /*
    private void HandleBonus()
    {
        HealthBonus bonusHealth = GetComponentInChildren<HealthBonus>(true);
        if (bonusHealth != null)
        {
            PS.TakeHP();
            isBonus = false;
            bonusHealth.DestroyMe();
        }
        ComboBonus bonusCombo = GetComponentInChildren<ComboBonus>(true);
        if (bonusCombo != null)
        {
            Debug.Log("COMBOTIME");
            BSS.GetComboBonus();
            isBonus = false;
            bonusCombo.DestroyMe();
        }
    }

    private void HandlePortal()
    {
        Portal portal = GetComponentInChildren<Portal>(true);
        isPortal = false;
        portal.DestroyMe();
        PSS.LoadSecretScene();
    }
    */
    private void StartTransfer(GameObject targetSide, string color)
    {
        transferInProgress = true;
        ResetSideMaterials();
        StartCoroutine(TransferPlayer(targetSide, color));
    }

    private void ResetSideMaterials()
    {
        SetSideMaterial(FS1, materialBasicFace);
        SetSideMaterial(FS2, materialBasicFace);
        SetSideMaterial(FS3, materialBasicFace);
    }

    private IEnumerator TransferPlayer(GameObject targetSide, string color)
    {
        yield return new WaitForSeconds(0.01f);
        FaceScript targetFace = targetSide.GetComponent<FaceScript>();

        havePlayer = false;
        targetFace.ReceivePlayer(player, gameObject, color, GetOtherGameObjects(color));

        transferInProgress = false;
    }

    public void ReceivePlayer(GameObject newPlayer, GameObject sidePrevious, string colorPrevious, GameObject[] sidesPreviousOther)
    {
        sides.Clear();
        isRight = false;
        isTop = false;
        isLeft = false;

        sides.Add($"{colorPrevious}Side", sidePrevious);

        FaceScript faceScriptPrevious = sidePrevious.GetComponent<FaceScript>();
        SetSideMaterial(faceScriptPrevious, GetMaterialForSide(colorPrevious));

        if (colorPrevious == "Right") { faceScriptPrevious.isRight = true; }
        else if (colorPrevious == "Left") { faceScriptPrevious.isLeft = true; }
        else if (colorPrevious == "Top") { faceScriptPrevious.isTop = true; }

        if (side1 == sidePrevious) { UpdateSidesBasedOnPrevious(side2, side3, sidesPreviousOther); }
        else if (side2 == sidePrevious) { UpdateSidesBasedOnPrevious(side1, side3, sidesPreviousOther); }
        else if (side3 == sidePrevious) { UpdateSidesBasedOnPrevious(side1, side2, sidesPreviousOther); }

        ResetOtherSides(sidesPreviousOther);

        havePlayer = true;
        PS.ResetMaterials();
        if (PCS!= null) PCS.SetPathCount();
        if (KYSS != null) KYSS.beatsNoMoving = 0;
        PS.SetCurrentFace(gameObject);

        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        newPlayer.transform.localRotation = isUpsideDown ? Quaternion.identity : Quaternion.Euler(0, 180, 180);

        //NHS.SetNavigationHint(FS1);
        //NHS.SetNavigationHint(FS2);
        //NHS.SetNavigationHint(FS3);
    }

    private Material GetMaterialForSide(string side)
    {
        switch (side)
        {
            case "Left": return materialLeftFace;
            case "Right": return materialRightFace;
            case "Top": return materialTopFace;
            default: return materialBasicFace;
        }
    }

    private void UpdateSidesBasedOnPrevious(GameObject sideOne, GameObject sideTwo, GameObject[] sidesPreviousOther)
    {
        Transform transformSideOne = sideOne.transform;
        Transform transformSideTwo = sideTwo.transform;

        float distanceFrom1To1 = Vector3.Distance(transformSideOne.position, sidesPreviousOther[0].transform.position);
        float distanceFrom1To2 = Vector3.Distance(transformSideOne.position, sidesPreviousOther[1].transform.position);
        float distanceFrom2To1 = Vector3.Distance(transformSideTwo.position, sidesPreviousOther[0].transform.position);
        float distanceFrom2To2 = Vector3.Distance(transformSideTwo.position, sidesPreviousOther[1].transform.position);

        if (distanceFrom1To1 < distanceFrom1To2 && distanceFrom2To1 > distanceFrom2To2)
        {

            UpdateSide(sideOne, sidesPreviousOther[0]);
            UpdateSide(sideTwo, sidesPreviousOther[1]);
        }
        else if (distanceFrom1To1 > distanceFrom1To2 && distanceFrom2To1 < distanceFrom2To2)
        {
            UpdateSide(sideOne, sidesPreviousOther[1]);
            UpdateSide(sideTwo, sidesPreviousOther[0]);
        }
        else
        {
            Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
    
    private void UpdateSide(GameObject side, GameObject sidePrevious)
    {
        FaceScript faceScriptSide = side.GetComponent<FaceScript>();
        FaceScript faceScriptSidePrevious = sidePrevious.GetComponent<FaceScript>();

        if (faceScriptSidePrevious.isRight)
        {
            sides.Add("RightSide", side);
            SetSideMaterial(faceScriptSide, materialRightFace);
            faceScriptSide.isRight = true;
        }
        else if (faceScriptSidePrevious.isLeft)
        {
            sides.Add("LeftSide", side);
            SetSideMaterial(faceScriptSide, materialLeftFace);
            faceScriptSide.isLeft = true;
        }
        else if (faceScriptSidePrevious.isTop)
        {
            sides.Add("TopSide", side);
            SetSideMaterial(faceScriptSide, materialTopFace);
            faceScriptSide.isTop = true;
        }
    }

    private void ResetOtherSides(GameObject[] sidesPreviousOther)
    {
        foreach (var side in sidesPreviousOther)
        {
            FaceScript faceScript = side.GetComponent<FaceScript>();
            faceScript.isRight = false;
            faceScript.isTop = false;
            faceScript.isLeft = false;
        }
    }*/
    /*
    public void ResetRightLeftTop()
    {
        if (isLeft) rend.material = materialLeftFace;
        else if (isRight) rend.material = materialRightFace;
        else if (isTop) rend.material = materialTopFace;
    }*/

    public GameObject GetGameObject(string key)
    {
        GameObject gameObject;
        if (sides.TryGetValue(key, out gameObject))
        {
            return gameObject;
        }
        return null;
    }

    public GameObject[] GetOtherGameObjects(string key)
    {
        GameObject[] otherObjects = new GameObject[2];
        int index = 0;

        foreach (var pair in sides)
        {
            if (pair.Key != key + "Side")
            {
                otherObjects[index] = pair.Value;
                index++;

                if (index == 2)
                    break;
            }
        }
        return otherObjects;
    }

    GameObject[] FindClosestObjectsFromArray(GameObject[] objectsArray, int count)
    {
        var sortedObjects = objectsArray
            .OrderBy(obj => Vector3.Distance(this.transform.position, obj.transform.position))
            .Skip(1) // Пропускаем первый объект (потому что самый ближайший - это и есть сама грань)
            .Take(count)
            .ToArray();

        return sortedObjects;
    }
    /*
    public void SetBC(BeatController bc)
    {
        BC = bc;
    }

    public void SetFAS(FaceArrayScript fas)
    {
        FAS = fas;
    }

    public void SetNHS(NavigationHintScript nhs)
    {
        NHS = nhs;
    }

    public void SetSS(SoundScript ss)
    {
        SS = ss;
    }*/
}