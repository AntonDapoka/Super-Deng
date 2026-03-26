using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetterScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;

    public void SetPlayer(GameObject face)
    {
        playerScript.SetCurrentFace(face);
        playerMovementInteractor.InitializePlayerFace(face.GetComponent<FaceScript>());
    }
}
