using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementInteractorScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private MonoBehaviour movementPresenter;
    public IPlayerMovementPresenterScript presenter => movementPresenter as IPlayerMovementPresenterScript;
    [SerializeField] private PlayerBeatSyncValidatorScript beatSyncValidator;
    [SerializeField] private PathCounterScript pathCounter;
    [SerializeField] private PlayerStateInteractorScript playerStateInteractor;

    [SerializeField] private FaceScript playerFace;
    private FaceStateScript playerFaceState;

    private Dictionary<string, GameObject> sides = new();

    private bool isTurnOn = false;

    public void InitializePlayerFace(FaceScript face)
    {
        playerFace = face;
        playerFaceState = playerFace.FaceState;

        player.transform.SetParent(playerFace.transform);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        SetPlayerFace();
        presenter.UpdatePlayerSides(sides, playerFace.gameObject);
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }

    private void SetPlayerFace()
    {
        playerFaceState.SetFaceState(FaceProperty.HavePlayer, true);

        playerFace.FaceState.SetFaceState(FaceProperty.IsRight, false);
        playerFace.FaceState.SetFaceState(FaceProperty.IsTop, false);
        playerFace.FaceState.SetFaceState(FaceProperty.IsLeft, false);

        /*
        playerFace.side1.GetComponent<FaceScript>().FaceState.Set(FaceProperty.IsLeft, true);
        playerFace.side2.GetComponent<FaceScript>().FaceState.Set(FaceProperty.IsRight, true);
        playerFace.side3.GetComponent<FaceScript>().FaceState.Set(FaceProperty.IsTop, true);//IM SORRY WHAT???

        sides.Clear();
        sides.Add("LeftSide", playerFace.side1);
        sides.Add("RightSide", playerFace.side2);
        sides.Add("TopSide", playerFace.side3);*/

        VeryPoorlyThoughtOutVoidForSettingThePlayerNeighborSidesPleaseRewriteIt();

        playerStateInteractor.SetCurrentFace(playerFace.gameObject);

        /*
        NHS.SetNavigationHint(FS1);
        NHS.SetNavigationHint(FS2);
        NHS.SetNavigationHint(FS3);*/
    }

    private void VeryPoorlyThoughtOutVoidForSettingThePlayerNeighborSidesPleaseRewriteIt()
    {
        var sideObjects = new[]
        {
            playerFace.side1,
            playerFace.side2,
            playerFace.side3
        };

        Vector3 center = playerFace.transform.position;
        Vector3 normal = Vector3.zero;

        for (int i = 0; i < sideObjects.Length; i++)
        {
            for (int j = i + 1; j < sideObjects.Length; j++)
            {
                Vector3 a = sideObjects[i].transform.position - center;
                Vector3 b = sideObjects[j].transform.position - center;

                Vector3 cross = Vector3.Cross(a, b);
                if (cross.sqrMagnitude > 0.0001f)
                    normal += cross.normalized;
            }
        }

        normal.Normalize();

        if (normal.sqrMagnitude < 0.0001f)
            normal = Vector3.forward;

        Vector3 right = Vector3.ProjectOnPlane(Vector3.right, normal).normalized;

        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.ProjectOnPlane(Vector3.forward, normal).normalized;

        Vector3 up = Vector3.Cross(normal, right).normalized;

        var sideData = sideObjects.Select(side =>
        {
            Vector3 dir = side.transform.position - center;

            return new
            {
                Side = side,
                X = Vector3.Dot(dir, right),
                Y = Vector3.Dot(dir, up)
            };
        }).ToList();

        var leftSide = sideData.OrderByDescending(s => s.X).First().Side;
        var rightSide = sideData.OrderBy(s => s.X).First().Side;
        var topSide = sideData.OrderByDescending(s => s.Y).First().Side;

        leftSide.GetComponent<FaceScript>().FaceState.SetFaceState(FaceProperty.IsLeft, true);
        rightSide.GetComponent<FaceScript>().FaceState.SetFaceState(FaceProperty.IsRight, true);
        topSide.GetComponent<FaceScript>().FaceState.SetFaceState(FaceProperty.IsTop, true);

        sides.Add("LeftSide", leftSide);
        sides.Add("RightSide", rightSide);
        sides.Add("TopSide", topSide);
    }

    public void MovePlayer(string direction)
    {
        if (isTurnOn == true 
            && playerFace.IsTurnOn == true
            && playerFaceState.GetFaceState(FaceProperty.HavePlayer)
            && !playerFaceState.GetFaceState(FaceProperty.TransferInProgress)
            && beatSyncValidator.CanPress() == true
            )
        {
            StartTransferPlayer(GetGameObject($"{direction}Side"), direction);
            beatSyncValidator.RegisterPress();
            //SS.TurnOnSoundStep();
        }
    }

    private void StartTransferPlayer(GameObject targetSide, string direction)
    {
        playerFaceState.SetFaceState(FaceProperty.TransferInProgress, true);
        //ResetSideMaterials();
        StartCoroutine(TransferPlayer(targetSide, direction));
    }

    private IEnumerator TransferPlayer(GameObject targetSide, string direction)
    {
        yield return new WaitForSeconds(0.01f);

        playerFaceState.SetFaceState(FaceProperty.HavePlayer, false);
        playerFaceState.SetFaceState(FaceProperty.TransferInProgress, false);

        ReceivePlayer(player, targetSide, playerFace.gameObject, direction, GetOtherGameObjects(direction));
    }

    public void ReceivePlayer(GameObject newPlayer, GameObject sideCurrent, GameObject sidePrevious, string directionPrevious, GameObject[] sidesPreviousOther)
    {
        playerFace = sideCurrent.GetComponent<FaceScript>();
        playerFaceState = playerFace.FaceState;

        playerFaceState.SetFaceState(FaceProperty.IsRight, false);
        playerFaceState.SetFaceState(FaceProperty.IsTop, false);
        playerFaceState.SetFaceState(FaceProperty.IsLeft, false);

        sides.Clear();

        sides.Add($"{directionPrevious}Side", sidePrevious);

        FaceScript faceScriptPrevious = sidePrevious.GetComponent<FaceScript>();

        if (directionPrevious == "Right") { faceScriptPrevious.FaceState.SetFaceState(FaceProperty.IsRight, true); }
        else if (directionPrevious == "Left") { faceScriptPrevious.FaceState.SetFaceState(FaceProperty.IsLeft, true); }
        else if (directionPrevious == "Top") { faceScriptPrevious.FaceState.SetFaceState(FaceProperty.IsTop, true); }

        if (playerFace.side1 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side2, playerFace.side3, sidesPreviousOther); }
        else if (playerFace.side2 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side1, playerFace.side3, sidesPreviousOther); }
        else if (playerFace.side3 == sidePrevious) { UpdateSidesBasedOnPrevious(playerFace.side1, playerFace.side2, sidesPreviousOther); }

        ResetOtherSides(sidesPreviousOther);

        presenter.UpdatePlayerSides(sides, sideCurrent);

        playerFaceState.SetFaceState(FaceProperty.HavePlayer, true);
        if (pathCounter != null) pathCounter.SetPathCount();
        //if (KYSS != null) KYSS.beatsNoMoving = 0;


        newPlayer.transform.SetParent(sideCurrent.transform);
        newPlayer.transform.localPosition = Vector3.zero;
        //IsUpsideDown = !IsUpsideDown;
        newPlayer.transform.localRotation =  Quaternion.identity;

        //NHS.SetNavigationHint(FS1);
        //NHS.SetNavigationHint(FS2);
        //NHS.SetNavigationHint(FS3);

        playerStateInteractor.SetCurrentFace(sideCurrent);
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

        if (faceScriptSidePrevious.FaceState.GetFaceState(FaceProperty.IsRight))
        {
            sides["RightSide"] = side;
            faceScriptSide.FaceState.SetFaceState(FaceProperty.IsRight, true);
        }
        else if (faceScriptSidePrevious.FaceState.GetFaceState(FaceProperty.IsLeft))
        {
            sides["LeftSide"] = side;
            faceScriptSide.FaceState.SetFaceState(FaceProperty.IsLeft, true);
        }
        else if (faceScriptSidePrevious.FaceState.GetFaceState(FaceProperty.IsTop))
        {
            sides["TopSide"] = side;
            faceScriptSide.FaceState.SetFaceState(FaceProperty.IsTop, true);
        }
    }

    private void ResetOtherSides(GameObject[] sidesPreviousOther)
    {
        foreach (var side in sidesPreviousOther)
        {
            FaceScript faceScript = side.GetComponent<FaceScript>();
            faceScript.FaceState.SetFaceState(FaceProperty.IsRight, false);
            faceScript.FaceState.SetFaceState(FaceProperty.IsTop, false);
            faceScript.FaceState.SetFaceState(FaceProperty.IsLeft, false);
            presenter.UpdateNonPlayerSide(side);
        }
    }

    private GameObject GetGameObject(string key)
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

    public GameObject GetCurrentFace()
    {
        return playerFace.gameObject;
    }
}
