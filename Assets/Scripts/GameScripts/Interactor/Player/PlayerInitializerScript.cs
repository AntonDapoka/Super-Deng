using UnityEngine;

public class PlayerInitializerScript : MonoBehaviour
{
    [SerializeField] private PlayerSetterScript playerSetter;
    [SerializeField] private PlayerStateInteractorScript playerScript;
    [SerializeField] private PlayerBeatSyncValidatorScript playerBeatSyncValidator;
    [SerializeField] private FaceArrayScript faceArray;
    [SerializeField] private PathCounterScript pathCounter;

    public void InitializePlayer(float beatInterval)
    {
        GameObject face = faceArray.GetRandomFace(); //Rewrite!!!!!
        playerSetter.SetPlayer(face);
        playerScript.SetCurrentFace(face);
        playerBeatSyncValidator.Initialize(beatInterval); //Add data
        pathCounter.StartPathCount();
    }
}
