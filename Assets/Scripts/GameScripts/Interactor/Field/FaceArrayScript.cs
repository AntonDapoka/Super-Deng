using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceArrayScript : MonoBehaviour
{
    [SerializeField] private GameObject[] faces;

    public void SetAllFaces(GameObject[] newFaces)
    {
        faces = newFaces;
    }

    public GameObject[] GetAllFaces()
    {
        return faces;
    }

    public GameObject GetRandomFace()
    {
        return faces[Random.Range(0, faces.Length)];
    }

    public FaceScript[] GetAllFaceScripts()
    {
        FaceScript[] result = new FaceScript[faces.Length];
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i] != null)
            {
                result[i] = faces[i].GetComponent<FaceScript>();
            }
        }
        return result;
    }
}
