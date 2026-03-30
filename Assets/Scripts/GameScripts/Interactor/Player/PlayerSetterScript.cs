using UnityEngine;

public class PlayerSetterScript : MonoBehaviour
{
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;

    public void SetPlayer(GameObject face)
    {
        playerMovementInteractor.InitializePlayerFace(face.GetComponent<FaceScript>());
    }
}
