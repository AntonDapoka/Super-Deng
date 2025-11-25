using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementInteractorScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private MonoBehaviour movementPresenter;
    public IPlayerMovementPresenterScript presenter => movementPresenter as IPlayerMovementPresenterScript;
    [SerializeField] private BeatController beatController;
    [SerializeField] private PathCounterScript pathCounter;

    [SerializeField] private FaceScript playerFace;
    private FaceStateScript playerFaceState;

    private Dictionary<string, GameObject> sides = new();

    private void Start()
    {
        InitializePlayerFace();
    }

    private void InitializePlayerFace()
    {
        playerFaceState = playerFace.FaceState;
        playerFaceState.Set(FaceProperty.HavePlayer, true);

        playerFace.FaceState.Set(FaceProperty.IsRight, false);
        playerFace.FaceState.Set(FaceProperty.IsTop, false);
        playerFace.FaceState.Set(FaceProperty.IsLeft, false);

        playerFace.side1.GetComponent<FaceScript>().FaceState.Set(FaceProperty.IsLeft, true);
        playerFace.side2.GetComponent<FaceScript>().FaceState.Set(FaceProperty.IsRight, true);
        playerFace.side3.GetComponent<FaceScript>().FaceState.Set(FaceProperty.IsTop, true);

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
        Debug.Log(playerFaceState.Get(FaceProperty.HavePlayer));

        if (playerFace.IsTurnOn == true
            && playerFaceState.Get(FaceProperty.HavePlayer)
            && !playerFaceState.Get(FaceProperty.TransferInProgress)
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
        playerFaceState.Set(FaceProperty.TransferInProgress, true);
        //ResetSideMaterials();
        StartCoroutine(TransferPlayer(targetSide, direction));
    }

    private IEnumerator TransferPlayer(GameObject targetSide, string direction)
    {
        yield return new WaitForSeconds(0.01f);
        FaceScript targetFace = targetSide.GetComponent<FaceScript>();

        playerFaceState.Set(FaceProperty.HavePlayer, false);
        playerFaceState.Set(FaceProperty.TransferInProgress, false);

        ReceivePlayer(player, targetSide, playerFace.gameObject, direction, GetOtherGameObjects(direction));
    }

    public void ReceivePlayer(GameObject newPlayer, GameObject sideCurrent, GameObject sidePrevious, string directionPrevious, GameObject[] sidesPreviousOther)
    {

        playerFace = sideCurrent.GetComponent<FaceScript>();
        playerFaceState = playerFace.FaceState;

        playerFaceState.Set(FaceProperty.IsRight, false);
        playerFaceState.Set(FaceProperty.IsTop, false);
        playerFaceState.Set(FaceProperty.IsLeft, false);

        //playerFace.FS1.FaceState.IsLeft = true;
        //playerFace.FS2.FaceState.IsRight = true;
        //playerFace.FS3.FaceState.IsTop = true;

        sides.Clear();

        sides.Add($"{directionPrevious}Side", sidePrevious);

        FaceScript faceScriptPrevious = sidePrevious.GetComponent<FaceScript>();
        //SetSideMaterial(faceScriptPrevious, GetMaterialForSide(colorPrevious));

        if (directionPrevious == "Right") { faceScriptPrevious.FaceState.Set(FaceProperty.IsRight, true); }
        else if (directionPrevious == "Left") { faceScriptPrevious.FaceState.Set(FaceProperty.IsLeft, true); }
        else if (directionPrevious == "Top") { faceScriptPrevious.FaceState.Set(FaceProperty.IsTop, true); }

        if (playerFace.side1 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side2, playerFace.side3, sidesPreviousOther); }
        else if (playerFace.side2 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side1, playerFace.side3, sidesPreviousOther); }
        else if (playerFace.side3 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side1, playerFace.side2, sidesPreviousOther); }

        ResetOtherSides(sidesPreviousOther);

        presenter.UpdateSides(sidePrevious);

        playerFaceState.Set(FaceProperty.HavePlayer, true);
        //PS.ResetMaterials();
        if (pathCounter != null) pathCounter.SetPathCount();
        //if (KYSS != null) KYSS.beatsNoMoving = 0;
        //PS.SetCurrentFace(gameObject);

        newPlayer.transform.SetParent(sideCurrent.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        //IsUpsideDown = !IsUpsideDown;
        newPlayer.transform.localRotation =  Quaternion.identity;

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

        if (faceScriptSidePrevious.FaceState.Get(FaceProperty.IsRight))
        {
            sides["RightSide"] = side;
            //SetSideMaterial(faceScriptSide, materialRightFace);
            faceScriptSide.FaceState.Set(FaceProperty.IsRight, true);
        }
        else if (faceScriptSidePrevious.FaceState.Get(FaceProperty.IsLeft))
        {
            sides["LeftSide"] = side;
            //SetSideMaterial(faceScriptSide, materialLeftFace);
            faceScriptSide.FaceState.Set(FaceProperty.IsLeft, true);
        }
        else if (faceScriptSidePrevious.FaceState.Get(FaceProperty.IsTop))
        {
            sides["TopSide"] = side;
            //SetSideMaterial(faceScriptSide, materialTopFace);
            faceScriptSide.FaceState.Set(FaceProperty.IsTop, true);
        }
    }

    private void ResetOtherSides(GameObject[] sidesPreviousOther)
    {
        foreach (var side in sidesPreviousOther)
        {
            FaceScript faceScript = side.GetComponent<FaceScript>();
            faceScript.FaceState.Set(FaceProperty.IsRight, false);
            faceScript.FaceState.Set(FaceProperty.IsTop, false);
            faceScript.FaceState.Set(FaceProperty.IsLeft, false);
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

    private GameObject[] GetOtherGameObjects(string key)
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
