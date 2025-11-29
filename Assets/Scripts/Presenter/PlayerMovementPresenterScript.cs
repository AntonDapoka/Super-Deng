using System.Collections;
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
                FaceMaterialView.SetMaterial(pair.Value.GetComponent<FaceScript>().rend, mat);
            }
            /*
            pair.Value.GetComponent<FaceScript>().rend.material = materialLeft;
            
                Debug.Log("Here");
                pair.Value.GetComponent<FaceScript>().rend.material = materialTop;*/
        }
    }

    public void UpdateNonPlayerSide(GameObject side)
    {
        FaceMaterialView.SetMaterial(side.GetComponent<FaceScript>().rend, MaterialType.Default);
    }
}
