using UnityEngine;

public class PlayerSetterScript : MonoBehaviour
{
    [SerializeField] private PlayerStateInteractorScript playerScript;
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;

    public void SetPlayer(GameObject face)
    {
        playerScript.SetCurrentFace(face);
        playerMovementInteractor.InitializePlayerFace(face.GetComponent<FaceScript>());
    }
}
