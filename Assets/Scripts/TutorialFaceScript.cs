using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// Обязательное уведомление: "Правые", "Левые" и "Верхние" указаны для треугольника с основанием, направленным ВНИЗ!!!
// Обратите внимание, что "Правая" сторона раньше носила название "BlueSide", "Левая" - "OrangeSide", а "Верхняя" - "GreenSide"
// Помимо прочих наименований, "Правая" сторона может записываться как "Side2", "Левая" - "Side1", а "Верхняя" - "Side3"
public class TutorialFaceScript : MonoBehaviour
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
    [SerializeField] private PlayerScript PS;

    [Space]
    [Header("Side of the Face")]
    [SerializeField] private GameObject prefabFace;
    private GameObject side1; 
    private GameObject side2;
    private GameObject side3; 
    public Dictionary<string, GameObject> sides;

    [HideInInspector] public TutorialFaceScript FS1;
    [HideInInspector] public TutorialFaceScript FS2;
    [HideInInspector] public TutorialFaceScript FS3;

    [Space]
    [Header("Materials")]
    [SerializeField] private Material materialBasicFace;
    [SerializeField] private Material materialKillerFace;
    [SerializeField] private Material materialPlayerFace;
    public Material materialRightFace;
    public Material materialLeftFace;
    public Material materialTopFace;

    [Space]
    [Header("Glowing&Rendering")]
    public MeshRenderer rend;
    public GameObject glowingPart;

    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;

    [Space]
    [Header("Scene ScriptManagers")]
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private BeatController BC;
    [SerializeField] private SoundScript SS;
    [SerializeField] private BonusSpawnerScript BSS;
    [SerializeField] private NavigationHintScript NHS;

    [Space]
    [Header("Questions")]
    public bool havePlayer = false;
    private bool transferInProgress = false;
    public bool isKilling = false;
    public bool isColored = false;
    public bool isBlocked = false;
    public bool isTurnOffNavigation = false;
    public bool isBonus = false;
    public bool isTutorial = true;
    public bool isLeft = false;
     public bool isRight = false;
     public bool isTop = false;

    
    private void Start()
    {
        rend = glowingPart.GetComponent<MeshRenderer>();
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));

        if (havePlayer)
        {
            side1 = Instantiate(prefabFace, transform);
            side2 = Instantiate(prefabFace, transform);
            side3 = Instantiate(prefabFace, transform);
            
            side1.transform.SetLocalPositionAndRotation(new Vector3(-1, 0, -0.5f), Quaternion.Euler(0, 180, 0));
            side2.transform.SetLocalPositionAndRotation(new Vector3(1, 0, -0.5f), Quaternion.Euler(0, 180, 0));
            side3.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.15f), Quaternion.Euler(0, 180, 0));

            side1.transform.parent = null;
            side2.transform.parent = null;
            side3.transform.parent = null;

            FS1 = side1.GetComponent<TutorialFaceScript>();
            FS2 = side2.GetComponent<TutorialFaceScript>();
            FS3 = side3.GetComponent<TutorialFaceScript>();

            TransfetDataToNewFaces(FS1, FS2, FS3);
        }
        sides = new Dictionary<string, GameObject>();

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
        /*
        foreach (var pair in sides)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value.name}");
        }*/
        FS1.isLeft = true;
        FS2.isRight = true;
        FS3.isTop = true;

        FS1.isBlocked = true;
        FS2.isBlocked = true;
        FS3.isBlocked = false;

        PS.SetCurrentFace(gameObject);

        SetSideMaterial(FS1, materialLeftFace);
        SetSideMaterial(FS2, materialRightFace);
        SetSideMaterial(FS3, materialTopFace);

        NHS.SetNavigationHintTutorial(FS1);
        NHS.SetNavigationHintTutorial(FS2);
        NHS.SetNavigationHintTutorial(FS3);
    }
 

    private void SetSideMaterial(TutorialFaceScript face, Material material)
    {
        if (!face.isKilling) face.rend.material = material;
    }
    
    private void TransfetDataToNewFaces(TutorialFaceScript faceScript1, TutorialFaceScript faceScript2, TutorialFaceScript faceScript3)
    {
        faceScript1.ReceiveDataToNewFaces(player, PS, prefabFace, FAS, RFS, BC, SS, BSS, NHS);
        faceScript2.ReceiveDataToNewFaces(player, PS, prefabFace, FAS, RFS, BC, SS, BSS, NHS);
        faceScript3.ReceiveDataToNewFaces(player, PS, prefabFace, FAS, RFS, BC, SS, BSS, NHS);
    }

    public void ReceiveDataToNewFaces(GameObject _player, PlayerScript _PS, GameObject _prefabFace,
        FaceArrayScript _FAS, RedFaceScript _RFS, BeatController _BC, SoundScript _SS, 
        BonusSpawnerScript _BSS, NavigationHintScript _NHS)
    {
        isTurnOn = true;
        isTutorial = true;
        player = _player;
        PS = _PS; 
        prefabFace = _prefabFace;
        FAS = _FAS;
        RFS = _RFS;
        BC = _BC;
        SS = _SS;
        BSS = _BSS;
        NHS = _NHS;
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
        if (Input.GetKeyDown(keyLeft) && !GetGameObject("LeftSide").GetComponent<TutorialFaceScript>().isBlocked)
        {
            return "Left";
        }
        else if (Input.GetKeyDown(keyTop) && !GetGameObject("TopSide").GetComponent<TutorialFaceScript>().isBlocked)
        {
            return "Top";
        }
        else if (Input.GetKeyDown(keyRight) && !GetGameObject("RightSide").GetComponent<TutorialFaceScript>().isBlocked)
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
        TutorialFaceScript targetFace = targetSide.GetComponent<TutorialFaceScript>();

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

        side1 = Instantiate(prefabFace, transform);
        side2 = Instantiate(prefabFace, transform);
        side3 = Instantiate(prefabFace, transform);

        side1.transform.SetLocalPositionAndRotation(new Vector3(-1, 0, -0.5f), Quaternion.Euler(0, 180, 0));
        side2.transform.SetLocalPositionAndRotation(new Vector3(1, 0, -0.5f), Quaternion.Euler(0, 180, 0));
        side3.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1.15f), Quaternion.Euler(0, 180, 0));

        side1.transform.parent = null;
        side2.transform.parent = null;
        side3.transform.parent = null;


        FS1 = side1.GetComponent<TutorialFaceScript>();
        FS2 = side2.GetComponent<TutorialFaceScript>();
        FS3 = side3.GetComponent<TutorialFaceScript>();
        TransfetDataToNewFaces(FS1, FS2, FS3);

        TutorialFaceScript faceScriptPrevious;

        if (sidePrevious.transform.position == side1.transform.position)
        {
            sides.Add($"{colorPrevious}Side", side1);

            faceScriptPrevious = side1.GetComponent<TutorialFaceScript>();
            UpdateSidesBasedOnPrevious(side2, side3, sidesPreviousOther);

        }
        else if (sidePrevious.transform.position == side2.transform.position)
        {
            sides.Add($"{colorPrevious}Side", side2);
            faceScriptPrevious = side2.GetComponent<TutorialFaceScript>();
            UpdateSidesBasedOnPrevious(side1, side3, sidesPreviousOther);
        }
        else
        {
            sides.Add($"{colorPrevious}Side", side3);
            faceScriptPrevious = side3.GetComponent<TutorialFaceScript>();
            UpdateSidesBasedOnPrevious(side1, side2, sidesPreviousOther);
        }

        SetSideMaterial(faceScriptPrevious, GetMaterialForSide(colorPrevious));

        if (colorPrevious == "Right") { faceScriptPrevious.isRight = true; }
        else if (colorPrevious == "Left") { faceScriptPrevious.isLeft = true; }
        else if (colorPrevious == "Top") { faceScriptPrevious.isTop = true; }

        ResetOtherSides(sidesPreviousOther);

        havePlayer = true;
        PS.ResetMaterials();
        PS.SetCurrentFace(gameObject);

        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        newPlayer.transform.localRotation = Quaternion.identity;

        NHS.SetNavigationHintTutorial(FS1);
        NHS.SetNavigationHintTutorial(FS2);
        NHS.SetNavigationHintTutorial(FS3);

        /*foreach (var pair in sides)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value.name}");
        }*/

        Destroy(sidePrevious);
        Destroy(sidesPreviousOther[0]);
        Destroy(sidesPreviousOther[1]);
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
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }

    private void UpdateSide(GameObject side, GameObject sidePrevious)
    {
        TutorialFaceScript faceScriptSide = side.GetComponent<TutorialFaceScript>();
        TutorialFaceScript faceScriptSidePrevious = sidePrevious.GetComponent<TutorialFaceScript>();

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
            TutorialFaceScript faceScript = side.GetComponent<TutorialFaceScript>();
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
}