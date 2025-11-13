using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class PlayerMovementInteractorScript : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private BeatController beatController;
    [SerializeField] private PathCounterScript pathCounter;

    [SerializeField] private FaceScript playerFace;
    private FaceStateScript playerFaceState;

    private Dictionary<string, GameObject> sides = new();

    private bool IsUpsideDown = true;

    private void Start()
    {
        InitializePlayerFace();
    }

    private void InitializePlayerFace()
    {
        playerFaceState = playerFace.FaceState;
        playerFaceState.HavePlayer = true;

        playerFace.FaceState.IsRight = false;
        playerFace.FaceState.IsTop = false;
        playerFace.FaceState.IsLeft = false;

        playerFace.FS1.FaceState.IsLeft = true;
        playerFace.FS2.FaceState.IsRight = true;
        playerFace.FS3.FaceState.IsTop = true;

        sides.Clear();
        sides.Add("LeftSide", playerFace.side1);
        sides.Add("RightSide", playerFace.side2);
        sides.Add("TopSide", playerFace.side3);

        /*
        PS.SetCurrentFace(gameObject);

        SetSideMaterial(FS1, materialLeftFace);
        SetSideMaterial(FS2, materialRightFace);
        SetSideMaterial(FS3, materialTopFace);

        NHS.SetNavigationHint(FS1);
        NHS.SetNavigationHint(FS2);
        NHS.SetNavigationHint(FS3);*/
    }

    public void MovePlayer(string direction)
    {
        Debug.Log(playerFaceState.HavePlayer);

        if (playerFace.isTurnOn == true
            && playerFaceState.HavePlayer == true
            && playerFaceState.TransferInProgress == false
            //&& beatController.canPress == true
            )
        {
            StartTransferPlayer(GetGameObject($"{direction}Side"), direction);
            beatController.isAlreadyPressed = true;
            beatController.isAlreadyPressedIsAlreadyPressed = false;
            //SS.TurnOnSoundStep();
        }
    }

    private void StartTransferPlayer(GameObject targetSide, string direction)
    {
        playerFaceState.TransferInProgress = true;
        //ResetSideMaterials();
        StartCoroutine(TransferPlayer(targetSide, direction));
    }

    private IEnumerator TransferPlayer(GameObject targetSide, string direction)
    {
        yield return new WaitForSeconds(0.01f);
        FaceScript targetFace = targetSide.GetComponent<FaceScript>();

        playerFaceState.HavePlayer = false;
        playerFaceState.TransferInProgress = false;

        ReceivePlayer(player, targetSide, playerFace.gameObject, direction, GetOtherGameObjects(direction));
    }

    public void ReceivePlayer(GameObject newPlayer, GameObject sideCurrent, GameObject sidePrevious, string directionPrevious, GameObject[] sidesPreviousOther)
    {

        playerFace = sideCurrent.GetComponent<FaceScript>();
        playerFaceState = playerFace.FaceState;

        playerFaceState.IsRight = false;
        playerFaceState.IsTop = false;
        playerFaceState.IsLeft = false;

        //playerFace.FS1.FaceState.IsLeft = true;
        //playerFace.FS2.FaceState.IsRight = true;
        //playerFace.FS3.FaceState.IsTop = true;

        sides.Clear();

        sides.Add($"{directionPrevious}Side", sidePrevious);

        FaceScript faceScriptPrevious = sidePrevious.GetComponent<FaceScript>();
        //SetSideMaterial(faceScriptPrevious, GetMaterialForSide(colorPrevious));

        if (directionPrevious == "Right") { faceScriptPrevious.FaceState.IsRight = true; }
        else if (directionPrevious == "Left") { faceScriptPrevious.FaceState.IsLeft = true; }
        else if (directionPrevious == "Top") { faceScriptPrevious.FaceState.IsTop = true; }

        if (playerFace.side1 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side2, playerFace.side3, sidesPreviousOther); }
        else if (playerFace.side2 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side1, playerFace.side3, sidesPreviousOther); }
        else if (playerFace.side3 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side1, playerFace.side2, sidesPreviousOther); }

        ResetOtherSides(sidesPreviousOther);

        playerFaceState.HavePlayer = true;
        //PS.ResetMaterials();
        if (pathCounter != null) pathCounter.SetPathCount();
        //if (KYSS != null) KYSS.beatsNoMoving = 0;
        //PS.SetCurrentFace(gameObject);

        newPlayer.transform.SetParent(sideCurrent.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        IsUpsideDown = !IsUpsideDown;
        newPlayer.transform.localRotation = IsUpsideDown ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        //NHS.SetNavigationHint(FS1);
        //NHS.SetNavigationHint(FS2);
        //NHS.SetNavigationHint(FS3);
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

        if (faceScriptSidePrevious.FaceState.IsRight)
        {
            sides.Add("RightSide", side);
            //SetSideMaterial(faceScriptSide, materialRightFace);
            faceScriptSide.FaceState.IsRight = true;
        }
        else if (faceScriptSidePrevious.FaceState.IsLeft)
        {
            sides.Add("LeftSide", side);
            //SetSideMaterial(faceScriptSide, materialLeftFace);
            faceScriptSide.FaceState.IsLeft = true;
        }
        else if (faceScriptSidePrevious.FaceState.IsTop)
        {
            sides.Add("TopSide", side);
            //SetSideMaterial(faceScriptSide, materialTopFace);
            faceScriptSide.FaceState.IsTop = true;
        }
    }

    private void ResetOtherSides(GameObject[] sidesPreviousOther)
    {
        foreach (var side in sidesPreviousOther)
        {
            FaceScript faceScript = side.GetComponent<FaceScript>();
            faceScript.FaceState.IsRight = false;
            faceScript.FaceState.IsTop = false;
            faceScript.FaceState.IsLeft = false;
        }
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
}
