using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// Обязательное уведомление: "Правые", "Левые" и "Верхние" указаны для треугольника с основанием, направленным ВНИЗ!!!
// Обратите внимание, что "Правая" сторона раньше носила название "BlueSide", "Левая" - "OrangeSide", а "Верхняя" - "GreenSide"
// Помимо прочих наименований, "Правая" сторона может записываться как "Side2", "Левая" - "Side1", а "Верхняя" - "Side3"
public class FaceScript : MonoBehaviour 
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
    public GameObject player;
    public int pathObjectCount = -1;
    [SerializeField] private PlayerScript PS;

    [Space]
    [Header("Sides of the Face")]
    [FormerlySerializedAs("sideBlue")]
    [SerializeField] private GameObject side1; // OrangeSide == Side1
    [SerializeField] private GameObject side2; // BlueSide == Side2
    [SerializeField] private GameObject side3; // GreenSide == Side3
    public Dictionary<string, GameObject> sides;

    [HideInInspector] public FaceScript FS1;
    [HideInInspector] public FaceScript FS2;
    [HideInInspector] public FaceScript FS3;

    [Space]
    [Header("Materials")]
    [FormerlySerializedAs("materialWhite")]
    [SerializeField] private Material materialBasicFace;
    [FormerlySerializedAs("materialRed")]
    [SerializeField] private Material materialKillerFace;
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

    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;
    private Animator animator;

    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;

    [Space]
    [Header("Scene ScriptManagers")]
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private BeatController BC;
    [SerializeField] private SoundScript SS;
    [SerializeField] private BonusSpawnerScript BSS;
    [SerializeField] private PathCounterScript PCS;
    [SerializeField] private PortalSpawnerScript PSS;
    [SerializeField] private NavigationHintScript NHS;

    [Space]
    [Header("Questions")]
    public bool havePlayer = false;
    private bool transferInProgress = false;
    public bool isKilling = false;
    public bool isBlinking = false;
    public bool isColored = false;
    public bool isBlocked = false;
    public bool isPortal = false;
    public bool isBonus = false;
     public bool isLeft = false;
     public bool isRight = false;
     public bool isTop = false;
    private void Awake()
    {
        rend = glowingPart.GetComponent<MeshRenderer>();
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));

        pathObjectCount = havePlayer ? 0 : -1;

        FS1 = side1.GetComponent<FaceScript>();
        FS2 = side2.GetComponent<FaceScript>();
        FS3 = side3.GetComponent<FaceScript>();
    }

    private void Start()
    {
        sides = new Dictionary<string, GameObject>();

        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        if (havePlayer)
        {
            InitializePlayerFace();
        }
    }

    private void InitializePlayerFace()
    {
        sides.Add("LeftSide", side1);
        sides.Add("RightSide", side2);
        sides.Add("TopSide", side3);

        FS1.isLeft = true;
        FS2.isRight = true;
        FS3.isTop = true;

        PS.SetCurrentFace(gameObject);

        SetSideMaterial(FS1, materialLeftFace);
        SetSideMaterial(FS2, materialRightFace);
        SetSideMaterial(FS3, materialTopFace);

        NHS.SetNavigationHint(FS1);
        NHS.SetNavigationHint(FS2);
        NHS.SetNavigationHint(FS3);
    }

    private void SetSideMaterial(FaceScript face, Material material)
    {
        if (!face.isKilling) face.rend.material = material;
    }

    private void Update()
    {
        if (isTurnOn && havePlayer && !transferInProgress && BC.canPress)
        {
            HandleInput();
        }

        if (havePlayer && isBonus && BSS != null)
        {
            HandleBonus();
        }

        if (havePlayer && isPortal && PSS != null)
        {
            HandlePortal();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(keyLeft) || Input.GetKeyDown(keyTop) || Input.GetKeyDown(keyRight))
        {
            string direction = GetDirectionFromInput();
            if (!string.IsNullOrEmpty(direction))
            {
                StartTransfer(GetGameObject($"{direction}Side"), direction);
                BC.isAlreadyPressed = true;
                BC.isAlreadyPressedIsAlreadyPressed = false;
                SS.TurnOnSoundStep();
            }
        }
    }

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
    }

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
        if (!targetFace.havePlayer)
        {
            havePlayer = false;
            targetFace.ReceivePlayer(player, this.gameObject, color, GetOtherGameObjects(color));
        }
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


        if (colorPrevious == "Right")
        {
            faceScriptPrevious.isRight = true;
        } 
        else if (colorPrevious == "Left")
        {
            faceScriptPrevious.isLeft = true;
        }
        else if (colorPrevious == "Top")
        {
            faceScriptPrevious.isTop = true;
        }

        if (side1 == sidePrevious)
        {
            UpdateSidesBasedOnPrevious(side2, side3, sidesPreviousOther);
        } 
        else if (side2 == sidePrevious)
        {
            UpdateSidesBasedOnPrevious(side1, side3, sidesPreviousOther);
        } 
        else if (side3 == sidePrevious)
        {
            UpdateSidesBasedOnPrevious(side1, side2, sidesPreviousOther);
        }


        ResetOtherSides(sidesPreviousOther);

        havePlayer = true;
        PS.ResetMaterials();
        PCS.SetPathCount();
        PS.SetCurrentFace(gameObject);

        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        newPlayer.transform.localRotation = Quaternion.identity;

        NHS.SetNavigationHint(FS1);
        NHS.SetNavigationHint(FS2);
        NHS.SetNavigationHint(FS3);

        foreach (var pair in sides)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value.name}");
        }
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

        float distance1 = Vector3.Distance(transformSideOne.position, sidesPreviousOther[0].transform.position);
        float distance2 = Vector3.Distance(transformSideOne.position, sidesPreviousOther[1].transform.position);
        float distance3 = Vector3.Distance(transformSideTwo.position, sidesPreviousOther[0].transform.position);
        float distance4 = Vector3.Distance(transformSideTwo.position, sidesPreviousOther[1].transform.position);

        Debug.Log(distance1.ToString() + "\n" + distance2.ToString() + "\n" + distance3.ToString() + "\n" + distance4.ToString());

        if (distance1 < distance2 && distance3 > distance4)
        {
            UpdateSide(sideOne, sidesPreviousOther[0], sideTwo, sidesPreviousOther[1]);
        }
        else if (distance1 > distance2 && distance3 < distance4)
        {
            UpdateSide(sideOne, sidesPreviousOther[1], sideTwo, sidesPreviousOther[0]);
        }
        else
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        }
    }

    private void UpdateSide(GameObject sideA, GameObject sidePreviousA, GameObject sideB, GameObject sidePreviousB)
    {
        FaceScript faceScriptSideA = sideA.GetComponent<FaceScript>();
        FaceScript faceScriptSideB = sideB.GetComponent<FaceScript>();
        FaceScript faceScriptSidePreviousA = sidePreviousA.GetComponent<FaceScript>();
        FaceScript faceScriptSidePreviousB = sidePreviousB.GetComponent<FaceScript>();


        if (faceScriptSidePreviousA.isRight)
        {
            sides.Add("RightSide", sideA);
            SetSideMaterial(faceScriptSideA, materialRightFace);
            faceScriptSideA.isRight = true;
        }
        else if (faceScriptSidePreviousA.isLeft)
        {
            sides.Add("LeftSide", sideA);
            SetSideMaterial(faceScriptSideA, materialLeftFace);
            faceScriptSideA.isLeft = true;
        }
        else if (faceScriptSidePreviousA.isTop)
        {
            sides.Add("TopSide", sideA);
            SetSideMaterial(faceScriptSideA, materialTopFace);
            faceScriptSideA.isTop = true;
        }

        if (faceScriptSidePreviousB.isRight)
        {
            sides.Add("RightSide", sideB);
            SetSideMaterial(faceScriptSideB, materialRightFace);
            faceScriptSideB.isRight = true;
        }
        else if (faceScriptSidePreviousB.isLeft)
        {
            sides.Add("LeftSide", sideB);
            SetSideMaterial(faceScriptSideB, materialLeftFace);
            faceScriptSideB.isLeft = true;
        }
        else if (faceScriptSidePreviousB.isTop)
        {
            sides.Add("TopSide", sideB);
            SetSideMaterial(faceScriptSideB, materialTopFace);
            faceScriptSideB.isTop = true;
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
    }

    public void ResetRightLeftTop()
    {
        if (isLeft) rend.material = materialLeftFace;
        else if (isRight) rend.material = materialRightFace;
        else if (isTop) rend.material = materialTopFace;
    }

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
            if (pair.Key != key)
            {
                otherObjects[index] = pair.Value;
                index++;

                if (index == 2)
                    break;
            }
        }
        return otherObjects;
    }
}