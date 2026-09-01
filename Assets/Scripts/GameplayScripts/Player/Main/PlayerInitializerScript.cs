using UnityEngine;

public class PlayerInitializerScript : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    
    [Header("Controllers")]
    [SerializeField] private PlayerMovementControllerScript playerMovementController;
    [SerializeField] private PlayerAbilityControllerScript playerAbilityController;

    [Header("Interactors")]
    [SerializeField] private ushort idFaceStart;
    [SerializeField] private PlayerSetterScript playerSetter;
    [SerializeField] private PlayerStateInteractorScript playerScript;
    [SerializeField] private PlayerBeatSyncValidatorScript playerBeatSyncValidator;
    [SerializeField] private FaceArrayScript faceArray;
    [SerializeField] private PathCounterScript pathCounter;
    [SerializeField] private IcosphereRotateToCameraScript cameraFollow;

    [Header("Presenters")]
    [SerializeField] private PlayerStatePresenterScript playerStatePresenter;
    [SerializeField] private PlayerMovementKeyBindingHintsPresenterScript playerMovementKeyBindingHintsPresenter;


    public void InitializePlayer(ushort idFace, MovementKeyBindingDataScript movementKeyBindingData, AbilityKeyBindingDataScript abilityKeyBindingData, float beatInterval)
    {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        idFaceStart = idFace;
        InitializeControllers(movementKeyBindingData, abilityKeyBindingData);
        InitializePresentors(movementKeyBindingData, abilityKeyBindingData);
        InitializeInteractors(beatInterval);
    }

    private void InitializeControllers(MovementKeyBindingDataScript movementKeyBindingData, AbilityKeyBindingDataScript abilityKeyBindingData)
    {
        playerMovementController.SetKeyBindings(movementKeyBindingData);
        playerMovementController.SetAbilityKeyBindings(abilityKeyBindingData);
        playerAbilityController.SetKeyBindings(abilityKeyBindingData);
    }

    private void InitializeInteractors(float beatInterval)
    {
        GameObject face = faceArray.GetFaceByID(idFaceStart);
        playerSetter.SetPlayer(player, face);
        playerScript.SetCurrentFace(face);
        playerBeatSyncValidator.Initialize(beatInterval); //Add data
        pathCounter.StartPathCount();

        cameraFollow.Initialize(player.transform);
    }

    private void InitializePresentors(MovementKeyBindingDataScript movementKeyBindingData, AbilityKeyBindingDataScript abilityKeyBindingData)
    {
        PlayerReferencesHolderScript referencesHolder = player.GetComponent<PlayerReferencesHolderScript>();
        playerStatePresenter.Initialize(referencesHolder);
        playerMovementKeyBindingHintsPresenter.SetKeyBindings(movementKeyBindingData);
        playerMovementKeyBindingHintsPresenter.TurnOn();
        playerMovementKeyBindingHintsPresenter.Initialize(referencesHolder.NavigationHints);
    }
}
