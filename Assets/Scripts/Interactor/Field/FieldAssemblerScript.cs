using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class FieldAssemblerScript : MonoBehaviour
{
    [SerializeField] private MonoBehaviour builder;
    [SerializeField] private FaceSetterScript faceSetter;
    [SerializeField] private FaceArrayScript faceArray;



    public IBuilderScript Builder => builder as IBuilderScript;

    public void SetStartField(GameObject facePrefab, float faceSideLength, float faceScale)
    {
        Builder.BuildField(facePrefab, faceSideLength, faceScale);
        faceArray.SetAllFaces(Builder.GetField());
        faceSetter.InitializeAllFaces(Builder.GetField());
    }
}
