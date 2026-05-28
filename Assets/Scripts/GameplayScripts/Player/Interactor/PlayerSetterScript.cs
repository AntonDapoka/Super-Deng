using UnityEngine;

public class PlayerSetterScript : MonoBehaviour
{
    [SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;

    public void SetPlayer(GameObject player, GameObject face)
    {
        playerMovementInteractor.SetPlayer(player);
        playerMovementInteractor.InitializePlayerFace(face.GetComponent<FaceScript>());
    }
}
