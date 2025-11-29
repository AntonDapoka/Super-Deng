using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInteractorScript : MonoBehaviour
{
    [SerializeField] private FieldAssemblerScript fieldAssembler;
    [SerializeField] private FieldDisassemblerScript fieldDisassembler;
    [SerializeField] private FieldReassemblerScript fieldReassembler;

    public void SetStartField(GameObject facePrefab, float faceSideLength, float faceScale)
    {
        fieldAssembler.SetStartField(facePrefab, faceSideLength, faceScale);
    }
}
