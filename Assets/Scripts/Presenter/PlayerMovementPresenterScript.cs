using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPresenterScript : MonoBehaviour, IPlayerMovementPresenterScript
{
    //[SerializeField] private PlayerMovementMaterialChangerScript materialChanger; //Change to Interface
    public Dictionary<string, Material> materials = new();
    [SerializeField] private Material materialRight;
    [SerializeField] private Material materialLeft;
    [SerializeField] private Material materialTop;
    [SerializeField] private Material materialBasic;

    private void Awake()
    {
        materials.Add("RightSide", materialRight);
        materials.Add("LeftSide", materialLeft);
        materials.Add("TopSide", materialTop);
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
                Debug.Log(pair.Key);
                pair.Value.GetComponent<FaceScript>().rend.material = mat;

                //Debug.LogError(pair.Value.GetComponent<FaceScript>().rend.material);
            }
            /*
            pair.Value.GetComponent<FaceScript>().rend.material = materialLeft;
            
                Debug.Log("Here");
                pair.Value.GetComponent<FaceScript>().rend.material = materialTop;*/
        }
    }

    public void UpdateNonPlayerSide(GameObject side)
    {
        side.GetComponent<FaceScript>().rend.material = materialBasic;
    }
}
