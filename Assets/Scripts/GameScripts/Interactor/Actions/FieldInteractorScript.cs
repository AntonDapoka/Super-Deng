using UnityEngine;

public class FieldInteractorScript : MonoBehaviour
{
    private GameObject fieldHolder;
    [SerializeField] private FieldAssemblerScript fieldAssembler;
    [SerializeField] private FieldDisassemblerScript fieldDisassembler;
    [SerializeField] private FieldReassemblerScript fieldReassembler;

    public void SetStartField(GameObject facePrefab, float faceSideLength, float faceScale)
    {
        fieldAssembler.SetStartField(facePrefab, faceSideLength, faceScale);
        fieldHolder = fieldAssembler.GetFieldHolder();

    }
}
