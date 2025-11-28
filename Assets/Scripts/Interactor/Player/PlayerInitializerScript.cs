using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializerScript : MonoBehaviour
{
    [SerializeField] private PlayerSetterScript playerSetter;
    [SerializeField] private FaceArrayScript faceArray;

    public void InitializePlayer()
    {
        GameObject face = faceArray.GetRandomFace(); //Rewrite!!!!!
        playerSetter.SetPlayer(face);
    }
}
