using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInitializerScript : MonoBehaviour
{
    [SerializeField] private FieldInteractorScript fieldInteractor;
    [SerializeField] private GameObject facePrefab;
    [SerializeField] private float faceSideLength;
    [SerializeField] private float faceScale;
    //[SerializeField] private FieldData fieldData;

    public void InitializeField() //AddData
    {
        fieldInteractor.SetStartField(facePrefab, faceSideLength, faceScale);
    }
}
