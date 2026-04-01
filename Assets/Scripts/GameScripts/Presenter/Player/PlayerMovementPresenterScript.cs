using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPresenterScript : MonoBehaviour, IPlayerMovementPresenterScript
{
    [SerializeField] private MonoBehaviour faceMaterialView;
    private IFaceMaterialViewScript FaceMaterialView => faceMaterialView as IFaceMaterialViewScript;
    [SerializeField] private PlayerMovementKeyBindingHintsPresenterScript playerMovementKeyBindingHintsPresenter;
    public Dictionary<string, MaterialType> materials = new();

    private void Awake()
    {
        materials.Add("RightSide", MaterialType.Right);
        materials.Add("LeftSide", MaterialType.Left);
        materials.Add("TopSide", MaterialType.Top);
    }

    public void UpdatePlayerSides(Dictionary<string, GameObject> sides, GameObject playerFace)
    {
        if (sides.Count != 3)
        {
            Debug.LogError("More than three sides!!!!");
            return;
        }
        

        foreach (var pair in sides)
        {
            if (materials.TryGetValue(pair.Key, out var mat))
            {
                FaceScript faceScript = pair.Value.GetComponent<FaceScript>();
                FaceStateScript faceState = faceScript.FaceState;

                if (!faceState.GetFaceState(FaceProperty.IsColored)
                    && !faceState.GetFaceState(FaceProperty.IsKilling)
                    && !faceState.GetFaceState(FaceProperty.IsBlinking)
                    && !faceState.GetFaceState(FaceProperty.IsBonus))
                {
                    FaceMaterialView.SetMaterial(faceScript.rend, mat);
                }
            }
            playerMovementKeyBindingHintsPresenter.SetNavigationHint(playerFace.transform, pair.Value.GetComponent<FaceScript>());
        }
    }

    public void UpdateNonPlayerSide(GameObject side)
    {
        FaceScript faceScript = side.GetComponent<FaceScript>();
        FaceStateScript faceState = faceScript.FaceState;

        if (faceState.GetFaceState(FaceProperty.IsColored)
        || faceState.GetFaceState(FaceProperty.IsKilling)
        || faceState.GetFaceState(FaceProperty.IsBlinking)
        || faceState.GetFaceState(FaceProperty.IsBonus) /*dd*/)
        {
            return;
        }
        else FaceMaterialView.SetMaterial(faceScript.rend, MaterialType.Default);
    }
}
