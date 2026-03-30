using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPresenterScript : MonoBehaviour, IPlayerMovementPresenterScript
{
    [SerializeField] private MonoBehaviour faceMaterialView;
    private IFaceMaterialViewScript FaceMaterialView => faceMaterialView as IFaceMaterialViewScript;
    public Dictionary<string, MaterialType> materials = new();

    private void Awake()
    {
        materials.Add("RightSide", MaterialType.Right);
        materials.Add("LeftSide", MaterialType.Left);
        materials.Add("TopSide", MaterialType.Top);
    }

    public void UpdatePlayerSides(Dictionary<string, GameObject> sides)
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

                //if (faceState.Get(FaceProperty.IsColored)
                    //|| faceState.Get(FaceProperty.IsKilling)
                    /*|| faceState.Get(FaceProperty.IsBlinking)
                    || faceState.Get(FaceProperty.IsBonusHealth) /*dd)
                //{
                    /*Debug.Log(pair.Key + " is colored: " + faceState.Get(FaceProperty.IsColored)  + " is killing: " + faceState.Get(FaceProperty.IsKilling));
                    return;
                //}
                else*/ FaceMaterialView.SetMaterial(faceScript.rend, mat);
            }
        }
    }

    public void UpdateNonPlayerSide(GameObject side)
    {
        FaceScript faceScript = side.GetComponent<FaceScript>();
        FaceStateScript faceState = faceScript.FaceState;

        if (faceState.Get(FaceProperty.IsColored)
        || faceState.Get(FaceProperty.IsKilling)
        || faceState.Get(FaceProperty.IsBlinking)
        || faceState.Get(FaceProperty.IsBonusHealth) /*dd*/)
        {
            return;
        }
        else FaceMaterialView.SetMaterial(faceScript.rend, MaterialType.Default);
    }
}
