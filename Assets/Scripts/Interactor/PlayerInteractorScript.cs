using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class PlayerInteractorScript : MonoBehaviour
{
    public GameObject player;
    public BeatController BC;
    public FaceScript playerFace;
    public FaceStateScript playerFaceState;
    public Dictionary<string, GameObject> sides;

    private void InitializePlayerFace()
    {
        sides.Clear();
        playerFaceState = playerFace.FaceState;

        sides.Add("LeftSide", playerFace.side1);
        sides.Add("RightSide", playerFace.side2);
        sides.Add("TopSide", playerFace.side3);

        playerFace.FS1.FaceState.IsLeft = true;
        playerFace.FS2.FaceState.IsRight = true;
        playerFace.FS3.FaceState.IsTop = true;

        isRight = false;
        isTop = false;
        isLeft = false;

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
        Debug.Log("Key: " + direction);

        if (playerFace.isTurnOn == true
            && playerFaceState.HavePlayer == true
            && playerFaceState.TransferInProgress == false
            && BC.canPress == true)
        {
            StartTransferPlayer(GetGameObject($"{direction}Side"), direction);
            BC.isAlreadyPressed = true;
            BC.isAlreadyPressedIsAlreadyPressed = false;
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

    public void ReceivePlayer(GameObject newPlayer, GameObject sideCurrent, GameObject sidePrevious, string colorPrevious, GameObject[] sidesPreviousOther)
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
        if (PCS != null) PCS.SetPathCount();
        if (KYSS != null) KYSS.beatsNoMoving = 0;
        PS.SetCurrentFace(gameObject);

        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        newPlayer.transform.localRotation = isUpsideDown ? Quaternion.identity : Quaternion.Euler(0, 180, 180);

        //NHS.SetNavigationHint(FS1);
        //NHS.SetNavigationHint(FS2);
        //NHS.SetNavigationHint(FS3);
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
