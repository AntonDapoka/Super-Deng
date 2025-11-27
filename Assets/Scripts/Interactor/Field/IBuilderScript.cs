using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilderScript 
{
    GameObject Holder { get; }

    void BuildField(GameObject facePrefab, float sideLength, float faceScale);
    GameObject[] GetField();
}
