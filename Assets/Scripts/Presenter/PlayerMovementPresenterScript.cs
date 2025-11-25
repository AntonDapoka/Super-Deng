using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPresenterScript : MonoBehaviour //, IPlayerMovementPresenterScript
{
    //[SerializeField] private PlayerMovementInteractorScript playerMovementInteractor;
    //public Dictionary<string, Material> materials = new();
    [SerializeField] private Material material;
    [SerializeField] private Renderer face;

    /*
    private void Start()
    {
        materials.Add("RightSide", materialRight);
        materials.Add("LeftSide", materialLeft);
        materials.Add("TopSide", materialTop);
    }

    public void UpdateSides(GameObject side)
    {
        /*
    if (sides.Count != 3)
    {
    Debug.LogError("More than three sides!!!!");
    return;
    }

    foreach (var pair in sides)
    {
    /*
    if (materials.TryGetValue(pair.Key, out var mat))
    {
        Debug.Log(pair.Key);
        pair.Value.GetComponent<FaceScript>().rend.material = mat;

        Debug.LogError(pair.Value.GetComponent<FaceScript>().rend.material);
    }
    pair.Value.GetComponent<FaceScript>().rend.material = materialLeft;
    }
        Debug.Log("Here");
        side.GetComponent<FaceScript>().rend.material = materialTop;
    }*/

    private void Update()
    {
        face.material = material;
    }
}
