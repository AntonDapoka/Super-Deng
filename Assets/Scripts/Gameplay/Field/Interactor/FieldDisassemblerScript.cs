using System.ComponentModel.DataAnnotations;
using UnityEngine;

public class FieldDisassemblerScript : MonoBehaviour
{
    [SerializeField] private FaceSetterScript faceSetter;
    [SerializeField] private FaceArrayScript faceArray;
    [SerializeField] private float impulseForce;
    [SerializeField, Range(0,1)] private float impulseRangeRandom;
    [SerializeField] private float torqueForce;

    public void DissambleField(FaceScript[] faces, transfrom impulsePoint, float faceScale)
    {
        for (int i = 0; i <= faces.Length; i++)
        {
            faces[i].FaceState.SetFaceState(FaceProperty.TransferInProgress, true);
            Rigidbody rb = faces[i].GetComponent<Rigidbody>();
            rb.AddForce(Random.onUnitSphere * impulseForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        }
        faceArray.SetAllFaces(Builder.GetField());
        faceSetter.InitializeAllFaces(Builder.GetField());
    }

    public GameObject GetFieldHolder()
    {
        return Builder.Holder;
    }
}
